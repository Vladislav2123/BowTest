using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private float _restartDelay;
    [SerializeField] private bool _fading;

#if UNITY_EDITOR
    [Header("=== TESTING ===")]
    [SerializeField] private bool _isTesting;
    [SerializeField] private int _testingLevel;
#endif

    [Inject] private DiContainer _diContainer;
    [Inject] private UIFade _uiFade;

    private const string COMPLETED_LEVELS_KEY = "CompletedLevels";
    private const int LOOP_LEVEL_INDEX = 0;

    public int CurrentLevelNumber {get; private set;}
    public Level CurrentLevel {get; private set;}

    private void Awake()
    {
#if UNITY_EDITOR
        if (_isTesting)
        {
            LoadLevel(_testingLevel);
            return;
        }
#endif

        LoadLastLevel();
    }

    private void LoadLastLevel()
    {
        CurrentLevelNumber = PlayerPrefs.GetInt(COMPLETED_LEVELS_KEY, 1);

        if (CurrentLevelNumber > _levels.Length) CurrentLevelNumber = _levels.Length - 1;
        LoadLevel(CurrentLevelNumber);
    }

    public void LoadNextLevel(float delay)
    {
        Invoke(nameof(LoadNextLevel), delay);
    }
    public void LoadNextLevel()
    {
        SaveLevel();
        RestartLevel();
    }

    public void RestartLevel(float delay)
    {
        Invoke(nameof(RestartLevel), delay);
    }
    public void RestartLevel()
    {
        StartCoroutine(RestartRoutine());
    }

    private void SaveLevel()
    {
#if UNITY_EDITOR
        if (_isTesting) return;
#endif

        if (CurrentLevelNumber < _levels.Length) PlayerPrefs.SetInt(COMPLETED_LEVELS_KEY, CurrentLevelNumber + 1);
        else PlayerPrefs.SetInt(COMPLETED_LEVELS_KEY, LOOP_LEVEL_INDEX);
    }

    private void LoadLevel(int levelNumber)
    {
        CurrentLevelNumber = levelNumber;
        CurrentLevel =_diContainer.InstantiatePrefab(_levels[levelNumber -1], Vector3.zero, Quaternion.identity, null).GetComponent<Level>();
        if (_fading) _uiFade.FadeOut();
    }
    private IEnumerator RestartRoutine()
    {
        if (_fading) _uiFade.FadeIn();

        yield return new WaitForSeconds(_restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
} 
