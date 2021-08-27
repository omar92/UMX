using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Map;
using System;
using SO;

public class GameManager : MonoBehaviour
{

    [Header("worldSettings")]
    [SerializeField] Vector2 size;
    [SerializeField] int shortcutsNum;
    [SerializeField] int pitFallsNum;

    [Header("gameSettings")]
    [SerializeField] int playersNum;

    [Header("soVariables")]
    [SerializeField] RoundDataSO roundData;

    [Header("Events")]
    public UnityEvent OnStartGeneratingLevelData;
    public UnityEvent OnRoundDataGenerated;

    GameMap map;

    private void OnEnable()
    {
        OnStartGeneratingLevelData.Invoke();

        StartCoroutine(GenerateWorldCO(() =>
        {

            var _roundData = new RoundData(playersNum, map);
            roundData.Value = _roundData;




            OnRoundDataGenerated.Invoke();
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
