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

    [SerializeField] UnityEvent OnLoseTurn;

    [SerializeField] UnityEvent<int> OnPlayerWin;

    private bool isGameStarted;
    private void Awake()
    {
        roundData.Subscribe(OnROundDataChanges);
    }

    public void OnGameStart()
    {
        isGameStarted = true;
        ExcuteRoundData(roundData);
    }

    public void OnPlayerMovementFinish()
    {
        var player = roundData.Value.GetCurrentPlayer();
        var reachedTile = roundData.Value.map.GetTile(player.position);
        var newRoundData = HandlePlayerReachedTile(player, reachedTile, roundData.Value);
        roundData.Value = newRoundData;
    }

    public void OnUseDice()
    {
        if (isGameStarted)
        {
            var newRoundData = roundData.Value;
            newRoundData.diceValue = selectedDice;
            roundData.Value = newRoundData;
        }
    }

    public void OnSkippDice()
    {
        if (isGameStarted)
        {
            var newRoundData = roundData.Value;
            newRoundData.players[newRoundData.turn % newRoundData.players.Length].RemainingSkips--;
            newRoundData.players[newRoundData.turn % newRoundData.players.Length].isSkippedLastTurn = true;
            newRoundData.players[newRoundData.turn % newRoundData.players.Length].lastRoll = selectedDice;
            newRoundData.roundState = RoundState.turnStart;
            roundData.Value = newRoundData;
        }
    }

    private RoundData HandlePlayerReachedTile(player player, Tile reachedTile, RoundData newRoundData)
    {
        newRoundData.diceValue = -1;
        switch (reachedTile.Type)
        {
            case TileType.Tile:
                {

                    newRoundData.roundState = RoundState.turnStart;
                }
                break;

            case TileType.End:
                {
                    Debug.LogError($"Player{player.id} Won the game");
                    OnPlayerWin.Invoke(player.id);
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
            newroundData = HandleState(newroundData.roundState, newroundData);
            roundData.Value = newroundData;
        }
    }

    private RoundData HandleState(RoundState roundState, RoundData newroundData)
    {
        switch (roundState)
        {
            case RoundState.loading:
                newroundData.isStarted = true;
                newroundData.roundState = RoundState.turnStart;
                break;
            case RoundState.turnStart:
                newroundData = HandleTurnStartingState(newroundData);
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
        return newroundData;
    }

    private RoundData HandleTurnStartingState(RoundData newroundData)
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
            var player = newroundData.GetCurrentPlayer();
            if (player.lastRoll == newroundData.diceValue && player.lastRoll == 6)//players lose a turn if they roll 6 twice in a row
            {
                Debug.Log("player lost turn");
                OnLoseTurn.Invoke();
                newroundData.roundState = RoundState.turnStart;
            }
            else
            {
                var moveDistance = newroundData.diceValue;
                if (player.isSkippedLastTurn)
                {
                    moveDistance += player.lastRoll;
                    player.isSkippedLastTurn = false;
                }
                player.position = newroundData.map.Next(player.position, moveDistance);
                player.lastRoll = newroundData.diceValue;
                newroundData.players[newroundData.turn % newroundData.players.Length] = player;
                newroundData.roundState = RoundState.movement;
            }
        }
        else
        {
            Debug.LogWarning("incoming dice number invalid:" + newroundData.diceValue);
        }
        return newroundData;
    }

    private RoundData HandleMovementState(RoundData newroundData)
    {
        return newroundData;
    }
}
