using UnityEngine;

namespace Map
{
    [System.Serializable]
    public struct Position
    {
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position(Vector2 cord)
        {
            this.x = (int)cord.x;
            this.y = (int)cord.y;
        }
        //public Cord(Vector3 cord)
        //{
        //    this.x = (int)cord.x;
        //    this.y = (int)cord.y;
        //}
        public int x;
        public int y;

        public new string ToString()
        {
            return $"({x:D3},{y:D3})";
        }
    }

}