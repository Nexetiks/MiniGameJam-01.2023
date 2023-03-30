using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float xSpeed = default;
    [SerializeField]
    private float ySpeed = default;
    [SerializeField]
    private float zSpeed = default;
    [SerializeField]
    private float xMagnitude = default;
    [SerializeField]
    private float yMagnitude = default;
    [SerializeField]
    private float zMagnitude = default;

    [SerializeField]
    private float xMaxTimeOffset = default;
    [SerializeField]
    private float yMaxTimeOffset = default;
    [SerializeField]
    private float zMaxTimeOffset = default;

    [SerializeField]
    private AnimationCurve x = default;
    [SerializeField]
    private AnimationCurve y = default;
    [SerializeField]
    private AnimationCurve z = default;

    private float xTimeOffset;
    private float yTimeOffset;
    private float zTimeOffset;

    private void Awake()
    {
        xTimeOffset = Random.Range(0, xMaxTimeOffset);
        yTimeOffset = Random.Range(0, yMaxTimeOffset);
        zTimeOffset = Random.Range(0, zMaxTimeOffset);
    }

    private void LateUpdate()
    {
        Vector3 position;
        position.x = x.Evaluate((Time.time * xSpeed + xTimeOffset) % x[x.length - 1].time);
        position.y = y.Evaluate((Time.time * ySpeed + yTimeOffset) % y[y.length - 1].time);
        position.z = z.Evaluate((Time.time * zSpeed + zTimeOffset) % z[z.length - 1].time);
        transform.localPosition = position;
    }

}
