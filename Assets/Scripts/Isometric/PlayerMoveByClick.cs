using System;
using UnityEngine;

namespace Assets.Scripts.ViewAbove
{
    internal class PlayerMoveByClick : PlayerMove
    {
        [SerializeField]
        Camera _camera;
        float _cameraZ;
        Vector3 _targetPos;
        private void Start()
        {
            var myPosition = transform.position;
            _cameraZ = _camera.transform.position.z;
            Vector3Int mapMyPosition = _tilemap.WorldToCell(myPosition);
            if (!_tilemap.HasTile(mapMyPosition))
                throw new Exception("Tile does not exist");
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = -_cameraZ;
                Vector3 clickPosition = _camera.ScreenToWorldPoint(mousePos);
                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
                Vector3Int mapClickPosition = _tilemap.WorldToCell(hit.point);
                if (_tilemap.HasTile(mapClickPosition))
                {
                    _targetPos =  _tilemap.CellToWorld(mapClickPosition);
                    _isMoving = true;
                    _targetPos.y += _tilemap.cellSize.y;
                    _targetPos.z = transform.position.z;
                }
            }
            if (_isMoving)
                Move(_targetPos);
        }
        protected override void SetNewCurrent() { }
    }
}
