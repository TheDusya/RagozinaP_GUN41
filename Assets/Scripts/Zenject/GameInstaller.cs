using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    Controls _controls;
    public override void InstallBindings()
    {
        _controls = new Controls();
        Container.BindInstance(_controls.Game.Get()).WithId("Game").AsSingle();
    }
    private void OnDestroy() => _controls.Dispose();
}