using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{

    [Header("worldSettings")]
    public Vector2 size;

    [SerializeField] GameMap map;

// Start is called before the first frame update
void Start()
    {
        map = new GameMap(new Vector2(8, 8), 3, 3);
        Debug.Log(map.ToString());

        map = new GameMap(new Vector2(8, 8), 3, 3);
        Debug.Log(map.ToString());

        map = new GameMap(new Vector2(8, 8), 3, 3);
        Debug.Log(map.ToString());

        map = new GameMap(new Vector2(8, 8), 3, 3);
        Debug.Log(map.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
