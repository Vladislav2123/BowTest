using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private BowTightnessIndicator _bowTightnessIndicator;
    [SerializeField] private StartButton _startButton;
    [SerializeField] private ResultUI _resultUI;
    [SerializeField] private UIFade _uiFade;

    public override void InstallBindings()
    {
        Container.Bind<BowTightnessIndicator>().FromInstance(_bowTightnessIndicator);
        Container.Bind<StartButton>().FromInstance(_startButton);
        Container.Bind<ResultUI>().FromInstance(_resultUI);
        Container.Bind<UIFade>().FromInstance(_uiFade);
    }
}