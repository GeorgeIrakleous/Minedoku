using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int level = 1;
    public int score = 0;
    public bool gameOver = false;

    // Event to notify when game over occurs.
    public event Action OnGameOver;
    public event Action OnLevelCompleted;
    public event Action OnRevealGrid;
    public event Action<int> OnScoreDisplay;
    public event Action OnStageDisplay;
    public event Action OnPlayAgain;

    // Reference to the GridManager (which is assumed to fire an event when score is zero)
    private GridManager gridManager;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private VolumeSettings volumeSettings;

    private void Awake()
    {
        // Set up the singleton.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        score = 0;
    }

    private void Start()
    {
        // Find and subscribe to the GridManager's OnScoreZero event.
        gridManager = GameObject.FindFirstObjectByType<GridManager>();

        if (gridManager != null)
        {
            gridManager.OnScoreZero += HandleScoreZero;
            gridManager.OnScoreMax += HandleLevelCompleted;
            gridManager.OnScoreUpdate += HandleScoreUpdate;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.OnPlayAgain += PlayAgain;
        }

        if (volumeSettings != null)
        {
            volumeSettings.Initialise();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        if (gridManager != null)
        {
            gridManager.OnScoreZero -= HandleScoreZero;
        }
    }

    // This method updates the score.
    // It adds the given multiplier to the score and checks if the score becomes zero.
    public void AddScore(int addedScore)
    {
        this.score += addedScore;
    }

    public void HandleScoreUpdate(int addedScore)
    {
        int displayScore = score + addedScore;
        OnScoreDisplay?.Invoke(displayScore);
    }

    // This method is called when the score reaches zero (either via the event or direct check).
    private void HandleScoreZero()
    {
        // At this point a gridManager notified the gameManager that the player lost
        EndGame();     
    }

    private void NextLevel()
    {
        level++;
        //Debug.Log("level: " + level);
    }

    private void HandleLevelCompleted(int levelScore)
    {
        SoundManager.Instance.PlaySfx("win");

        // At this point a gridManager notified the gameManager that the player completed a level
        //Debug.Log("You completed the level!");
        AddScore(levelScore);
        NextLevel();
        OnLevelCompleted?.Invoke();
        OnStageDisplay?.Invoke();
        OnRevealGrid?.Invoke();
    }
    private void EndGame()
    {
        gameOver = true;
        //Debug.Log("Game Over!");
        OnGameOver?.Invoke();
        OnRevealGrid?.Invoke();
        SoundManager.Instance.PlaySfx("lose");
    }

    private void PlayAgain()
    {
        level = 1;
        score = 0;
        gameOver = false;
        OnPlayAgain?.Invoke();
        OnScoreDisplay?.Invoke(score);
        OnStageDisplay?.Invoke();
    }

    public int GetCurrentLevel()
    {
        return level;
    }
}
