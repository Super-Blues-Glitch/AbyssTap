using Zenject;

public class TimerManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TimerManager>().FromComponentOn(gameObject).AsSingle();
    }
}