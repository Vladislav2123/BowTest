using UnityEngine;
using Zenject;

public class GameServicesInstaller : MonoInstaller
{
    [SerializeField] private InputHandler _inputHabdler;
    [SerializeField] private GameStateController _gameStateController;
    [SerializeField] private LevelsManager _levelsManager;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromInstance(_inputHabdler);
        Container.Bind<GameStateController>().FromInstance(_gameStateController);
        Container.Bind<LevelsManager>().FromInstance(_levelsManager);
    }
}