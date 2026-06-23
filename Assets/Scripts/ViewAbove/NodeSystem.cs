
using Assets.Scripts.ViewAbove;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeSystem : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap _tilemap;
    private List<TileNode> _walkableNodes;

    void Start()
    {
        _walkableNodes = new();
        FillInNodes(_tilemap.cellBounds);
    }

    private void FillInNodes(BoundsInt area)
    {
        for (int x = area.xMin + 1; x <= area.xMax; x++)
            for (int y = area.yMin + 1; y <= area.yMax; y++)
                if (IsNodeWalkable(x, y))
                    _walkableNodes.Add(new TileNode(new Vector3Int(x, y)));
    }

    public TileNode GetNeighbour(TileNode node, MoveDirection direction)
    {
        Vector3Int position = direction switch
        {
            MoveDirection.Left => new Vector3Int(node.X - 1, node.Y, node.Z),
            MoveDirection.Right => new Vector3Int(node.X + 1, node.Y, node.Z),
            MoveDirection.Up => new Vector3Int(node.X, node.Y + 1, node.Z),
            MoveDirection.Down => new Vector3Int(node.X, node.Y - 1, node.Z),
            _ => throw new System.Exception("Wrong direction!")
        };
        TileNode newNode = new(position);
        return HasNode(newNode) ? newNode : null;
    }

    public bool HasNode(TileNode node)
    {
        for (int i = -1; i <= 0; i++)
            for (int j = -1; j <= 0; j++)
                if (!_tilemap.HasTile(new Vector3Int(node.X + i, node.Y + j)))
                    return false;
        return true;
    }

    private bool IsNodeWalkable(int cornerX, int cornerY)
    {
        for (int i = -1; i <= 0; i++)
            for (int j = -1; j <= 0; j++)
                if (!IsTileWalkable(new Vector3Int(cornerX + i, cornerY + j)))
                    return false;
        return true;
    }
    public bool IsNodeWalkable(TileNode node) => IsNodeWalkable(node.X, node.Y); 

    private bool IsTileWalkable(Vector3Int position)
    {
        if (!_tilemap.HasTile(position))
            return false;
        var colliderType = _tilemap.GetColliderType(position);
        return colliderType == Tile.ColliderType.None;
    }

    public Vector3 GetWorldPosition(TileNode node) => new Vector3(node.X, node.Y, 0);
}
