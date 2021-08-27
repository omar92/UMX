using UnityEngine;


namespace Map
{
    [System.Serializable]
    class End : Tile
    {
        public End(int num, Vector2 mapSize) : base(num, mapSize)
        {
            logIcon = "[E]";
        }
    }
}