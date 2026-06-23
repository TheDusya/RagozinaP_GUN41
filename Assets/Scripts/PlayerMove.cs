using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    abstract class PlayerMove : MonoBehaviour
    {
        [SerializeField]
        protected Tilemap _tilemap;
        [SerializeField]
        protected float _speed = 5;
        protected bool _isMoving;

        protected void Move(Vector3 targetPosition)
        {
            transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _speed * Time.deltaTime
                );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                SetNewCurrent();
                _isMoving = false;
            }
        }

        protected abstract void SetNewCurrent();
    }
}
