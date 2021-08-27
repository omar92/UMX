using System;
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
        string mapText = "Portals\n\n";
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

        mapText += "\n\n";

        mapText += "Directions\n\n";
        row = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (row != map[i].Cord.y)
            {
                mapText += "\n";
                row = (int)map[i].Cord.y;
            }
            mapText += map[i].DirectionStr;
        }

        mapText += "\n\n";

        mapText += "Compined\n\n";
        row = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (row != map[i].Cord.y)
            {
                mapText += "\n";
                row = (int)map[i].Cord.y;
            }

            if (map[i].ToString() != "[_]")
                mapText += map[i].ToString();
            else
                mapText += map[i].DirectionStr;
        }

        mapText += "\n\n";

        mapText += "next\n\n";
        row = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (row != map[i].Cord.y)
            {
                mapText += "\n";
                row = (int)map[i].Cord.y;
            }

            mapText += $" [{map[i].Num:D3}:<color=green>{map[i].Next:D3}</color>] ";

        }

        return mapText;
    }

    private void GenerateMap(int shortcutsNum, int PitFallsNum)
    {
        List<int> reserved = new List<int>();
        InitMap(this.size, reserved);
        GeneratePortals(shortcutsNum, PitFallsNum, reserved);
    }

    private void GeneratePortals(int shortcutsNum, int pitFallsNum, List<int> reserved)
    {
        if ((shortcutsNum * 2 + pitFallsNum * 2) > (size.x * size.y - 2))
        {
            Debug.LogError("shortcuts and portals number must be less than the map tiles");
        }
        else
        {
            for (int i = 0; i < shortcutsNum; i++)
            {
                GeneratePortal("<", reserved, out int start, out int end);
                reserved.Add(start);
                reserved.Add(end);
                map[start] = new Portal(start, end, size);
                map[start].logIcon = "<color=blue>[V]</color>";
                map[end].logIcon = "<color=blue>[O]</color>";
            }
            for (int i = 0; i < pitFallsNum; i++)
            {
                GeneratePortal(">", reserved, out int start, out int end);
                reserved.Add(start);
                reserved.Add(end);
                map[start] = new Portal(start, end, size);
                map[start].logIcon = "<color=red>[^]</color>";
                map[end].logIcon = "<color=red>[O]</color>";
            }
        }
    }

    private void GeneratePortal(string rule, List<int> reserved, out int startIndex, out int endIndex)
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
                            startIndex = UnityEngine.Random.Range(0, lastRowStartingTileIndex);
                        } while (reserved.Contains(startIndex));
                        startCord.x = (startIndex % (int)size.x);
                        startCord.y = startIndex / (int)size.x;
                        EndCord.x = startCord.x;
                        EndCord.y = UnityEngine.Random.Range((int)startCord.y + 1, (int)size.y - 1);
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
                            startIndex = UnityEngine.Random.Range(secoundRowStartingTileIndex, map.Length);
                        } while (reserved.Contains(startIndex));
                        startCord.x = (startIndex % (int)size.x);
                        startCord.y = startIndex / (int)size.x;
                        EndCord.x = startCord.x;
                        EndCord.y = UnityEngine.Random.Range(0, (int)startCord.y);
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

    private void InitMap(Vector2 size, List<int> reserved)
    {
        map = new Tile[(int)size.x * (int)size.y];
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new Tile(i, size);
        }

        map[0] = new Start(0, size);
        reserved.Add(0);//reserve start point 

        if ((size.y % 2) != 0)
        {
            map[map.Length - 1] = new End(map.Length - 1, size);
            reserved.Add(map.Length - 1);//reserve end point
        }
        else
        {
            int lastRowStartingTileIndex = (int)(size.x * (size.y - 1));
            map[lastRowStartingTileIndex] = new End(lastRowStartingTileIndex, size);
            reserved.Add(lastRowStartingTileIndex);//reserve end point
        }
    }
}

[System.Serializable]
class Tile
{
    public int Num { get => num; }
    [SerializeField] private int num;
    public Vector2 Cord { get => cord; }
    [SerializeField] private Vector2 cord;
    public Directions Direction { get => direction; }

    public int Next { get => next; }
    [SerializeField] private int next;
    public string DirectionStr { get { return GetDirectionLogIcon(); } }

    [SerializeField] private Directions direction;

    public string logIcon;
    public Tile(int num, Vector2 mapSize)
    {
        this.num = num;

        var x = (num % (int)mapSize.x);
        var y = num / (int)mapSize.x;
        cord = new Vector2(x, y);

        direction = ((y % 2) == 0) ? Directions.right : Directions.left;
        if ((x == 0 && (y % 2) != 0) || (x == (int)mapSize.x - 1 && (y % 2) == 0)) direction = Directions.up;

        CalculateNext(mapSize);

        logIcon = "[_]";
    }

    private void CalculateNext(Vector2 mapSize)
    {
        switch (direction)
        {
            case Directions.right:
                next = num + 1;
                break;
            case Directions.left:
                next = num - 1;
                break;
            case Directions.up:
                //if (cord.y % 2 != 0)
                    next = num + (int)mapSize.x;
                //else
                //    next = num + 1;
                break;
            case Directions.down:
                if (cord.y % 2 == 0)
                    next = num - (int)mapSize.x;
                else
                    next = num - 1;
                next = num;
                break;
            default:
                break;
        }
    }

    public new virtual string ToString()
    {
        return logIcon;
    }
    private string GetDirectionLogIcon()
    {
        switch (direction)
        {
            case Directions.right:
                return "[>]";
            case Directions.left:
                return "[<]";
            case Directions.up:
                return "[V]";
            case Directions.down:
                return "[^]";
            default:
                return logIcon;
        }
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
enum Directions
{
    right, left, up, down
}