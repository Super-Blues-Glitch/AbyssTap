using UnityEngine;
using Zenject;

public class InputsManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputsManager>().FromComponentsOn(gameObject).AsSingle();
    }
}