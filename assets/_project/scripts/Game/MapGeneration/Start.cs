using UnityEngine;


namespace Map
{
    [System.Serializable]
    class Start : Tile
    {
        public Start(Position cord, Position mapSize) : base(cord, mapSize)
        {
            type = TileType.Start;
        }

        public override string ToString()
        {
            return "[S]";
        }
    }
}