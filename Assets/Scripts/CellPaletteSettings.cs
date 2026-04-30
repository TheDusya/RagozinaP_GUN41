using UnityEngine;

[CreateAssetMenu(menuName = "Settings/CellPaletteSettings", fileName = "CellPaletteSettingsFile")]
public class CellPaletteSettings : ScriptableObject
{
    [field: SerializeField]
    public Material SelectCell { get; private set; }
    [field: SerializeField]
    public Material MoveCell { get; private set; }
    [field: SerializeField]
    public Material AttackCell { get; private set; }
    [field: SerializeField]
    public Material MoveAndAttackCell { get; private set; }
}
