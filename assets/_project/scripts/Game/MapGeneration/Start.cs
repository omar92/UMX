using UnityEngine;


namespace Map
{
    [System.Serializable]
    class Start : Tile
    {
        public Start(int num, Vector2 mapSize) : base(num, mapSize)
        {
            logIcon = "[S]";
        }
    }
}