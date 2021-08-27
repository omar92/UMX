
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

    [System.Serializable]
    public struct TileTypePrefabPair
    {
        public TileType tileType;
        public GameObject prefab;
    }

    private List<GameObject> createdTiles = new List<GameObject>();

    public void BuildWorld()
    {
        for (int i = 0; i < roundData.Value.map.tiles.Length; i++)
        {
            InstantiateTile(roundData.Value.map.tiles[i]);
        }
    }

    private void InstantiateTile(Tile tile)
    {
        Vector3 pos = tile.Cord;
        pos.x = tile.Cord.x * padding.x;
        pos.y = tile.Next * elevation;
        pos.z = tile.Cord.y * padding.y;

        createdTiles.Add(GameObject.Instantiate(GetTilePrefab(tile.Type), transform));
        createdTiles[createdTiles.Count - 1].transform.position = pos;
        createdTiles[createdTiles.Count - 1].name = tile.ToString();
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
