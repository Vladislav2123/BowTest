using UnityEngine;
using Zenject;

public class EffectsInstaller : MonoInstaller
{
    [SerializeField] private CameraShake _cameraSheke;

    public override void InstallBindings()
    {
        Container.Bind<CameraShake>().FromInstance(_cameraSheke).AsSingle();
    }
}