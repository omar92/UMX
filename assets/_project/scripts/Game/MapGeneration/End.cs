using UnityEngine;


namespace Map
{
    [System.Serializable]
    class End : Tile
    {
        public End(Position cord, Position mapSize) : base(cord, mapSize)
        {
            type = TileType.End;
        }
        public override string ToString()
        {
            return "[E]";
        }
    }
}