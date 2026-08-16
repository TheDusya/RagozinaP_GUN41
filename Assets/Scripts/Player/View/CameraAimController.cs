using Assets.Scripts.Parameters;
using Assets.Scripts.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraAimController : MonoBehaviour
{
    [Inject]
    PlayerSignalBus _signalBus;
    [Inject]
    AimingParameters _aimingParameters;

    float _normalFOV;
    float _aimingFOV;

    CinemachineVirtualCamera _virtualCamera;

    private void OnEnable()
    {
        _signalBus.Aim += OnAim;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _normalFOV = _virtualCamera.m_Lens.FieldOfView;
        _aimingFOV = _normalFOV * _aimingParameters.AimFOVShrinkCoeff;
    }

    private void OnAim(bool isAiming)
    {
        _virtualCamera.m_Lens.FieldOfView = isAiming ? _aimingFOV : _normalFOV;
    }

    private void OnDestroy()
    {
        _signalBus.Aim -= OnAim;
    }
}
