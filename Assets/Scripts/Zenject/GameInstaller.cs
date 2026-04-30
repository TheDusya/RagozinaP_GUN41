using Zenject;

public class GameInstaller : MonoInstaller
{
    private CellManager _cellManager;
    Controls _controls;
    public override void InstallBindings()
    {
        _controls = new Controls();
        Container.BindInstance(_controls.Game.Get()).WithId("Game").AsSingle();
        _cellManager = gameObject.AddComponent<CellManager>();
        Container.BindInstance(_cellManager).WithId("CellManager").AsSingle();
    }
    private void OnDestroy() => _controls.Dispose();
}