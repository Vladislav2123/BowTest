using UnityEngine;
using Zenject;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private ResultPanelUI _winPanel;
    [SerializeField] private ResultPanelUI _losePanel;

    [Inject] private LevelsManager _levelsManager;

    public void OnNextButton()
    {
        _levelsManager.LoadNextLevel();
    }

    public void OnRestartButton()
    {
        _levelsManager.RestartLevel();
    }

    public void ShowWinPanel(float delay)
    {
        Invoke(nameof(ShowWinPanel), delay);
    }
    public void ShowLosePanel(float delay)
    {
        Invoke(nameof(ShowLosePanel), delay);
    }

    public void ShowWinPanel()
    {
        if (_losePanel.IsShowing) _losePanel.TryHide();
        if (_winPanel.IsShowing == false) _winPanel.TryShow();
    }

    public void ShowLosePanel()
    {
        if (_winPanel.IsShowing) _winPanel.TryHide();
        if (_losePanel.IsShowing == false) _losePanel.TryShow();
    }
}