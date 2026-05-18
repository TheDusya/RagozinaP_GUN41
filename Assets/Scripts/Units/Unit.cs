using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    public Cell CurrentCell { get; set; }
    public bool IsQueen { get; set; }
    public Team Team { get; set; }
    //seemed too small to give it a data struct
    private Material _material;
    private Material _transparentMaterial;
    private Renderer _renderer;

    public void OnPointerEnter(PointerEventData eventData) => CurrentCell.OnPointerEnter(eventData);
    public void OnPointerExit(PointerEventData eventData) => CurrentCell.OnPointerExit(eventData);
    public void OnPointerClick(PointerEventData eventData) => CurrentCell.OnPointerClick(eventData);
    public void SetMaterials(Material material, Material transparentMaterial)
    {
        _renderer = gameObject.GetOrAddComponent<Renderer>();
        _renderer.material = material;
        _material = material;
        _transparentMaterial = transparentMaterial;
    }
    public void TransparencyOn() => _renderer.material = _transparentMaterial;
    public void TransparencyOff() => _renderer.material = _material;
    public void EnterQueenMode()
    {
        IsQueen = true;
        if (gameObject.transform.Find("Crown") is Transform crown && crown != null)
            if (crown.gameObject.TryGetComponent<MeshRenderer>(out var crownRenderer))
                crownRenderer.enabled = true;
    }
    public void Kill()
    {
        CurrentCell.CurrentUnit = null;
        gameObject.SetActive(false);
    }
}
