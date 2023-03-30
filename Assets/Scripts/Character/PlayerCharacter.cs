using System;
using System.Collections;
using NoGround.Pulses;
using NoGround.Utility;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Utility;

namespace NoGround.Character
{
    public class PlayerCharacter : Singleton<PlayerCharacter>, ITakePulse
    {
        [SerializeField]
        [Tooltip("Character rigidbody used for movement")]
        private new Rigidbody2D rigidbody;
        [SerializeField]
        [Tooltip("Sprite attached to character collider, used to debug the Jump feature")]
        private SpriteRenderer positionColliderSprite;
        [SerializeField]
        [Tooltip("Audio source for jump grunt")]
        private AudioClipRandomizer jumpCharacterAudio;
        [SerializeField]
        [Tooltip("Audio source for jump sfx")]
        private AudioClipRandomizer jumpAudio;
        [SerializeField]
        [Tooltip("Audio source for damage grunt")]
        private AudioClipRandomizer damageAudio;
        [SerializeField]
        [Tooltip("Audio source for dying sound")]
        private AudioClipRandomizer deadAudio;

        [SerializeField]
        [Tooltip("Movement modificator for X and Y axis")]
        private Vector2 moveForce = Vector2.one;
        [SerializeField]
        [Tooltip("How long character will be in air during the jump")]
        private float jumpTime = 2f;
        [SerializeField]
        [Tooltip("Time after which we will start building structure when we hold jump button")]
        private float buildActivationTime = 3f;

        [Tooltip("Character hit points")]
        [field: SerializeField]
        public HitPoints HitPoints { get; private set; } = new HitPoints(10);
        public Rigidbody2D Rigidbody => rigidbody;

        private PlayerInputActions playerInputActions;
        private bool isInAir = false;
        private float currentJumpTime = 0;
        private bool isTakingPulsePossibleAtTheMoment => !isInAir;

        public CountdownTrigger BuildTrigger;

        public bool IsInAir => isInAir;
        public float TotalJumpTime => jumpTime;
        public float CurrentJumpTime => currentJumpTime;
        public event Action OnAttacked;

        private void Awake()
        {
            BuildTrigger = new CountdownTrigger(buildActivationTime);
            BuildTrigger.OnTriggered += StartBuilding;
            BuildTrigger.OnDisable += StopBuilding;

            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Jump.started += context => Jump();

            playerInputActions.Player.Jump.performed += context =>
            {
                if (GameManager.Instance.IsAtTheEndOfStage) BuildTrigger.Enable();
            };

            playerInputActions.Player.Jump.canceled += context => BuildTrigger.Disable();

            HitPoints.Reset();

            HitPoints.OnDamageTaken += (damage, remain) => PlayDamageAudio();
            HitPoints.OnHitPointsDepleted += PlayDeadAudio;
        }

        private void OnEnable()
        {
            playerInputActions.Enable();

            if (GameManager.Instance != null)
            {
                RegisterInTakePulseList();
            }
        }

        private void OnDisable()
        {
            playerInputActions.Disable();

            if (GameManager.Instance != null)
            {
                UnRegisterInTakePulseList();
            }
        }

        private void Update()
        {
            BuildTrigger.Update(Time.deltaTime);

            // Move action should be updated every frame
            if (playerInputActions.Player.Move.IsPressed())
            {
                Move(playerInputActions.Player.Move.ReadValue<Vector2>());
            }
            else
            {
                Move(Vector2.zero);
            }
        }

        #region Input handling

        public void EnableCharacterInput(bool enable)
        {
            if (enable)
                playerInputActions.Player.Enable();
            else
                playerInputActions.Player.Disable();
        }

        private void Move(Vector2 directions)
        {
            // Debug.Log($"Move in directions {directions}");
            rigidbody.velocity = directions * moveForce;
        }

        private void Jump()
        {
            if (isInAir)
                return;

            StartCoroutine(InAirCoroutine());
        }

        private IEnumerator InAirCoroutine()
        {
            isInAir = true;
            currentJumpTime = 0;
            Color mainColor = positionColliderSprite.color;
            Color inAirColor = mainColor;
            inAirColor.a = 0.25f;
            positionColliderSprite.color = inAirColor;

            PlayJumpAudio();

            UpdateLayers(transform, LayerMask.NameToLayer("Air"));

            while (currentJumpTime < jumpTime)
            {
                currentJumpTime = Mathf.Min(currentJumpTime + Time.deltaTime, jumpTime);
                yield return null;
            }

            UpdateLayers(transform, LayerMask.NameToLayer("Ground"));

            positionColliderSprite.color = mainColor;

            isInAir = false;
            currentJumpTime = 0;
        }

        private void UpdateLayers(Transform objectTransform, int layer)
        {
            if (objectTransform.childCount > 0)
            {
                for (int i = 0; i < objectTransform.childCount; i++)
                {
                    UpdateLayers(transform.GetChild(i), layer);
                }
            }

            objectTransform.gameObject.layer = layer;
        }

        private void StartBuilding()
        {
            //Debug.Log($"Building!");
            GameManager.Instance.StartBuildingProcess(transform.position);
        }

        private void StopBuilding()
        {
            //Debug.Log($"Stop Building!");
            GameManager.Instance.InterruptBuildingProcess();
        }

        #endregion

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get => !isInAir;
            set { }
        }

        public void RegisterInTakePulseList()
        {
            GameManager.Instance.TakePulseList.Add(this);
        }

        public void UnRegisterInTakePulseList()
        {
            GameManager.Instance.TakePulseList.Remove(this);
        }

        public void TakePulse()
        {
            int damage = 1;
            TakeDamage(damage);
            Debug.Log($"Bzzzt, bzzzt! Pulse taken, damage = {damage}");
        }

        public void TakeDamage(int damage = 1)
        {
            HitPoints.TakeDamage(damage);
            OnAttacked?.Invoke();
        }

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }

        public float GetSpeedNormalized()
        {
            return rigidbody.velocity.magnitude / moveForce.x;
        }

        #region SFX

        private void PlayDeadAudio()
        {
            damageAudio.Stop();
            deadAudio.Play();
        }

        private void PlayJumpAudio()
        {
            jumpCharacterAudio.Play();
            jumpAudio.Play();
        }

        private void PlayDamageAudio()
        {
            damageAudio.Play();
        }

        #endregion
    }
}