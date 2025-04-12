using UnityEngine;
using TMPro;  // Use TMPro for UI text

public class StageDisplay : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the score.
    // (If you're using TextMeshPro for UI elements, you typically use TextMeshProUGUI.)
    public TextMeshProUGUI stageText;

    // Reference to the GridManager.
    public ShopPanel shopPanel;
    public GameManager gameManager;

    void Start()
    {
        if (shopPanel != null)
        {
            // Subscribe to the score updated event.
            shopPanel.OnContinue += UpdateStageText;
        }
        else
        {
            Debug.LogError("ShopPanel not found in the scene!");
        }

        if (gameManager != null)
        {
            // Subscribe to the score updated event.
            gameManager.OnStageDisplay += UpdateStageText;
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }

    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        if (shopPanel != null)
        {
            shopPanel.OnContinue -= UpdateStageText;
        }
    }

    // Event handler that updates the score text.
    private void UpdateStageText()
    {
        if (stageText != null)
        {
            stageText.text = gameManager.GetCurrentLevel().ToString();
        }
    }
}
