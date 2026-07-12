using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField]
    Button _endRoundButton;
    [SerializeField]
    TextManager _textManager;

    private EventManager _eventManager;
    private ScoreManager _scoreManager;

    public override void InstallBindings()
    {
        Container.BindInstance(_endRoundButton).AsSingle();
        Container.BindInstance(_ball).WithId("Ball");
        Container.BindInstance(_startingPoint).WithId("Start");
        var renderer = _field.GetComponent<Renderer>();
        float min = renderer.bounds.min.x + _borderWidth;
        float max = renderer.bounds.max.x - _borderWidth;
        Container.BindInstance((min, max)).AsSingle();
        Container.BindInstance(_gameParameters).AsSingle();
        _eventManager = new EventManager(_endRoundButton);
        Container.BindInstance(_eventManager).AsSingle();
        _scoreManager = new ScoreManager(_gameParameters, _eventManager, _textManager);
    }
}