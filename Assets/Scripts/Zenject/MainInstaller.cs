using Assets.Scripts;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    PlayerParameters _playerParameters;
    public override void InstallBindings()
    {
        if (_playerParameters != null)
            Container.Bind<PlayerParameters>().FromInstance(_playerParameters).AsSingle();
        else 
            Debug.LogError("PlayerParameters not found!");
    }
}