using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    private Cell[] _cells;
    public Action<Cell> OnCellClicked;
    public Dictionary<Cell, Dictionary<NeighbourType, Cell>> _neighbours;
    void Awake()
    {
        _cells = FindObjectsOfType<Cell>();
        var positions = Array.ConvertAll(_cells, cell => cell.transform.position);
        _neighbours = new();
        var distance = _cells[0].transform.lossyScale.x; //assuming they are squares
        for (int i = 0; i < _cells.Length; i++)
        {
            _neighbours[_cells[i]] = new Dictionary<NeighbourType, Cell>();
            _cells[i].OnPointerClickEvent += OnCellClicked;
            for (int j = 0; j < _cells.Length; j++)
            {
                if (i == j) continue;
                var source = positions[i];
                var target = positions[j];
                var xDist = source.x - target.x;
                var zDist = source.z - target.z;
                if (Math.Abs(xDist) > distance || Math.Abs(zDist) > distance)
                    continue;
                var type = (Math.Sign(zDist), Math.Sign(xDist)) switch
                {
                    (1, 1) => NeighbourType.TOP_RIGHT,
                    (1, 0) => NeighbourType.TOP,
                    (1, -1) => NeighbourType.TOP_LEFT,
                    (0, 1) => NeighbourType.RIGHT,
                    (0, -1) => NeighbourType.LEFT,
                    (-1, 1) => NeighbourType.BOTTOM_RIGHT,
                    (-1, 0) => NeighbourType.BOTTOM,
                    (-1, -1) => NeighbourType.BOTTOM_LEFT,
                    _ => default
                };
                _neighbours[_cells[i]][type] = _cells[j];
            }
        }
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _neighbours[_cells[i]].Clear();
            _cells[i].OnPointerClickEvent -= OnCellClicked;
        }
        _neighbours.Clear();
    }

    public Cell GetCell(Unit unit)
    {
        var position = unit.transform.position;
        return _cells.OrderBy(el => (el.transform.position - position).sqrMagnitude).FirstOrDefault();
    }
}
