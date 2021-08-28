using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipHandler : MonoBehaviour
{
    public so.RoundDataSO roundData;
    public Text text;

    public UnityEvent OnCanSkip;
    public UnityEvent OnCantSkip;

    private int currentPLayerRemainingSkipps;
    private void Awake()
    {
        roundData.Subscribe(OnRounDataChanges);
        currentPLayerRemainingSkipps = -1;
    }

    private void OnRounDataChanges(RoundData obj)
    {
        if (roundData.Value.isStarted)
        {
            currentPLayerRemainingSkipps = roundData.Value.GetCurrentPlayer().RemainingSkips;
            text.text = currentPLayerRemainingSkipps.ToString();
        }
    }

    public void OnDiceSelected()
    {
        if (currentPLayerRemainingSkipps > 0)
        {
            OnCanSkip.Invoke();
        }
        else
        {
            OnCantSkip.Invoke();
        }
    }

}
