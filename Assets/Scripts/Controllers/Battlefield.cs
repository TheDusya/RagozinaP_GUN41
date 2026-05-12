using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class Battlefield : MonoBehaviour
{
    private Cell[] _cells;
    private Unit[] _units;
    public Dictionary<Cell, Dictionary<NeighbourType, Cell>> _neighbours;
    [Inject]
    private ColorPaletteSettings colorPalette;
    [SerializeField]
    private float _movementSpeed = 1f;
    private Vector3 _divider; //more complex divider may be created for more teams
    public static event Action OnMoveBegin;
    public static event Action OnMoveEnd;

    void Awake()
    {
        InitCells();
        InitUnits();
    }

    private void InitCells()
    {
        _cells = FindObjectsOfType<Cell>();
        var positions = Array.ConvertAll(_cells, cell => cell.transform.position);
        _neighbours = new();
        var oneCellDistance = _cells[0].transform.lossyScale.x; //assuming they are squares
        for (int i = 0; i < _cells.Length; i++)
        {
            var currCell = _cells[i];
            //InitUnitOnCell(currCell);
            _neighbours[currCell] = new Dictionary<NeighbourType, Cell>();
            currCell.OnPointerClickEvent += OnCellClicked;
            for (int j = 0; j < _cells.Length; j++)
            {
                if (i == j) continue;
                var source = positions[i];
                var target = positions[j];
                var type = GetNeighbourType(source, target, oneCellDistance);
                if (type is NeighbourType typeNotNull)
                    _neighbours[currCell][typeNotNull] = _cells[j];
            }
        }
    }

    private void InitUnits()
    {
        _units = FindObjectsOfType<Unit>();
        foreach (var unit in _units)
        {
            var cell = GetCell(unit);
            InitUnit(unit);
            InstantMove(unit, cell);
        }
    }

    private void InstantMove(Unit unit, Cell cell)
    {
        var thisPos = transform.position;
        var cellPos = cell.transform.position;
        thisPos.Set(cellPos.x, thisPos.y, cellPos.z);
        SetCell(unit, cell);
    }
    private void Move(Unit unit, Cell newCell) //questionable
    {
        InstantMove(unit, unit.CurrentCell);
        StartCoroutine(DoMovement(unit, newCell));
    }
    private IEnumerator DoMovement(Unit unit, Cell newCell)
    {
        OnMoveBegin.Invoke();
        var start = unit.transform.position;
        var end = newCell.transform.position;
        var duration = Vector3.Distance(start, end) / _movementSpeed;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var res = Vector3.MoveTowards(transform.position, end, _movementSpeed * Time.deltaTime);
            unit.transform.position = res;
            yield return null;
        }
        unit.transform.position = end;
        SetCell(unit, newCell);
        OnMoveEnd.Invoke();
    }
    public void SetCell(Unit unit, Cell cell)
    {
        if (unit.CurrentCell != null)
            unit.CurrentCell.CurrentUnit = null; //sorry for this monstrosity
        unit.CurrentCell = cell;
        cell.CurrentUnit = unit;
    }

    public NeighbourType? GetNeighbourType(Vector3 source, Vector3 target, float oneCellDistance)
    {
        var xDist = source.x - target.x;
        var zDist = source.z - target.z;
        if (Math.Abs(xDist) > oneCellDistance || Math.Abs(zDist) > oneCellDistance)
        return null;
        NeighbourType? type = (Math.Sign(zDist), Math.Sign(xDist)) switch
        {
            (1, 1) => NeighbourType.TopRight,
            (1, 0) => NeighbourType.Top,
            (1, -1) => NeighbourType.TopLeft,
            (0, 1) => NeighbourType.Right,
            (0, -1) => NeighbourType.Left,
            (-1, 1) => NeighbourType.BottomRight,
            (-1, 0) => NeighbourType.Bottom,
            (-1, -1) => NeighbourType.BottomLeft,
            _ => null
        };
        return type;
    }

    public void SetDivider (GameObject divider) => _divider = divider.transform.position;

    public void OnDestroy()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _neighbours[_cells[i]].Clear();
            _cells[i].OnPointerClickEvent -= OnCellClicked;
        }
        _neighbours.Clear();
    }

    public Cell GetCell(Unit unit) =>  _cells.OrderBy(el => (el.transform.position - unit.transform.position).sqrMagnitude).FirstOrDefault();
    public void OnCellClicked(Cell cell) => cell.SetSelect(colorPalette.SelectCell);
    private void InitUnit(Unit unit)
    {
        unit.IsQueen = false;
        var team = GetTeam(unit.transform.position);
        unit.Team = team;
        unit.SetMaterials(colorPalette.GetTeamMaterial(team), colorPalette.GetTeamTransparenMaterial(team));
    }

    private Team GetTeam(Vector3 position) => position.z < _divider.z ? Team.Player1 : Team.Player2;
}
