using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldBuilder : MonoBehaviour
{

    [Header("worldSettings")]
    public Vector2 size;
    public int shortcutsNum;
    public int pitFallsNum;
    [Header("Events")]
    public UnityEvent OnStartBuilding;
    public UnityEvent OnFinishedBuilding;

    [SerializeField] GameMap map;


    private void OnEnable()
    {
        BuildWorld();
    }

    private void BuildWorld()
    {
        OnStartBuilding.Invoke();
        StartCoroutine(GenerateWorldCO(() =>
        {
            StartCoroutine(BuildSceneCO());
        }));
    }
    private IEnumerator GenerateWorldCO(Action onWorldGenerated)
    {
        yield return new WaitForEndOfFrame();
        map = new GameMap(size, shortcutsNum, pitFallsNum);
        Debug.Log(map.ToString());
        onWorldGenerated();
    }
    private IEnumerator BuildSceneCO()
    {
        yield return new WaitForEndOfFrame();
    }
}
