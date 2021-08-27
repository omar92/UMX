using UnityEngine;


namespace Map
{
    [System.Serializable]
    class Portal : Tile
    {
        public int Target { get => target; }
        [SerializeField] private int target;
        public Portal(int num, int target, Vector2 mapSize) : base(num, mapSize)
        {
            this.target = target;
            logIcon = "<color=blue>[O]</color>";
        }
    }
}