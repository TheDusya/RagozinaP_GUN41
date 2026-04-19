using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotate = new();
    [SerializeField]
    private float _speed = 10f;
    private IEnumerator Start()
    {
        //не очень поняла формулировку "внутри метода Start находится физическое тело объекта"
        if (!TryGetComponent<Rigidbody>(out var rigidbody) || rigidbody == null)
        {
            Debug.LogError("No rigidbody to move found!");
        }
        while (true)
        {
            Quaternion deltaRotation = Quaternion.Euler(_speed * Time.deltaTime * _rotate);
            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
            yield return null;
        }
    }
}
