using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    #region Singleton

    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
                Debug.LogWarning("Invalid singleton");
                Destroy(this);
            }
        }
    }

    #endregion Singleton

    //

    [SerializeField]
    private InputAction _Input_PostGameReturnToMenu = default;

    [Space]

    [SerializeField]
    private GameObject _PlayerPrefab = default;

    [SerializeField]
    private Transform _PlayerSpawnPoint = default;

    [Space]

    [SerializeField]
    private GameObject _GameOverCanvas = default;

    [SerializeField]
    private GameObject _LevelClearedCanvas = default;

    [Space]

    [SerializeField]
    private AudioSource _Audio_OnLevelCleared = default;

    //

    private enum GameState { Play, GameOver, LevelCleared }
    private GameState _gameState = GameState.Play;

    private List<EnemySpawner> _sortedEnemySpawners = new List<EnemySpawner>();
    private int _nextEnemySpawnerIndex = 0;

    private int _score = 0;

    //

    public static void EndGame(bool win)
    {
        if (instance._gameState == GameState.Play)
        {
            Player.Instance.gameObject.SetActive(false);
            instance._Input_PostGameReturnToMenu.Enable();

            if (win)
            {
                instance._gameState = GameState.LevelCleared;
                instance._LevelClearedCanvas.SetActive(true);

                instance._Audio_OnLevelCleared.Play();
            } else
            {
                instance._gameState = GameState.GameOver;
                instance._GameOverCanvas.SetActive(true);
            }
        }
    }

    public static void AddScore(int addScore)
    {
        instance._score += addScore;

        HUDCanvas.SetScoreLabel(instance._score);
    }

    //

    private void Start()
    {
        _GameOverCanvas.SetActive(false);
        _LevelClearedCanvas.SetActive(false);

        ViewPort.ResetPosition();

        Instantiate(_PlayerPrefab, _PlayerSpawnPoint.position, Quaternion.identity);

        EnemySpawner[] enemySpawners = FindObjectsOfType<EnemySpawner>();

        for (int i = 0; i < enemySpawners.Length; i ++)
        {
            if (_sortedEnemySpawners.Count == 0)
            {
                _sortedEnemySpawners.Add(enemySpawners[i]);
            } else
            {
                bool inserted = false;

                for (int j = 0; j < _sortedEnemySpawners.Count; j++)
                {
                    if (enemySpawners[i].GetActivationYPos() < _sortedEnemySpawners[j].GetActivationYPos())
                    {
                        _sortedEnemySpawners.Insert(j, enemySpawners[i]);
                        inserted = true;
                        break;
                    }
                }

                if (!inserted) _sortedEnemySpawners.Add(enemySpawners[i]);
            }
        }
    }

    private void Update()
    {
        if (_gameState == GameState.Play)
        {
            bool complete = ViewPort.Progress(Time.deltaTime);

            float sweepPos = ViewPort.SweepPosition;
            while (_nextEnemySpawnerIndex < _sortedEnemySpawners.Count && sweepPos >= _sortedEnemySpawners[_nextEnemySpawnerIndex].GetActivationYPos())
            {
                _sortedEnemySpawners[_nextEnemySpawnerIndex].SpawnEnemy();
                _nextEnemySpawnerIndex++;
            }

            if (complete) EndGame(true);
        } else
        {
            if (_Input_PostGameReturnToMenu.ReadValue<float>() > 0f && _Input_PostGameReturnToMenu.triggered)
            {
                SceneManager.LoadScene("StartMenu");
            }
        }
    }

}
