using UnityEngine;


namespace Map
{
    [System.Serializable]
    class PortalOut : Tile
    {
        public Position Source { get => source; }
        [SerializeField] private Position source;
        public PortalOut(Position cord, Position source, Position mapSize) : base(cord, mapSize)
        {
            this.source = source;
            type = TileType.Tile;
        }

        public override string ToString()
        {
            if (Cord.y > source.y)
                return "<color=blue>[O]</color>";
            else
                return "<color=red>[O]</color>";
        }
    }
}