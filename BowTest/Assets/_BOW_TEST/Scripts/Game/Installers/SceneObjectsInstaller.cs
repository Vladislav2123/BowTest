using UnityEngine;
using Zenject;

public class SceneObjectsInstaller : MonoInstaller
{
    [SerializeField] private Archer _archer;

    public override void InstallBindings()
    {
        Container.Bind<Archer>().FromInstance(_archer).AsSingle();
    }
}