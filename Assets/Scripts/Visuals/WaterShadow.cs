using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShadow : MonoBehaviour
{
    public Transform follower = default;
    [SerializeField]
    private new Renderer renderer = default;

    private void LateUpdate()
    {
        renderer.material.SetVector("_ShadowPosition", follower.transform.position);
    }
}
