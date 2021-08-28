
using Map;
using SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldBuilder : MonoBehaviour
{
    [Header("soVariables")]
    public RoundDataSO roundData;
    public TilesMapSO tilesMapSO;
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


    public void BuildWorld()
    {
       
        InittilesMapSO();

        int order = 0;
        for (Position pos = new Position(0,0); PositionIsWithenBoundries(pos); pos = roundData.Value.map.tiles[pos.y][pos.x].Next)
        {
            var newTile = InstantiateTile(roundData.Value.map.tiles[pos.y][pos.x], order++);
            newTile.name = pos.ToString();
            newTile.GetComponent<TileHandler>().cord = pos;
            tilesMapSO.Value[pos.y][pos.x] = newTile;
        }

        OnBuildComplete.Invoke();
    }

    private void InittilesMapSO()
    {
        tilesMapSO.Value = new GameObject[roundData.Value.map.tiles.Length][];
        for (int y = 0; y < roundData.Value.map.tiles.Length; y++)
        {
            tilesMapSO.Value[y] = new GameObject[roundData.Value.map.tiles[y].Length];
        }
    }

    private bool PositionIsWithenBoundries(Position pos)
    {
        return ((pos.x < roundData.Value.map.Size.x) && ((pos.y < roundData.Value.map.Size.y)));
    }

    private GameObject InstantiateTile(Tile tile, int order)
    {
        Vector3 pos = new Vector3();
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
