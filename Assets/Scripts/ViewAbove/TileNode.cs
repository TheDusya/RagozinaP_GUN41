using UnityEngine;

namespace Assets.Scripts.ViewAbove
{
    public class TileNode
    {
        public Vector3Int Position;
        public int X => Position.x; 
        public int Y => Position.y;
        public int Z => Position.z;

        public TileNode(Vector3Int cornerPosition)
        {
            Position = cornerPosition;
        }
        public override bool Equals(object obj)
        {
            return obj is TileNode node && X == node.X && Y == node.Y;
        }
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
