using UnityEngine;
using TMPro;  // Use TMPro for UI text

public class ScoreDisplay : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the score.
    // (If you're using TextMeshPro for UI elements, you typically use TextMeshProUGUI.)
    public TextMeshProUGUI scoreText;

    // Reference to the GridManager.
    private GameManager gameManager;

    void Start()
    {
        // Find the GridManager in the scene.
        gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            // Subscribe to the score updated event.
            gameManager.OnScoreDisplay += UpdateScoreText;
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        if (gameManager != null)
        {
            gameManager.OnScoreDisplay -= UpdateScoreText;
        }
    }

    // Event handler that updates the score text.
    private void UpdateScoreText(int level)
    {
        Debug.Log("display text updated");
        if (scoreText != null)
        {
            scoreText.text = "SCORE: "+level.ToString();
        }
    }
}
