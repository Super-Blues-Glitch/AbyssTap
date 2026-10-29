using UnityEngine;
using Zenject;

public class SceneManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScenesManager>().FromComponentOn(gameObject).AsSingle().NonLazy();
    }
}