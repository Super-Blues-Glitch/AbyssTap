using UnityEngine;
using Zenject;

public class AudioManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AudioManager>().FromComponentOn(gameObject).AsSingle();
    }
}