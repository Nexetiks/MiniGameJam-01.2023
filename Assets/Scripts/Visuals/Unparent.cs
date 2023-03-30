using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparent : MonoBehaviour
{
    [SerializeField]
    private Transform[] unparenterdChildren = default;
    private void Start()
    {
        foreach (var item in unparenterdChildren)
        {
            item.SetParent(null);
        }
    }
}
