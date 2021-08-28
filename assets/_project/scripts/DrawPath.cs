using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class DrawPath : MonoBehaviour
{

    public Vector3 linePosition;

    LineRenderer lr = null;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Draw()
    {
        lr.positionCount = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            lr.SetPosition(i, transform.GetChild(i).position + linePosition);
        }
    }

}
