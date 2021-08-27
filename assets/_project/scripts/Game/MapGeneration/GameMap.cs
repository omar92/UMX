using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Map
{
    [System.Serializable]
    public class GameMap
    {
        public Vector2 Size { get => size; }
        Vector2 size;
        public Tile[] tiles;
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
            for (int i = 0; i < tiles.Length; i++)
            {
                if (row != tiles[i].Cord.y)
                {
                    mapText += "\n";
                    row = (int)tiles[i].Cord.y;
                }
                mapText += tiles[i].ToString();
            }

            mapText += "\n\n";

            mapText += "Directions\n\n";
            row = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (row != tiles[i].Cord.y)
                {
                    mapText += "\n";
                    row = (int)tiles[i].Cord.y;
                }
                mapText += tiles[i].DirectionStr;
            }

            mapText += "\n\n";

            mapText += "Compined\n\n";
            row = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (row != tiles[i].Cord.y)
                {
                    mapText += "\n";
                    row = (int)tiles[i].Cord.y;
                }

                if (tiles[i].ToString() != "[_]")
                    mapText += tiles[i].ToString();
                else
                    mapText += tiles[i].DirectionStr;
            }

            mapText += "\n\n";

            mapText += "next\n\n";
            row = 0;
            for (int i = 0; i < tiles.Length; i++)
            {
                if (row != tiles[i].Cord.y)
                {
                    mapText += "\n";
                    row = (int)tiles[i].Cord.y;
                }

                mapText += $" [{tiles[i].Num:D3}:<color=green>{tiles[i].Next:D3}</color>] ";

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
                    tiles[start] = new Portal(start, end, size);
                    tiles[start].logIcon = "<color=blue>[V]</color>";
                    tiles[end].logIcon = "<color=blue>[O]</color>";
                }
                for (int i = 0; i < pitFallsNum; i++)
                {
                    GeneratePortal(">", reserved, out int start, out int end);
                    reserved.Add(start);
                    reserved.Add(end);
                    tiles[start] = new Portal(start, end, size);
                    tiles[start].logIcon = "<color=red>[^]</color>";
                    tiles[end].logIcon = "<color=red>[O]</color>";
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
                                startIndex = UnityEngine.Random.Range(secoundRowStartingTileIndex, tiles.Length);
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
            tiles = new Tile[(int)size.x * (int)size.y];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(i, size);
            }

            tiles[0] = new Start(0, size);
            reserved.Add(0);//reserve start point 

            if ((size.y % 2) != 0)
            {
                tiles[tiles.Length - 1] = new End(tiles.Length - 1, size);
                reserved.Add(tiles.Length - 1);//reserve end point
            }
            else
            {
                int lastRowStartingTileIndex = (int)(size.x * (size.y - 1));
                tiles[lastRowStartingTileIndex] = new End(lastRowStartingTileIndex, size);
                reserved.Add(lastRowStartingTileIndex);//reserve end point
            }
        }
    }
}