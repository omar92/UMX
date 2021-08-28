using so;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHandler : MonoBehaviour
{
    public int id;
    public float speed;
    [Header("soVariabless")]
    public RoundDataSO roundData;
    public TilesMapSO tilesMap;
    // public IntSO currentTurn;

    [Header("Turn Events")]
    public UnityEvent OnMyTurnStart;
    public UnityEvent OnMyTurnEnds;

    [Header("Movement Events")]
    public UnityEvent OnMovementStart;
    public UnityEvent OnMoevementEnds;

    private bool isMyTurn;
    private player localPlayerData;
    Vector3 Offset;
    bool isOffsetCalculated = false;
    private void Awake()
    {
        roundData.Subscribe(OnRoundDataChanged);
        localPlayerData = roundData.Value.players[id];
    }
    private void Start()
    {

    }

    private void Update()
    {
    }
    private void OnRoundDataChanged(RoundData newRoundData)
    {
        if (newRoundData.isStarted)
        {
            HandleTurn(newRoundData.turn);
            if (newRoundData.roundState == RoundState.movement)
            {
                HandhleMovement(newRoundData.players[id].position);
            }
        }
    }

    private void HandhleMovement(Position newLocation)
    {
        if (!localPlayerData.position.IsEqual(newLocation))
        {
            MoveTo(newLocation);
            localPlayerData.position = newLocation;
        }
    }

    private void MoveTo(Position newLocation)
    {
        StartCoroutine(MoveToCo(newLocation));
    }

    private IEnumerator MoveToCo(Position newLocation)
    {
        Debug.Log("MoveToCo: " + newLocation);
        OnMovementStart.Invoke();


        var path = GeneratePathPontsTo(newLocation);

        if (!isOffsetCalculated)
        {
            Offset = transform.position - tilesMap.Value[localPlayerData.position.y][localPlayerData.position.x].transform.position;
        }

        for (int i = 1; i < path.Length; i++)
        {
            var targetPos = path[i].position + Offset;
            do
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            } while (Vector3.Distance(transform.position, targetPos) > .1);
        }

        OnMoevementEnds.Invoke();
    }

    private Transform[] GeneratePathPontsTo(Position newLocation)
    {
        List<Transform> points = new List<Transform>();
        if (roundData.Value.map.GetTile(localPlayerData.position).Type == Map.TileType.Pit || roundData.Value.map.GetTile(localPlayerData.position).Type == Map.TileType.ShortCut)
        {
            points.Add(tilesMap.Value[localPlayerData.position.y][localPlayerData.position.x].transform);
            points.Add(tilesMap.Value[newLocation.y][newLocation.x].transform);
        }
        else
        {
            for (Position pos = localPlayerData.position; !pos.IsEqual(newLocation); pos = roundData.Value.map.GetTile(pos).Next)
            {
                points.Add(tilesMap.Value[pos.y][pos.x].transform);
            }
            points.Add(tilesMap.Value[newLocation.y][newLocation.x].transform);
        }
        return points.ToArray();
    }

    private void HandleTurn(int turn)
    {
        if (turn % roundData.Value.players.Length == id)
        {
            if (!isMyTurn)
            {
                isMyTurn = true;
                OnMyTurnStart.Invoke();
            }
        }
        else
        {
            if (isMyTurn)
            {
                isMyTurn = false;
                OnMyTurnEnds.Invoke();
            }
        }
    }
}
