using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DiceHandler : MonoBehaviour
{

    public int maxDice;
    public Text text;
    public UnityEvent<int> onDiceSelected;
    public so.RoundDataSO roundData;


    public void RollDice()
    {
        var diceVal = Random.Range(1, maxDice + 1);
        text.text = diceVal.ToString();
        if (roundData.Value.GetCurrentPlayer().isSkippedLastTurn)
        {
            text.text = $"{diceVal} + {roundData.Value.GetCurrentPlayer().lastRoll}";
        }
        onDiceSelected.Invoke(diceVal);
    }


}
