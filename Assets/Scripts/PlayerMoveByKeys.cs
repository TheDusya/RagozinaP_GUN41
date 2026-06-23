using Assets.Scripts;
using Assets.Scripts.ViewAbove;
using UnityEngine;

class PlayerMoveByKeys : PlayerMove
{
    private NodeSystem _nodeSystem;
    private MoveDirection _moveDirection;
    private TileNode _currNode;
    private TileNode _nextNode = null;

    void Start()
    {
        var myRenderer = GetComponent<Renderer>();
        _nodeSystem = FindAnyObjectByType<NodeSystem>();
        Vector3Int myPosition = Vector3Int.FloorToInt(myRenderer.bounds.max);
        var node = new TileNode(myPosition);
        if (!_nodeSystem.HasNode(node))
            throw new System.Exception("Wrong position!");
        _currNode = node;
        _moveDirection = MoveDirection.NoMovement;
    }
    void Update()
    {
        if (!_isMoving && _nextNode == null)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                _moveDirection = MoveDirection.Left;
            else if (Input.GetKey(KeyCode.RightArrow))
                _moveDirection = MoveDirection.Right;
            else if (Input.GetKey(KeyCode.UpArrow))
                _moveDirection = MoveDirection.Up;
            else if (Input.GetKey(KeyCode.DownArrow))
                _moveDirection = MoveDirection.Down;
            else
                return;
        }
        if (_nextNode == null)
        {
            var neighbour = _nodeSystem.GetNeighbour(_currNode, _moveDirection);
            if (_nodeSystem.HasNode(neighbour) && _nodeSystem.IsNodeWalkable(neighbour))
                _nextNode = neighbour;
        }
        else
            Move(_nextNode.Position);
    }
    protected override void SetNewCurrent()
    {
        _currNode = _nextNode;
        _nextNode = null;
    }
}
