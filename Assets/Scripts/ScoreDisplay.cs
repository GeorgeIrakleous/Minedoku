using UnityEngine;
using TMPro;  // Use TMPro for UI text
using DG.Tweening;

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
            scoreText.text = level.ToString();
            AnimateScoreText();
        }
    }

    private void AnimateScoreText()
    {
        // Get the RectTransform component from the scoreText
        RectTransform rt = scoreText.GetComponent<RectTransform>();
        if (rt != null)
        {
            // Ensure the pivot is centered so scaling doesn't shift the object
            rt.pivot = new Vector2(0.5f, 0.5f);

            // Create a DOTween sequence to scale up then back to normal
            Sequence seq = DOTween.Sequence();
            seq.Append(rt.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad));
            seq.Append(rt.DOScale(1f, 0.15f).SetEase(Ease.InQuad));
        }
    }

}
