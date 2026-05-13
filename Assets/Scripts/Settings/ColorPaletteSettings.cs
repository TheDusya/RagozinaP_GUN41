using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/CellPaletteSettings", fileName = "CellPaletteSettingsFile")]
public class ColorPaletteSettings : ScriptableObject
{
    [field: SerializeField]
    public Material SelectCell { get; private set; }
    [field: SerializeField]
    public Material MoveCell { get; private set; }
    [field: SerializeField]
    public Material AttackCell { get; private set; }
    [field: SerializeField]
    public Material MoveAndAttackCell { get; private set; }
    [field: SerializeField, Space(10)]
    public Material Team1Material { get; private set; }
    [field: SerializeField]
    public Material Team1TransparentMaterial { get; private set; }
    [field: SerializeField]
    public Material Team2Material { get; private set; }
    [field: SerializeField]
    public Material Team2TransparentMaterial { get; private set; }
    public Material GetTeamMaterial(Team team)
    {
        return team switch
        {
            Team.Player1 => Team1Material,
            Team.Player2 => Team2Material,
            _ => throw new NotImplementedException("Team material not found!"),
        };
    }
    public Material GetTeamTransparenMaterial(Team team)
    {
        return team switch
        {
            Team.Player1 => Team1TransparentMaterial,
            Team.Player2 => Team2TransparentMaterial,
            _ => throw new NotImplementedException("Team material not found!"),
        };
    }

}
