using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseVisuals : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null, true);
        transform.position = new Vector3(transform.position.x, 0, transform.position.y);
    }
}
