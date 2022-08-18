using UnityEngine;
using System;
using Zenject;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private float _resultPanelsShowingDelay;
    [SerializeField] private float _levelsLoadingDelay;

    public event Action OnMenuStateEvent;
    public event Action OnGameStartedEvent;
    public event Action OnGameEndedEvent;
    public event Action OnWinEvent;
    public event Action OnLoseEvent;

    [Inject] private TargetsHolder _targetsHolder;
    [Inject] private InputHandler _inputHandler;
    [Inject] private StartButton _startButton;
    [Inject] private ResultUI _resultUI;
    [Inject] private LevelsManager _levelsManager;

    public bool IsPlaying {get; private set;}

    private void Awake()
    {
        _targetsHolder.OnTargetsCountChangedEvent += CheckWin;
    }

    private void CheckWin()
    {
        if (_targetsHolder.IsEmpty) Win();
    }

    private void Start()
    {
        SetMenuState();
    }

    public void SetMenuState()
    {
        _inputHandler.IsBlocked = true;
        _startButton.TryShow();
        OnMenuStateEvent?.Invoke();
        IsPlaying = false;
    }

    public void TryStartGame()
    {
        if(IsPlaying) return;

        _inputHandler.IsBlocked = false;
        _startButton.TryHide();
        IsPlaying = true;
        OnGameStartedEvent?.Invoke();
    }

    public void Win()
    {
        EndGame();
        _resultUI.ShowWinPanel(_resultPanelsShowingDelay);
        _levelsManager.LoadNextLevel(_levelsLoadingDelay);
        OnWinEvent?.Invoke();
    }

    public void Lose()
    {
        EndGame();
        _resultUI.ShowLosePanel(_resultPanelsShowingDelay);
        _levelsManager.RestartLevel(_levelsLoadingDelay);
        OnLoseEvent?.Invoke();
    }

    public void EndGame()
    {
        IsPlaying = false;
        OnGameEndedEvent?.Invoke();
    }
}
