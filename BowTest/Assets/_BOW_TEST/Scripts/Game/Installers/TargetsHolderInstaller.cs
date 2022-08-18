using Zenject;

public class TargetsHolderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TargetsHolder>().FromNew().AsSingle();
    }
}