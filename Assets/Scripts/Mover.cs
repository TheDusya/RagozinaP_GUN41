using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    internal class Mover : MonoBehaviour
    {
        [SerializeField, Tooltip("Local")]
        private Vector3 _start;
        [SerializeField, Tooltip("Local")]
        private Vector3 _end;
        //проблема - сломается при передвижении player:(
        [SerializeField, Min(0.1f)]
        private float _speed = 5;
        [SerializeField, Min(0f)]
        private float _delay = 0;
        private Vector3 globalStart;
        private Vector3 globalEnd;
        private void RecalculateEnds()
        {
            globalStart = _start + transform.position;
            globalEnd = _end + transform.position;
        }
        private IEnumerator Start()
        {
            RecalculateEnds();
            transform.position = globalStart;
            bool isForward = true;
            if (_speed == 0)
                while (true)
                    yield return null;
            while (true) {
                yield return StartCoroutine(OneWayMovement(isForward));
                yield return new WaitForSeconds(_delay);
                isForward = !isForward;
            }
        }
        private IEnumerator OneWayMovement(bool isForward)
        {
            var start = transform.position;
            var end = isForward ? globalEnd : globalStart;
            var duration = Vector3.Distance(start, end) / _speed;
            var elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                var res = Vector3.MoveTowards(transform.position, end, _speed * Time.deltaTime);
                transform.position = res;
                yield return null;
            }
            transform.position = end;
        }
    }
}
