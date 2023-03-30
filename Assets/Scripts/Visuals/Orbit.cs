using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField]
    private bool recalculateInUpdate = default;
    [SerializeField]
    private bool useOrbit = true;
    [SerializeField]
    private bool useRotation = true;

    [SerializeField]
    private float minOrbitSpeed = default;
    [SerializeField]
    private float maxOrbitSpeed = default;
    [SerializeField]
    private float minRotationSpeed = default;
    [SerializeField]
    private float maxRotationSpeed = default;

    [SerializeField]
    private Vector3 minOrbitDirection = default;
    [SerializeField]
    private Vector3 maxOrbitDirection = default;
    [SerializeField]
    private Vector3 minRotationDirection = default;
    [SerializeField]
    private Vector3 maxRotationDirection = default;


    private float orbitSpeed;
    private float rotationSpeed;
    private Quaternion orbitQuaternion;
    private Quaternion rotationQuaternion;

    private void Awake()
    {
        SetUp();
    }

    private void SetUp()
    {
        orbitSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
        orbitQuaternion = Quaternion.LerpUnclamped(Quaternion.identity, (Quaternion.Euler(
            Random.Range(minOrbitDirection.x, maxOrbitDirection.x), Random.Range(minOrbitDirection.y, maxOrbitDirection.y), Random.Range(minOrbitDirection.z, maxOrbitDirection.z))), orbitSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationQuaternion = Quaternion.LerpUnclamped(Quaternion.identity, (Quaternion.Euler(
            Random.Range(minRotationDirection.x, maxRotationDirection.x), Random.Range(minRotationDirection.y, maxRotationDirection.y), Random.Range(minRotationDirection.z, maxRotationDirection.z))), rotationSpeed);
    }

    void LateUpdate()
    {
        if (recalculateInUpdate)
            SetUp();
        if (useRotation)
            transform.localRotation = transform.localRotation * rotationQuaternion;
        if (useOrbit)
            transform.localPosition = orbitQuaternion * transform.localPosition;
    }
}
