using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Map
{
    [System.Serializable]
    public class GameMap
    {
        public Tile[][] tiles;

        public Position Size { get => size; }
        private Position size;

        private Position minSize = new Position(2, 2);



        private GameMap() { }
        public GameMap(Vector2 size, int shortcutsNum, int PitFallsNum)
        {
            if (size.x < minSize.x || size.y < minSize.y)
            {
                Debug.LogError($"map size must be > {minSize.ToString()}");
            }
            else
            {
                this.size = new Position(size);
                GenerateMap(shortcutsNum, PitFallsNum);
            }
        }
        public Tile GetTile(Position pos)
        {
            return tiles[pos.y][pos.x];
        }

        internal bool PositionIsWithenBoundries(Position pos)
        {
            return ((pos.x < Size.x) && ((pos.y < Size.y)));
        }

        private void GenerateMap(int shortcutsNum, int pitFallsNum)
        {

            List<Position> reserved = new List<Position>();
            InitMap(reserved);
            GeneratePortals(shortcutsNum, pitFallsNum, reserved);
        }

        private void InitMap(List<Position> reserved)
        {
            tiles = new Tile[size.y][];

            for (int y = 0; y < size.y; y++)
            {
                tiles[y] = new Tile[size.x];
                for (int x = 0; x < size.x; x++)
                {
                    tiles[y][x] = new Tile(new Position(x, y), size);
                }
            }
            //set start Point
            tiles[0][0] = new Start(new Position(0, 0), size);
            reserved.Add(new Position(0, 0));//reserve start point 

            Position endCord;
            if ((size.y % 2) != 0)
            {
                endCord = new Position(size.x - 1, size.y - 1);
            }
            else
            {
                endCord = new Position(0, size.y - 1);
            }
            tiles[endCord.y][endCord.x] = new End(endCord, size);
            reserved.Add(endCord);//reserve end point
        }
        private void GeneratePortals(int shortcutsNum, int pitFallsNum, List<Position> reserved)
        {
            if ((shortcutsNum * 2 + pitFallsNum * 2) > (size.x * size.y - 2))
            {
                Debug.LogError("shortcuts and portals number must be less than the map tiles");
            }
            else
            {
                for (int i = 0; i < shortcutsNum; i++)
                {
                    GeneratePortal("<", reserved, out Position start, out Position end);
                    reserved.Add(start);
                    reserved.Add(end);
                    tiles[start.y][start.x] = new Portal(start, end, size);
                    tiles[end.y][end.x] = new PortalOut(end, start, size);
                }
                for (int i = 0; i < pitFallsNum; i++)
                {
                    GeneratePortal(">", reserved, out Position start, out Position end);
                    reserved.Add(start);
                    reserved.Add(end);
                    tiles[start.y][start.x] = new Portal(start, end, size);
                    tiles[end.y][end.x] = new PortalOut(end, start, size);
                }
            }
        }

        internal Position Next(Position start, int num)
        {
            var currentTile = GetTile(start);
            for (int i = 0; (i < num); i++)
            {
                if (PositionIsWithenBoundries(currentTile.Next))
                {
                    currentTile = GetTile(currentTile.Next);
                }
                else
                {
                    break;
                }
            }
            return currentTile.Cord;
        }

        private void GeneratePortal(string rule, List<Position> reserved, out Position startCord, out Position EndCord)
        {
            switch (rule)
            {
                case "<":
                    {
                        do
                        {
                            do
                            {
                                startCord.x = UnityEngine.Random.Range(0, size.x - 1);
                                startCord.y = UnityEngine.Random.Range(0, size.y - 2);//dont select from last row so it can be used as out later

                            } while (reserved.Contains(startCord));

                            EndCord.x = startCord.x;
                            EndCord.y = UnityEngine.Random.Range(startCord.y + 1, size.y - 1);
                        } while (reserved.Contains(EndCord));
                    }
                    break;
                case ">":
                    {
                        do
                        {
                            do
                            {
                                startCord.x = UnityEngine.Random.Range(0, size.x - 1);
                                startCord.y = UnityEngine.Random.Range(1, size.y - 1);//dont select from first row so it can be used as out later
                            } while (reserved.Contains(startCord));
                            EndCord.x = startCord.x;
                            EndCord.y = UnityEngine.Random.Range(0, startCord.y);
                        } while (reserved.Contains(EndCord));
                    }
                    break;
                default:
                    Debug.LogError("GeneratePortal must user rule '<' or '>'");
                    startCord = new Position(-1, -1);
                    EndCord = new Position(-1, -1);
                    break;
            }
        }


        public new string ToString()
        {
            string mapText = "Portals\n\n";

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    mapText += tiles[y][x].ToString();
                }
                mapText += "\n";
            }

            mapText += "\n\n";

            mapText += "Directions\n\n";

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    mapText += tiles[y][x].ToString(true);
                }
                mapText += "\n";
            }

            return mapText;
        }
    }

}