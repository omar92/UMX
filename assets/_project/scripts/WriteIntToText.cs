using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WriteIntToText : MonoBehaviour
{

    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Write(int val)
    {
        text.text = val.ToString();
    }

}
