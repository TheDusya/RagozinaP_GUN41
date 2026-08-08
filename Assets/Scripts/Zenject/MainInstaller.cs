using Assets.Scripts;
using Assets.Scripts.Player;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    PlayerParameters _playerParameters;
    [SerializeField]
    PlayerSignalBus _playerSignalBus;
    public override void InstallBindings()
    {
        if (_playerParameters != null)
            Container.Bind<PlayerParameters>().FromInstance(_playerParameters).AsSingle();
        else 
            Debug.LogError("PlayerParameters not found!");

        if (_playerSignalBus != null)
            Container.Bind<PlayerSignalBus>().FromInstance(_playerSignalBus).AsSingle();
        else
            Debug.LogError("PlayerSignalBus not found!");
    }
}