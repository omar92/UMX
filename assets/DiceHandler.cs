using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceHandler : MonoBehaviour
{

    public int maxDice;
    public UnityEvent<int> onDiceSelected;



    public void RollDice()
    {
        onDiceSelected.Invoke(Random.Range(1, maxDice + 1));
    }


}
