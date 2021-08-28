using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Map;
using System;
using so;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    [Header("worldSettings")]
    [SerializeField] Vector2 size;
    [SerializeField] int shortcutsNum;
    [SerializeField] int pitFallsNum;

    [Header("GameSettings")]
    [SerializeField] int playerMaxSkipps;
    [Header("gameSettings")]
    [SerializeField] int playersNum;

    [Header("soVariables")]
    [SerializeField] RoundDataSO roundData;
    [Header("SOEvents")]
    public EventSO SpawnPlayers;
    [Header("Events")]
    public UnityEvent OnStartGeneratingLevelData;
    public UnityEvent OnRoundDataGenerated;
    public UnityEvent OnGameStart;
    GameMap map;

    private void OnEnable()
    {
        OnStartGeneratingLevelData.Invoke();

        StartCoroutine(GenerateWorldCO(() =>
        {
            var _roundData = new RoundData(playersNum, playerMaxSkipps, map);
            roundData.Value = _roundData;
            OnRoundDataGenerated.Invoke();
        }));


    }


    public void OnWorldBuildComplete()
    {
        SpawnPlayers.Raise();
    }

    public void OnPlayersSpawned()
    {
        OnGameStart.Invoke();
        var newROundData = roundData.Value;
        newROundData.isStarted = true;
        roundData.Value = newROundData;
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
