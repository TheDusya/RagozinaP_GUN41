using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour
    {
        LineRenderer _lineRenderer;
        public Vector3[] Points { get; private set; }

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var positionCount = _lineRenderer.positionCount;
            Points = new Vector3[positionCount];
            _lineRenderer.GetPositions(Points);
            //нам нужны позиции без сдвига
            Points = Points.Select(point => new Vector3(point.x + transform.position.x, point.y, point.z + transform.position.z)).ToArray();
        }
    }
}
