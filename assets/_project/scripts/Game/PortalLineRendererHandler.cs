using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PortalHandler))]
[RequireComponent(typeof(LineRenderer))]
public class PortalLineRendererHandler : MonoBehaviour
{
    public Vector3 offset;

    private PortalHandler ph;
    private LineRenderer lr;

    private void Awake()
    {
        ph = GetComponent<PortalHandler>();
        lr = GetComponent<LineRenderer>();
    }


    public void Draw()
    {
        lr.positionCount = 3;

        lr.SetPosition(0, transform.position + offset);

        var middlePos = Vector3.Lerp(transform.position + offset, ph.Exit.transform.position + offset, .5f);
        middlePos.y += offset.y;
        lr.SetPosition(1, middlePos);

        lr.SetPosition(2, ph.Exit.transform.position + offset);
    }
}
