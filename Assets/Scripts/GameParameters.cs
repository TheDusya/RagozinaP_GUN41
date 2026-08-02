using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters")]
public class GameParameters : ScriptableObject
{
    [SerializeField]
    private Material _pickedPathMaterial;
    [SerializeField]
    private Material _notPickedPathMaterial;
    [SerializeField]
    private float _basicMovementSpeed = 3;
    [SerializeField]
    private float _colorChangingTime = 3;

    public Material PickedPathMaterial { get => _pickedPathMaterial; }
    public Material NotPickedPathMaterial { get => _notPickedPathMaterial; }
    public float BasicMovementSpeed { get => _basicMovementSpeed; }
    public float ColorChangingTime { get => _colorChangingTime; }

    public void OnEnable()
    {
        if (_pickedPathMaterial == null || _notPickedPathMaterial == null)
            throw new System.Exception("Not all game parameters are filled in!");
    }

    public const string MovementDOTweenTag = "Movement";
    public const string ColorDOTweenTag = "Color";
    public const string IsCharacterMovingParameterName = "IsCharacterMoving";
    public const float SlowestMovementSquared = 0.005f;
}
