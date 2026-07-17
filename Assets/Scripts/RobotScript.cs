using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    internal class RobotScript : MonoBehaviour
    {
        private const float _rotationLimit = 0.01f;
        private const int _antiStuckCounterMax = 5;

        [SerializeField]
        private float _speed = 2;
        [SerializeField]
        private float _rotationSpeed = 2;
        [SerializeField]
        private float _maxObstacleDistance = 1;
        private bool IsRotating => _rotationGoal != 0;
        private System.Random _random = new();
        private float _rotationGoal = 0;
        private float _rotationSum = 0;
        private int _antiStuckCounter = 0;
        private bool _isPrevBackward = false;
        private CapsuleCollider _collider;

        private void OnEnable()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        private float AngleFromDirection(Direction direction) => 
            direction switch {
                Direction.Forward => 0,
                Direction.Left => -90,
                Direction.Right => 90,
                _ => throw new NotImplementedException()
            };

        private Vector3 VectorFromDirection(Direction direction) => 
            direction switch {
                Direction.Forward => transform.forward,
                Direction.Left => transform.forward + Vector3.left,
                Direction.Right => transform.forward + Vector3.right,
                _ => throw new NotImplementedException()
            };

        private bool IsObstacleInDirection(Direction direction) =>  
            Physics.SphereCast(new Ray(transform.position, VectorFromDirection(direction)), _collider.radius, _maxObstacleDistance);

        private bool TryGetPossibleDirections(out List<Direction> directions)
        {
            directions = new List<Direction>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                if (!IsObstacleInDirection(direction))
                    directions.Add(direction);
            return directions.Any();
        }

        private void Update()
        {
            if (!IsRotating)
                if (TryGetPossibleDirections(out var directions))
                {
                    bool isWayForward = directions.Contains(Direction.Forward);
                    bool isOnlyWayForward = isWayForward && directions.Count == 1;
                    if (_antiStuckCounter > _antiStuckCounterMax) //stop moving forward/backward
                        if (isOnlyWayForward)
                            ReportRobotIsStuck();
                        else
                        {
                            isWayForward = false;
                            directions.Remove(Direction.Forward);
                            _antiStuckCounter = 0;
                        }
                    if (isWayForward)
                        MoveForward();
                    else if (directions.Count == 1)
                        _rotationGoal = AngleFromDirection(directions[0]);
                    else
                        _rotationGoal = AngleFromDirection(directions[_random.Next(directions.Count)]);
                    _isPrevBackward = false;
                }
                else
                    MoveBackward();
            if (IsRotating)
                    Rotate();
        }

        private void MoveForward()
        {
            if (_isPrevBackward)
                _antiStuckCounter++;
            else 
                _antiStuckCounter = 0;
            transform.position += _speed * Time.deltaTime * transform.forward;
            _isPrevBackward = false;
        }
        private void MoveBackward()
        {
            transform.position -= _speed * Time.deltaTime * transform.forward;
            _isPrevBackward = true;
        }

        private void Rotate()
        {
            float degreesToPass = _rotationSpeed * Time.deltaTime * _rotationGoal;
            transform.Rotate(0, degreesToPass, 0);
            if (Math.Abs(_rotationSum) > Math.Abs(_rotationGoal) || 
                Math.Abs(_rotationSum - _rotationGoal) < _rotationLimit)
            {
                _rotationGoal = 0;
                _rotationSum = 0;
            }
            else
                _rotationSum += degreesToPass;
            _isPrevBackward = false;
        }

        private void ReportRobotIsStuck() => throw new Exception("AAAAAAAAAAAAAAA ROBOT IS STUCK!!!!!!!!");
    }
}
