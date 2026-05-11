using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    Cell currentCell;
    [SerializeField]
    private float _speed = 1f;
    [Inject(Id = "CellManager")]
    CellManager _cellManager;
    public static event Action OnMoveEndCallback;

    public void OnPointerEnter(PointerEventData eventData) => currentCell.OnPointerEnter(eventData);
    public void OnPointerExit(PointerEventData eventData) => currentCell.OnPointerExit(eventData);
    public void OnPointerClick(PointerEventData eventData) => currentCell.OnPointerClick(eventData);
    private void Start()
    {
        SetCell(_cellManager.GetCell(this));
        InstantMove(currentCell);
    }
    private void InstantMove(Cell cell)
    {
        var thisPos = transform.position;
        var cellPos = cell.transform.position;
        thisPos.Set(cellPos.x, thisPos.y, cellPos.z);
    }
    private void Move(Cell newCell) //questionable
    {
        InstantMove(currentCell);
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
        SetCell(newCell);
        OnMoveEndCallback.Invoke();
    }
    private void SetCell(Cell cell)
    {
        if (currentCell != null)
            currentCell.Unit = null;
        currentCell = cell;
        currentCell.Unit = this;
    }
}
