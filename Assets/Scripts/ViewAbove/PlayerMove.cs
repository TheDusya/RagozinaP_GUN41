using Assets.Scripts.ViewAbove;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Tilemap _map;
    [SerializeField]
    private float _speed = 5;

    private NodeSystem _nodeSystem;
    private MoveDirection _moveDirection;
    private bool IsMoving => _nextNode != null && _moveDirection != MoveDirection.NoMovement;
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
        if (!IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _moveDirection = MoveDirection.Left;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                _moveDirection = MoveDirection.Right;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                _moveDirection = MoveDirection.Up;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
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
            Move();
    }
    private void Move()
    {
        Vector3 targetPosition = _nextNode.Position;
        transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime
            );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            _currNode = _nextNode;
            _nextNode = null;
            _moveDirection = MoveDirection.NoMovement;
        }
    }
}
