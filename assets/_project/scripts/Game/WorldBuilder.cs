
using Map;
using SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldBuilder : MonoBehaviour
{
    [Header("Round Data")]
    public RoundDataSO roundData;
    [Header("Building options")]
    public float elevation;
    public Vector2 padding;
    [Header("Prefabs")]
    public TileTypePrefabPair[] tiles;

    [Header("Events")]
    public UnityEvent OnBuildComplete;

    [System.Serializable]
    public struct TileTypePrefabPair
    {
        public TileType tileType;
        public GameObject prefab;
    }

    private List<GameObject> sortedTiles = new List<GameObject>();
    // private GameObject[] unsortedTiles;
    public void BuildWorld()
    {
        int order = 0;
        // unsortedTiles = new GameObject[roundData.Value.map.tiles.Length];
        for (int i = 0; i < roundData.Value.map.tiles.Length; i = roundData.Value.map.tiles[i].Next)
        {
            var newTile = InstantiateTile(roundData.Value.map.tiles[i], order++);
            sortedTiles.Add(newTile);
            newTile.name = i.ToString();
            newTile.GetComponent<TileHandler>().tileData = roundData.Value.map.tiles[i];
            //   unsortedTiles[i] = newTile;
        }
        OnBuildComplete.Invoke();
    }

    private GameObject InstantiateTile(Tile tile, int order)
    {
        Vector3 pos = tile.Cord;
        pos.x = tile.Cord.x * padding.x;
        pos.y = order * elevation;
        pos.z = tile.Cord.y * padding.y;

        var newtile = GameObject.Instantiate(GetTilePrefab(tile.Type), transform);
        newtile.transform.position = pos;
        newtile.name = tile.ToString();
        return newtile;
    }

    private GameObject GetTilePrefab(TileType type)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].tileType == type)
            {
                return tiles[i].prefab;
            }
        }
        return null;
    }
}
