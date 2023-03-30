using NoGround.Buildings;
using NoGround.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(-1)]
public class TowerVisuals : MonoBehaviour
{
    [SerializeField]
    private Animator animator = default;
    [SerializeField]
    private DecalProjector radiusDecal = default;
    [SerializeField]
    private Color insideColor = default;
    [SerializeField]
    private Color outsideColor = default;

    private Building building;

    private void Awake()
    {
        building = GetComponentInParent<Building>();
        building.OnBuildingFinished += Building_OnBuildingFinished;
        transform.SetParent(null);
        transform.position = new Vector3(building.transform.position.x, 0, building.transform.position.y);
        radiusDecal.size = new Vector3(building.MaxPlayerDistanceToBuild * 2, building.MaxPlayerDistanceToBuild * 2, 10);
    }

    private void Building_OnBuildingFinished()
    {
        animator.SetBool("Builded", true);
        radiusDecal.enabled = false;
    }

    private void Update()
    {
        if (radiusDecal.enabled)
        {
            radiusDecal.material.SetColor("_Tint", building.CheckMaximumPlayerDistance() ? insideColor : outsideColor);
        }
    }
}
