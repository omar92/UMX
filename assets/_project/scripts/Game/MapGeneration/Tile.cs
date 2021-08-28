using System;
using UnityEngine;


namespace Map
{
    [System.Serializable]
    public class Tile
    {
        public Position Cord { get => cord; }
        [SerializeField] private Position cord;

        public Position Next { get => next; }
        [SerializeField] private Position next;

        public TileType Type { get => type; }
        [SerializeField] protected TileType type;

        public Tile(Position cord, Position mapSize)
        {
            this.cord = cord;
            CalculateNext(mapSize);
            type = TileType.Tile;
        }
        private Directions nextDirection;
        private void CalculateNext(Position mapSize)
        {
            nextDirection = ((cord.y % 2) == 0) ? Directions.right : Directions.left;
            if ((cord.x == 0 && (cord.y % 2) != 0) || (cord.x == mapSize.x - 1 && (cord.y % 2) == 0)) nextDirection = Directions.up;

            next = GetNeighpor(nextDirection);
        }

        public Position GetNeighpor(Directions direction)
        {
            Position neighporPos;
            switch (direction)
            {
                case Directions.right:
                    neighporPos = new Position(cord.x + 1, cord.y);
                    break;
                case Directions.left:
                    neighporPos = new Position(cord.x - 1, cord.y);
                    break;
                case Directions.up:
                    neighporPos = new Position(cord.x, cord.y + 1);
                    break;
                case Directions.down:
                    neighporPos = new Position(cord.x, cord.y - 1);
                    break;
                default:
                    throw new Exception("Unknown Direction");
            }
            return neighporPos;
        }

        public new virtual string ToString()
        {
            return "[_]";
        }
        public virtual string ToString(bool showDirection)
        {
            return GetDirectionLogIcon();
        }
        private string GetDirectionLogIcon()
        {
            switch (nextDirection)
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
                    return "[_]";
            }
        }
        //public int Num { get => num; }
        //[SerializeField] private int num;

        //public Directions Direction { get => direction; }
        //[SerializeField] private Directions direction;



        //public int Next { get => next; }
        //[SerializeField] private int next;
        //public string DirectionStr { get { return GetDirectionLogIcon(); } }



        //public string logIcon;
        //public Tile(int num, Vector2 mapSize)
        //{
        //    this.num = num;

        //    var x = (num % (int)mapSize.x);
        //    var y = num / (int)mapSize.x;
        //    cord = new Vector2(x, y);

        //    direction = ((y % 2) == 0) ? Directions.right : Directions.left;
        //    if ((x == 0 && (y % 2) != 0) || (x == (int)mapSize.x - 1 && (y % 2) == 0)) direction = Directions.up;

        //    CalculateNext(mapSize);

        //    logIcon = "[_]";
        //    type = TileType.Tile;
        //}



        //public new virtual string ToString()
        //{
        //    return logIcon;
        //}

    }
}