using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMap
{
    public Vector2 Size { get => size; }

    Vector2 size;
    [SerializeField] Tile[] map;
    Vector2 minSize = new Vector2(2, 2);

    private GameMap() { }
    public GameMap(Vector2 size, int shortcutsNum, int PitFallsNum)
    {
        this.size = new Vector2((int)size.x, (int)size.y);
        if (this.size.x < minSize.x || this.size.x < minSize.y)
        {
            Debug.LogError($"map size must be > {minSize.ToString()}");
        }
        else
        {
            GenerateMap(shortcutsNum, PitFallsNum);
        }
    }

    public new string ToString()
    {
        string mapText = "";
        int row = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (row != map[i].Cord.y)
            {
                mapText += "\n";
                row = (int)map[i].Cord.y;
            }

            mapText += map[i].ToString();
        }
        return mapText;
    }

    private void GenerateMap(int shortcutsNum, int PitFallsNum)
    {

        InitMap(this.size);
        GeneratePortals(shortcutsNum, PitFallsNum);
    }

    private void GeneratePortals(int shortcutsNum, int pitFallsNum)
    {
        if ((shortcutsNum * 2 + pitFallsNum * 2) > (size.x * size.y - 2))
        {
            Debug.LogError("shortcuts and portals number must be less than the map tiles");
        }
        else
        {
            List<int> reserved = new List<int>();
            for (int i = 0; i < shortcutsNum; i++)
            {
                GeneratePortal("<", ref reserved, out int start, out int end);
                reserved.Add(start);
                reserved.Add(end);
                map[start] = new Portal(start, end, size);
                map[start].logIcon = "<color=green>[V]</color>";
                map[end].logIcon = "<color=green>[O]</color>";
            }
            for (int i = 0; i < pitFallsNum; i++)
            {
                GeneratePortal(">",ref reserved, out int start, out int end);
                reserved.Add(start);
                reserved.Add(end);
                map[start] = new Portal(start, end, size);
                map[start].logIcon = "<color=red>[^]</color>";
                map[end].logIcon = "<color=red>[O]</color>";
            }
        }
    }

    private void GeneratePortal(string rule, ref List<int> reserved, out int startIndex, out int endIndex)
    {
        Vector2 startCord;
        Vector2 EndCord;
        switch (rule)
        {
            case "<":
                {
                    do
                    {
                        int lastRowStartingTileIndex = (int)(size.x * (size.y - 1)) + 1;
                        do
                        {
                            startIndex = Random.Range(1, lastRowStartingTileIndex);
                        } while (reserved.Contains(startIndex));
                        startCord.x = (startIndex % (int)size.x);
                        startCord.y = startIndex / (int)size.x;
                        EndCord.x = startCord.x;
                        EndCord.y = Random.Range((int)startCord.y, (int)size.y - 1);
                        endIndex = (int)(EndCord.x + size.x * EndCord.y);
                    } while (reserved.Contains(endIndex));
                }
                break;
            case ">":
                {
                    do
                    {
                        int secoundRowStartingTileIndex = (int)(size.x);
                        do
                        {
                            startIndex = Random.Range(secoundRowStartingTileIndex, (int)(size.x * size.y) - 1);
                        } while (reserved.Contains(startIndex));
                        startCord.x = (startIndex % (int)size.x);
                        startCord.y = startIndex / (int)size.x;
                        EndCord.x = startCord.x;
                        EndCord.y = Random.Range(1, (int)startCord.y);
                        endIndex = (int)(EndCord.x + size.x * EndCord.y);
                    } while (reserved.Contains(endIndex));
                }
                break;
            default:
                startIndex = -1;
                endIndex = -1;
                break;
        }
    }

    private void InitMap(Vector2 size)
    {
        map = new Tile[(int)size.x * (int)size.y];
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new Tile(i, size);
        }

        map[0] = new Start(0, size);
        map[map.Length - 1] = new End(map.Length - 1, size);
    }
}

[System.Serializable]
class Tile
{

    public int Num { get => num; }
    [SerializeField] private int num;
    public Vector2 Cord { get => cord; }
    [SerializeField] private Vector2 cord;

    public string logIcon;
    public Tile(int num, Vector2 mapSize)
    {
        this.num = num;

        var x = (num % (int)mapSize.x);
        var y = num / (int)mapSize.x;
        cord = new Vector2(x, y);
        logIcon = "[_]";
    }
    public new virtual string ToString()
    {
        return logIcon;
    }
}

[System.Serializable]
class Portal : Tile
{
    public int Target { get => target; }
    [SerializeField] private int target;
    public Portal(int num, int target, Vector2 mapSize) : base(num, mapSize)
    {
        this.target = target;
        logIcon = "<color=blue>[O]</color>";
    }
}

[System.Serializable]
class Start : Tile
{
    public Start(int num, Vector2 mapSize) : base(num, mapSize)
    {
        logIcon = "[S]";
    }
}
[System.Serializable]
class End : Tile
{
    public End(int num, Vector2 mapSize) : base(num, mapSize)
    {
        logIcon = "[E]";
    }
}