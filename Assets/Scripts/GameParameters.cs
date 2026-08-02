using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters")]
public class GameParameters : ScriptableObject
{
    [SerializeField]
    private Material _pickedPathMaterial;
    [SerializeField]
    private Material _notPickedPathMaterial;

    public Material PickedPathMaterial { get => _pickedPathMaterial; }
    public Material NotPickedPathMaterial { get => _notPickedPathMaterial; }

    public const string IsCharacterMovingParameterName = "IsCharacterMoving";
    public const float SlowestMovementSquared = 0.005f;
}
