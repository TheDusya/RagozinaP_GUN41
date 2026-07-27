using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    AudioSource FireSound;
    [SerializeField]
    AudioSource HitSound;
    [SerializeField]
    AudioSource ReloadSound;
    public override void InstallBindings()
    {
        Container.Bind<AudioSource>().WithId("FireSound").FromInstance(FireSound);
        Container.Bind<AudioSource>().WithId("HitSound").FromInstance(HitSound);
        Container.Bind<AudioSource>().WithId("ReloadSound").FromInstance(ReloadSound);
    }
}