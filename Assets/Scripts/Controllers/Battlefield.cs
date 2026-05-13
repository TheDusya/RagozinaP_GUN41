using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using static UnityEngine.UI.CanvasScaler;

public class Battlefield : MonoBehaviour
{
    private Cell[] _cells;
    private Unit[] _units;
    private Dictionary<Cell, Dictionary<NeighbourType, Cell>> _neighbours;
    private Dictionary<Unit, Dictionary<Cell, Unit>> _assessibleCells; //unit is the attacked checker, unit == null -> move without an attack
    private Vector3 _divider; //more complex divider may be created for more teams

    [Inject]
    private ColorPaletteSettings colorPalette;
    [Inject]
    SharedDataManager _dataManager;

    #region initialisation

    void Awake()
    {
        InitCells();
        InitUnits();
        _dataManager.OnGameEvent += ProcessEvent;
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
            foreach (var team in Enum.GetValues(typeof(Team)))
                if (!_neighbours[currCell].Where(el => AcessibleNeighbourTypes((Team)team).Contains(el.Key)).Any()) //the end cell for this team
                    currCell.IsFinalFor = (Team)team;
        }
    }

    private void InitUnits()
    {
        _assessibleCells = new();
        _units = FindObjectsOfType<Unit>();
        foreach (var unit in _units)
        {
            var cell = GetCell(unit);
            InitUnit(unit);
            InstantMove(unit, cell);
        }
        RecountAccess();
    }

    public void SetDivider(GameObject divider) => _divider = divider.transform.position;
    public Cell GetCell(Unit unit) => _cells.OrderBy(el => (el.transform.position - unit.transform.position).sqrMagnitude).FirstOrDefault();
    private void InitUnit(Unit unit)
    {
        unit.IsQueen = false;
        var team = GetTeam(unit.transform.position);
        unit.Team = team;
        unit.SetMaterials(colorPalette.GetTeamMaterial(team), colorPalette.GetTeamTransparenMaterial(team));
    }
    private Team GetTeam(Vector3 position) => position.z < _divider.z ? Team.Player1 : Team.Player2;
    #endregion

    private NeighbourType[] AcessibleNeighbourTypes(Team team) => (team == Team.Player1) ?
            new NeighbourType[2] { NeighbourType.BottomLeft, NeighbourType.BottomRight } :
            new NeighbourType[2] { NeighbourType.TopLeft, NeighbourType.TopRight };
    private Team OtherTeam(Team team) => team == Team.Player1 ? Team.Player2 : Team.Player1;

    private void ProcessEvent(GameEvent gameEvent)
    {
        if (gameEvent == GameEvent.MovementEnd)
        {
            InstantMove(_dataManager.Unit, _dataManager.Cell);
            _dataManager.Cell.ResetSelect();
            RecountAccess();
        }
        if (gameEvent == GameEvent.CancelCell)
            SelectUnit(_dataManager.Unit);
        if (gameEvent == GameEvent.CancelUnit)
            foreach (var cell in _cells)
                cell.ResetSelect();
    }
    public void OnCellClicked(Cell cell)
    {
        if (_dataManager.CurrentState == State.ChoosingUnit && cell.CurrentUnit != null && _dataManager.CurrentPlayer == cell.CurrentUnit.Team)
            SelectUnit(cell.CurrentUnit);
        else if (_dataManager.CurrentState == State.ChoosingCell && _dataManager.Unit != null)
            SelectCell(cell);
    }
    private void SelectUnit(Unit unit)
    {
        var accessible = _assessibleCells[unit];
        _dataManager.SelectUnit(unit);
        foreach (var cell in _cells)
            if (accessible.ContainsKey(cell))
                cell.SetSelect(accessible[cell] ? colorPalette.AttackCell : colorPalette.SelectCell);
            else
                cell.ResetSelect();
    }
    private void SelectCell(Cell cell)
    {
        var acessibleForUnit = _assessibleCells[_dataManager.Unit];
        if (!acessibleForUnit.TryGetValue(cell, out var isAttack))
            return;
        _dataManager.SelectCell(cell, isAttack);
        foreach (var acessibleCell in acessibleForUnit.Keys)
            if (acessibleCell != cell)
                acessibleCell.ResetSelect();
    }

    private void InstantMove(Unit unit, Cell cell)
    {
        var thisPos = transform.position;
        var cellPos = cell.transform.position;
        thisPos.Set(cellPos.x, thisPos.y, cellPos.z);
        SetCell(unit, cell);
    }
    public void SetCell(Unit unit, Cell cell)
    {
        if (unit.CurrentCell != null)
            unit.CurrentCell.CurrentUnit = null;
        unit.CurrentCell = cell;
        cell.CurrentUnit = unit;
        if (cell.IsFinalFor is Team team && team == unit.Team)
            unit.EnterQueenMode();
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
            (1, -1) => NeighbourType.TopLeft,
            (-1, 1) => NeighbourType.BottomRight,
            (-1, -1) => NeighbourType.BottomLeft,
            _ => null
        };
        return type;
    }

    private Dictionary<Cell, Unit> GetAccessibleFrom(Cell cell, Unit unit) //Im sorry for these functions
    {
        if (unit.IsQueen)
            return GetAccessibleFromQueen(cell, unit.Team);
        else
            return GetAccessibleFromSimpleChecker(cell, unit.Team);
    }

    private Dictionary<Cell, Unit> GetAccessibleFromQueen(Cell cell, Team team)
    {
        var result = new Dictionary<Cell, Unit>();
        foreach (var objectNeighbourType in Enum.GetValues(typeof(NeighbourType)))
        {
            var neighbourType = (NeighbourType)objectNeighbourType;
            _neighbours[cell].TryGetValue(neighbourType, out var nextNeighbour);
            while (nextNeighbour != null)
            {
                _neighbours[nextNeighbour].TryGetValue(neighbourType, out var newNeighbour);
                if (nextNeighbour.CurrentUnit == null)
                {
                    result.Add(nextNeighbour, null);
                    nextNeighbour = newNeighbour;
                }
                else if (nextNeighbour.CurrentUnit.Team != team && newNeighbour != null && newNeighbour.CurrentUnit == null) //jumping over the enemy checker
                {
                    result.Add(newNeighbour, nextNeighbour.CurrentUnit);
                    nextNeighbour = null;
                }
                else
                    nextNeighbour = null; //we can't jump over
            }
        }
        var attackResult = (result.Where(val => val.Value != null)).ToDictionary(el => el.Key, el => el.Value);
        if (attackResult.Any()) //we have to attack
            return attackResult;
        else
            return result;
    }

    private Dictionary<Cell, Unit> GetAccessibleFromSimpleChecker(Cell cell, Team team)
    {
        var result = new Dictionary<Cell, Unit>();
        foreach (var neighbourType in AcessibleNeighbourTypes(team))
        {
            if (_neighbours[cell].TryGetValue(neighbourType, out var neighbour))
                if (neighbour.CurrentUnit == null)
                    result.Add(neighbour, null);
                else if (neighbour.CurrentUnit.Team != team && neighbour != null && _neighbours[neighbour].TryGetValue(neighbourType, out var newNeighbour))
                    if (newNeighbour.CurrentUnit == null)
                        result.Add(newNeighbour, neighbour.CurrentUnit);
        }
        foreach (var neighbourType in AcessibleNeighbourTypes(OtherTeam(team)))
        {
            if (_neighbours[cell].TryGetValue(neighbourType, out var neighbour) && neighbour.CurrentUnit != null)
                if (neighbour.CurrentUnit.Team != team && neighbour != null && _neighbours[neighbour].TryGetValue(neighbourType, out var newNeighbour))
                    if (newNeighbour.CurrentUnit == null)
                        result.Add(newNeighbour, neighbour.CurrentUnit);
        }
        var attackResult = (result.Where(val => val.Value != null)).ToDictionary(el => el.Key, el => el.Value);
        if (attackResult.Any()) //we have to attack
            return attackResult;
        else
            return result;
    }
    private void RecountAccess()
    {
        foreach (var unit in _units)
            _assessibleCells[unit] = GetAccessibleFrom(unit.CurrentCell, unit);
        //TODO: force you to attack;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _neighbours[_cells[i]].Clear();
            _cells[i].OnPointerClickEvent -= OnCellClicked;
        }
        _neighbours.Clear();
        foreach (var unit in _units)
            _assessibleCells[unit].Clear();
        _assessibleCells.Clear();
        _dataManager.OnGameEvent -= ProcessEvent;
    }

}
