using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event Action<Cell> OnPointerClickEvent;
    private enum Plane { Select, Focus };
    Dictionary<Plane, GameObject> _planes;
    private GameObject Select => _planes[Plane.Select];
    private GameObject Focus => _planes[Plane.Focus];
    void Start()
    {
        _planes = new Dictionary<Plane, GameObject>();
        foreach (Plane plane in Enum.GetValues(typeof(Plane)))
            if (transform.Find(plane.ToString()) is Transform found && found != null)
                _planes[plane] = found.gameObject;
            else
                Debug.LogError($"{plane} plane not found!");
    }
    public void OnPointerEnter(PointerEventData eventData) => Focus.GetOrAddComponent<MeshRenderer>().enabled = true;
    public void OnPointerExit(PointerEventData eventData) => Focus.GetOrAddComponent<MeshRenderer>().enabled = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this);
    }
    private void SetSelect(Material material)
    {
        Select.GetOrAddComponent<Renderer>().material = material;
        Select.GetOrAddComponent<MeshRenderer>().enabled = true;
    }
    private void ResetSelect() => Select.GetOrAddComponent<MeshRenderer>().enabled = false;
}
