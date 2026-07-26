using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    Animator _animator;
    [SerializeField]
    SearchTrigger _searchTrigger;
    [SerializeField]
    NavMeshAgent _navMeshAgent;
    public override void InstallBindings()
    {
        Container.Bind<IdleState>().AsSingle();
        Container.Bind<SearchState>().AsSingle();
        Container.Bind<CollectState>().AsSingle();

        Container.Bind<Animator>().FromInstance(_animator).AsSingle();
        Container.Bind<SearchTrigger>().FromInstance(_searchTrigger).AsSingle();
        Container.Bind<NavMeshAgent>().FromInstance(_navMeshAgent).AsSingle();

        Container.BindInterfacesAndSelfTo<StateBehaviour>().AsSingle();
    }
}