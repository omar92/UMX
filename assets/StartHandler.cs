using so;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TileHandler))]
public class StartHandler : MonoBehaviour
{

    [Header("propereties")]
    public Transform startPositionsGroup;
    public PlayerHandler[] PlayersPrefabs;

    [Header("soVariables")]
    public RoundDataSO roundData;

    [Header("soEvents")]
    public EventSO onPlayersSpawned;

    TileHandler th;

    private void Awake()
    {
        th = GetComponent<TileHandler>();
    }

    public void On_SpawnPlayers()
    {
        for (int i = 0; i < roundData.Value.players.Length; i++)
        {
            var player = GameObject.Instantiate(PlayersPrefabs[i % PlayersPrefabs.Length]);
            player.id = i;
            player.transform.position = startPositionsGroup.GetChild(i % PlayersPrefabs.Length).position;
        }
        onPlayersSpawned.Raise();
    }
}
