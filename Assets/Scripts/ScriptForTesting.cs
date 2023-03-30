using System.Collections;
using Buildings.NormalBuildings.ShootingBuilding;
using NoGround.Buildings.Generator;
using UnityEngine;
using UnityEngine.Serialization;

public class ScriptForTesting : MonoBehaviour
{
    [SerializeField]
    private MainGenerator main;
    [FormerlySerializedAs("normal")]
    [SerializeField]
    private ShootingBuilding shooting;

    [ContextMenu("Test Spawn GENERATOR building")]
    public void TestSpawnMain()
    {
        StartCoroutine(BuildBuildingMain());
    }

    private IEnumerator BuildBuildingMain()
    {
        main.SelectBuildingToBuild();
        yield return new WaitForSeconds(1f);
        main.StartBuildingTheBuilding();
    }

    [ContextMenu("Test Spawn NORMAL building")]
    public void TestSpawnNormal()
    {
        StartCoroutine(BuildBuildingNormal());
    }

    private IEnumerator BuildBuildingNormal()
    {
        shooting.SelectBuildingToBuild();
        yield return new WaitForSeconds(1f);
        shooting.StartBuildingTheBuilding();
    }
}