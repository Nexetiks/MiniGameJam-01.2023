using NoGround.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Teleport teleport = default;
    [SerializeField]
    private float shakeDuration = 0.2f;
    
    private int shakeCount = 0;

    private void OnEnable()
    {
        PlayerCharacter.Instance.HitPoints.OnDamageTaken += OnDamageTaken;
    }
    private void OnDisable()
    {
        PlayerCharacter.Instance.HitPoints.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(float damage, float remainingHitPoints)
    {
        StartCoroutine(ShakeScreen());
    }

    private IEnumerator ShakeScreen()
    {
        teleport.enabled = true;
        shakeCount += 1;
        yield return new WaitForSeconds(shakeDuration);
        shakeCount -= 1;
        if (shakeCount == 0)
        {
            teleport.enabled = false;
        }
    }
}
