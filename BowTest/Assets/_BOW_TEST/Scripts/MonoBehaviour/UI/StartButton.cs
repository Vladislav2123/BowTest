using Zenject;

public class StartButton : UIPanel
{
    [Inject] private GameStateController _gameStateController;

    public void OnClick()
    {
        _gameStateController.TryStartGame();
    }
}
