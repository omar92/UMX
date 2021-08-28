using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TileHandler))]
public class PortalHandler : MonoBehaviour
{

    public UnityEvent OnPortalReady;

    TileHandler th;
    Position exitPos;
    [SerializeField] public TileHandler Exit;

    private void Awake()
    {
        th = GetComponent<TileHandler>();
    }

    private void Start()
    {
        var portlaData = (Portal)th.roundData.Value.map.GetTile(th.cord);
        exitPos = portlaData.Target;
        Exit = th.tilesMap.Value[exitPos.y][exitPos.x];
        OnPortalReady.Invoke();
    }
}
