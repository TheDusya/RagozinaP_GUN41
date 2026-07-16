using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    internal class RobotScript : MonoBehaviour
    {
        private const float _rotationLimit = 0.01f;

        [SerializeField]
        private float _speed = 2;
        [SerializeField]
        private float _rotationSpeed = 2;
        [SerializeField]
        private float _maxObstacleDistance = 1;
        private bool IsRotating => _rotationGoal != Vector3.zero;
        private System.Random _random = new();
        private Vector3 _rotationGoal = Vector3.zero;

        private Vector3 VectorFromDirection(Direction direction) => 
            direction switch {
                Direction.Forward => transform.forward,
                Direction.Left => (Vector3.left + transform.forward).normalized,
                Direction.Right => (Vector3.right + transform.forward).normalized,
                _ => throw new NotImplementedException()
            };

        private bool IsObstacleInDirection(Direction direction) => 
            Physics.Raycast(transform.position, VectorFromDirection(direction), _maxObstacleDistance);

        private bool TryGetPossibleDirections(out List<Direction> directions)
        {
            directions = new List<Direction>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                if (!IsObstacleInDirection(direction))
                    directions.Add(direction);
            return directions != null;
        }

        private void Update()
        {
            if (!IsRotating)
                if (TryGetPossibleDirections(out var directions))
                    if (directions.Contains(Direction.Forward))
                        MoveForward();
                    else if (directions.Count == 1)
                        _rotationGoal = VectorFromDirection(directions[0]);
                    else
                        _rotationGoal = VectorFromDirection(directions[_random.Next(directions.Count)]);
                else
                    MoveBackward();
            if (IsRotating)
                    Rotate();
        } 


        private void MoveForward() => transform.position += _speed * Time.deltaTime * transform.forward;
        private void MoveBackward() => transform.position -= _speed * Time.deltaTime * transform.forward;

        private void Rotate()
        {
            transform.Rotate(transform.up, _rotationSpeed * Time.deltaTime * _rotationGoal.y);
            if (Vector3.Angle(transform.forward, _rotationGoal) < _rotationLimit)
              _rotationGoal = Vector3.zero;
        }
    }
}
