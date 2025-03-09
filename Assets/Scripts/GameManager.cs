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

    // Event to notify when player completed a level.
    //public event Action OnLevelCompleted;

    // Reference to the GridManager (which is assumed to fire an event when score is zero)
    private GridManager gridManager;

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
        score += addedScore;
        Debug.Log("Score updated: " + score);
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
        Debug.Log("level: " + level);
    }

    private void HandleLevelCompleted(int levelScore)
    {
        // At this point a gridManager notified the gameManager that the player completed a level
        Debug.Log("You completed the level!");
        AddScore(levelScore);
        NextLevel();
        OnLevelCompleted?.Invoke();
    }
    public void EndGame()
    {
        gameOver = true;
        Debug.Log("Game Over!");
        OnGameOver?.Invoke();
    }

    public int GetCurrentLevel()
    {
        return level;
    }
}
