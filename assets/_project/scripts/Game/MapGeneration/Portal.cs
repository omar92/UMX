using UnityEngine;


namespace Map
{
    [System.Serializable]
    class Portal : Tile
    {
        public Position Target { get => target; }
        [SerializeField] private Position target;
        public Portal(Position cord, Position target, Position mapSize) : base(cord, mapSize)
        {
            this.target = target;
            type = target.y > cord.y ? TileType.ShortCut : TileType.Pit;
        }

        public override string ToString()
        {
            if (target.y > Cord.y)
                return "<color=blue>[V]</color>";
            else
                return "<color=red>[^]</color>";
        }
    }
}