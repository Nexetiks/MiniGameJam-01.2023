using NoGround.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterVisuals : MonoBehaviour
{
    [SerializeField]
    private Transform[] unparentedChildren = default;
    [SerializeField]
    private Transform legFollower = default;
    [SerializeField]
    private Transform headPivot = default;
    [SerializeField]
    private Animator animator = default;
    [SerializeField]
    private ParticleSystem waveParticles = default;
    [SerializeField]
    private ParticleSystem rippleParticles = default;
    [SerializeField]
    private float movementAnimationStrength = default;
    [SerializeField]
    private float rotationAnimationStrength = default;
    [SerializeField]
    private float jumpAnimationStrength = default;
    [Range(0f, 1f)]
    [SerializeField]
    private float rotationStrength = default;
    [SerializeField]
    private float jumpStrength = default;
    [SerializeField]
    private AnimationCurve jumpCurve = default;
    [SerializeField]
    private DecalProjector shadowDecal = default;
    [SerializeField]
    private Color shadowDefault = default;
    [SerializeField]
    private Color shadowInAir = default;


    private float lastJumpHeight;
    private Vector3 targetDirection;



    private void Awake()
    {
        foreach (var item in unparentedChildren)
        {
            item.SetParent(null);
        }
        targetDirection = transform.forward;
    }
    private void FixedUpdate()
    {
        SetDirection(PlayerCharacter.Instance.Rigidbody.velocity);
    }
    private void LateUpdate()
    {

        PlayerCharacter instance = PlayerCharacter.Instance;

        ParticleSystem.EmissionModule waveEmission = waveParticles.emission;
        ParticleSystem.EmissionModule rippleEmission = rippleParticles.emission;
        if (instance.IsInAir && waveEmission.enabled)
        {
            waveEmission.enabled = false;
            rippleEmission.enabled = false;
        }
        else if (!instance.IsInAir && !waveEmission.enabled)
        {
            waveEmission.enabled = true;
            rippleEmission.enabled = true;
        }

        Vector2 targetPosition = instance.Rigidbody.position;
        float jumpHeight = instance.IsInAir ? jumpCurve.Evaluate(instance.CurrentJumpTime / instance.TotalJumpTime) : 0;
        transform.position = new Vector3(targetPosition.x, jumpHeight * jumpStrength, targetPosition.y);

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 legFollowerPosition = legFollower.position;
        Vector3 deltaPosition = headPivot.position - legFollowerPosition;
        Quaternion quaternion = Quaternion.LookRotation(-deltaPosition, forward);
        headPivot.rotation = quaternion;
        Vector3 flatDeltaPosition = deltaPosition;
        flatDeltaPosition.y = 0;
        float dot = Vector2.Dot(forward, flatDeltaPosition);
        float flatDeltaMagnitude = dot < 0 ? 0 : flatDeltaPosition.magnitude;
        animator.SetFloat("MovementBlend", Mathf.Clamp01(flatDeltaMagnitude * movementAnimationStrength));

        float angle = Vector3.SignedAngle(forward, targetDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection, Vector3.up), Mathf.Abs(angle) * rotationStrength);
        animator.SetFloat("HeadBlend", Mathf.Clamp(angle * rotationAnimationStrength, -1, 1));

        animator.SetBool("IsJumping", instance.IsInAir);
        animator.SetFloat("JumpVelocity", Mathf.Clamp((jumpHeight - lastJumpHeight) * jumpAnimationStrength, -1, 1));
        animator.SetFloat("HeadBlend", Mathf.Clamp(angle * rotationAnimationStrength, -1, 1));
        lastJumpHeight = jumpHeight;

        shadowDecal.material.SetColor("_Tint", instance.IsInAir ? shadowInAir : shadowDefault);
        //if (flatDeltaMagnitude > 0.05f)
        //{
        //    targetForward = flatDeltaPosition;
        //}
        //Vector3 forward = transform.forward;
        //forward.y = 0;
        //transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(targetForward, Vector3.up), rotationStrength);

        //animator.SetFloat("HeadBlend", Vector3.SignedAngle(lastForward, targetOffset, Vector3.up));
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
        {
            targetDirection = new Vector3(direction.x, 0, direction.y);
        }
    }
}
