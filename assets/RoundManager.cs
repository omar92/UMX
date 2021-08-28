using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using so;
using System;
using UnityEngine.Events;
using Map;

public class RoundManager : MonoBehaviour
{
    [Header("soVariables")]
    [SerializeField] RoundDataSO roundData;
    [SerializeField] IntSO selectedDice;
    [Header("UnityEvents")]
    [SerializeField] UnityEvent OnIdleState;
    [SerializeField] UnityEvent OnDiceState;
    [SerializeField] UnityEvent OnMovingState;


    private bool isGameStarted;
    private void Awake()
    {
        roundData.Subscribe(OnROundDataChanges);
        selectedDice.Subscribe(OnDiceChange);
    }

    public void OnGameStart()
    {
        isGameStarted = true;
        ExcuteRoundData(roundData);
    }

    public void OnPlayerMovementFinish()
    {
        var player = roundData.Value.players[roundData.Value.turn % roundData.Value.players.Length];
        var reachedTile = roundData.Value.map.GetTile(player.position);
        var newRoundData = HandlePlayerReachedTile(player, reachedTile, roundData.Value);
        roundData.Value = newRoundData;
    }

    private RoundData HandlePlayerReachedTile(player player, Tile reachedTile, RoundData newRoundData)
    {
        newRoundData.diceValue = -1;
        switch (reachedTile.Type)
        {
            case TileType.Tile:
                {

                    newRoundData.roundState = RoundState.idle;
                }
                break;

            case TileType.End:
                {
                    Debug.LogError($"Player{player.id} Won the game");
                }
                break;
            case TileType.ShortCut:
            case TileType.Pit:
                {
                    player.position = roundData.Value.map.GetTile(((Portal)roundData.Value.map.GetTile(player.position)).Target).Cord;
                    newRoundData.players[player.id] = player;
                }
                break;
            case TileType.Start:
            default:
                {
                    Debug.LogError($"Player{player.id} reached unwanted tile type: " + reachedTile.Type);
                }
                break;
        }
        return newRoundData;
    }

    private void OnROundDataChanges(RoundData newroundData)
    {
        ExcuteRoundData(newroundData);
    }

    private void ExcuteRoundData(RoundData newroundData)
    {
        if (isGameStarted)
        {
            Debug.Log("ExcuteRoundData");
            newroundData = HandleState(newroundData.roundState, newroundData);
            roundData.Value = newroundData;
        }
    }

    private RoundData HandleState(RoundState roundState, RoundData newroundData)
    {
        Debug.Log("roundState: " + roundState);
        //  if (localRoundData.roundState != roundState)
        {
            switch (roundState)
            {
                case RoundState.loading:
                    newroundData.isStarted = true;
                    newroundData.roundState = RoundState.idle;
                    break;
                case RoundState.idle:
                    newroundData = HandleIdleState(newroundData);
                    OnIdleState.Invoke();
                    break;
                case RoundState.Dice:
                    newroundData = HandleDiceState(newroundData);
                    OnDiceState.Invoke();
                    break;
                case RoundState.movement:
                    newroundData = HandleMovementState(newroundData);
                    OnMovingState.Invoke();
                    break;
                default:
                    Debug.LogError("Unhandle round State");
                    break;
            }
        }
        return newroundData;
    }

    private RoundData HandleIdleState(RoundData newroundData)
    {
        newroundData.turn++;
        selectedDice.Value = -1;
        newroundData.roundState = RoundState.Dice;
        return newroundData;
    }
    private RoundData HandleDiceState(RoundData newroundData)
    {
        if (newroundData.diceValue > 0)
        {
            var player = newroundData.players[newroundData.turn % newroundData.players.Length];
            player.lastRoll = newroundData.diceValue;
            player.position = newroundData.map.Next(player.position, newroundData.diceValue);

            newroundData.players[newroundData.turn % newroundData.players.Length] = player;
            newroundData.roundState = RoundState.movement;
        }
        return newroundData;
    }
    private void OnDiceChange(int diceVal)
    {
        if (isGameStarted)
        {
            var newRoundData = roundData.Value;
            newRoundData.diceValue = diceVal;
            roundData.Value = newRoundData;
        }
    }
    private RoundData HandleMovementState(RoundData newroundData)
    {
        return newroundData;
    }
}
