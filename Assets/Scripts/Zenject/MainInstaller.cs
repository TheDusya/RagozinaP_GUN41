using Assets.Scripts;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    GameObject _ball;
    [SerializeField]
    private GameObject _startingPoint;
    [SerializeField]
    private GameObject _field;
    [SerializeField]
    private float _borderWidth;
    [SerializeField]
    GameParameters _gameParameters;

    public override void InstallBindings()
    {
        Container.BindInstance(_ball).WithId("Ball");
        Container.BindInstance(_startingPoint).WithId("Start");
        var renderer = _field.GetComponent<Renderer>();
        float min = renderer.bounds.min.x + _borderWidth;
        float max = renderer.bounds.max.x - _borderWidth;
        Container.BindInstance((min, max)).AsSingle();
        Container.BindInstance(_gameParameters).AsSingle();
    }
}