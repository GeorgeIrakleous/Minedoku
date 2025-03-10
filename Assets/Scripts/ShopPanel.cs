using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopPanel : MonoBehaviour
{
    // Reference to the Continue button (set in the Inspector).
    public Button continueButton;

    // Optional event if you want to notify other systems when the player clicks continue.
    public event Action OnContinue;

    private void Start()
    {
        // Ensure the panel is hidden initially.
        gameObject.SetActive(false);

        // Set up the continue button.
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(HandleContinueClicked);
        }

        // Subscribe to the GameManager's OnLevelCompleted event.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelCompleted += HandleLevelCompleted;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelCompleted -= HandleLevelCompleted;
        }
    }

    // This method is called when the GameManager fires the OnLevelCompleted event.
    private void HandleLevelCompleted()
    {
        ShowPanel();
    }

    // Enables the ShopPanel.
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // Disables the ShopPanel.
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    // Called when the Continue button is clicked.
    private void HandleContinueClicked()
    {
        OnContinue?.Invoke();
        HidePanel();
    }
}
