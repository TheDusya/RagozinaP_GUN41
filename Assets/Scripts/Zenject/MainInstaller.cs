using Assets.Scripts;
using Assets.Scripts.Parameters;
using Assets.Scripts.Player;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    AimingParameters _aimingParameters;
    [SerializeField]
    PlayerParameters _playerParameters;
    [SerializeField]
    WeaponParameters _weaponParameters;
    [SerializeField]
    PlayerSignalBus _playerSignalBus;
    public override void InstallBindings()
    {
        if (_aimingParameters != null)
            Container.Bind<AimingParameters>().FromInstance(_aimingParameters).AsSingle();
        else 
            Debug.LogError("AimingParameters not found!");

        if (_playerParameters != null)
            Container.Bind<PlayerParameters>().FromInstance(_playerParameters).AsSingle();
        else 
            Debug.LogError("PlayerParameters not found!");

        if (_weaponParameters != null)
            Container.Bind<WeaponParameters>().FromInstance(_weaponParameters).AsSingle();
        else 
            Debug.LogError("WeaponParameters not found!");

        if (_playerSignalBus != null)
            Container.Bind<PlayerSignalBus>().FromInstance(_playerSignalBus).AsSingle();
        else
            Debug.LogError("PlayerSignalBus not found!");
    }
}