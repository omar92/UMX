using UnityEngine;


namespace Map
{
    [System.Serializable]
    public class Tile
    {
        public int Num { get => num; }
        [SerializeField] private int num;
        public Vector2 Cord { get => cord; }
        [SerializeField] private Vector2 cord;
        public Directions Direction { get => direction; }
        [SerializeField] private Directions direction;

        public TileType Type { get => type; }
        [SerializeField] protected TileType type;

        public int Next { get => next; }
        [SerializeField] private int next;
        public string DirectionStr { get { return GetDirectionLogIcon(); } }



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
            type = TileType.Tile;
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
}