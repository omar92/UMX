using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UpdateTurnView : MonoBehaviour
{

    public so.RoundDataSO roundData;
    Text Text;
    private void Awake()
    {
        Text = GetComponent<Text>();
        roundData.Subscribe(OnRoundDataChanges);
    }

    private void OnRoundDataChanges(RoundData newRoundData)
    {

        Text.text = "" + ((newRoundData.turn % newRoundData.players.Length) + 1);

    }
}
