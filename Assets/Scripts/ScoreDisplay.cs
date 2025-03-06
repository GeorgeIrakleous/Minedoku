using UnityEngine;
using TMPro;  // Use TMPro for UI text

public class ScoreDisplay : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the score.
    // (If you're using TextMeshPro for UI elements, you typically use TextMeshProUGUI.)
    public TextMeshProUGUI scoreText;

    // Reference to the GridManager.
    private GridManager gridManager;

    void Start()
    {
        // Find the GridManager in the scene.
        gridManager = UnityEngine.Object.FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            // Subscribe to the score updated event.
            gridManager.OnScoreUpdate += UpdateScoreText;
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        if (gridManager != null)
        {
            gridManager.OnScoreUpdate -= UpdateScoreText;
        }
    }

    // Event handler that updates the score text.
    private void UpdateScoreText(int newScore)
    {
        Debug.Log("display text updated");
        if (scoreText != null)
        {
            scoreText.text = "Score: "+newScore.ToString();
        }
    }
}
