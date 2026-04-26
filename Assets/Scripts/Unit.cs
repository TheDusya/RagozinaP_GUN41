using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    Cell currentCell;
    [SerializeField]
    private float _speed = 1f;
    public static event Action OnMoveEndCallback;

    public void OnPointerEnter(PointerEventData eventData) => currentCell.OnPointerEnter(eventData);
    public void OnPointerExit(PointerEventData eventData) => currentCell.OnPointerEnter(eventData);
    public void OnPointerClick(PointerEventData eventData) => currentCell.OnPointerEnter(eventData);
    private void Move(Cell newCell)
    {
        StartCoroutine(DoMovement(newCell));
    }
    private IEnumerator DoMovement(Cell newCell)
    {
        var start = transform.position;
        var end = newCell.transform.position;
        var duration = Vector3.Distance(start, end) / _speed;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var res = Vector3.MoveTowards(transform.position, end, _speed * Time.deltaTime);
            transform.position = res;
            yield return null;
        }
        transform.position = end;
        currentCell = newCell;
        OnMoveEndCallback.Invoke();
    }
}
