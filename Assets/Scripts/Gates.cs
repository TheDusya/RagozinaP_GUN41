using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Gates : MonoBehaviour
{
    public int Score;
    private void IncreaseScore() => Score++;
    private void WriteScore() => Debug.Log($"Your score is {Score}!");

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ball>(out _)) 
        {
            Destroy(other);
            IncreaseScore();
            WriteScore();
        }
    }
}
