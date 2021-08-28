using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinHandler : MonoBehaviour
{

    public Text text;
    public string WinMessage = "player # Won";
    public so.RoundDataSO roundData;
    // Start is called before the first frame update
    void OnEnable()
    {
        text.text = WinMessage;
        text.text = text.text.Replace("#", (roundData.Value.GetCurrentPlayer().id + 1).ToString());
    }

}
