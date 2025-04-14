using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] private GameObject title;       // The title UI element to disable while panel is active.
    [SerializeField] private Button closeButton;       // The button to close the panel.

    // Reference to MainMenuUI
    [SerializeField] private MainMenuUI mainMenuUI;

    private void Start()
    {
        // Subscribe to an event that should toggle this panel.
        // Replace 'OnHowToPlayPanelToggle' with your actual event.
        if (mainMenuUI != null)
        {
            mainMenuUI.OnHowToPlay += TogglePanel;
        }
        else
        {
            Debug.LogError("MainMenuUI is not assigned in HowToPlayPanel.");
        }

        // Ensure the close button triggers the closing animation.
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        else
        {
            Debug.LogError("CloseButton is not assigned in HowToPlayPanel.");
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks.
        if (mainMenuUI != null)
        {
            mainMenuUI.OnHowToPlay -= TogglePanel;
        }
    }

    /// <summary>
    /// TogglePanel is called when the corresponding event is fired.
    /// It shows the panel with an opening animation and disables the title.
    /// </summary>
    public void TogglePanel()
    {
        if (!gameObject.activeSelf)
        {
            // Activate the panel, disable the title.
            title.SetActive(false);
            gameObject.SetActive(true);

            // Reset scale to zero then animate to full size.
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.6f)
                .SetEase(Ease.OutBack);
        }
        else
        {
            // If panel is active, close it.
            OnCloseButtonClicked();
        }
    }

    /// <summary>
    /// Handles the close button click: plays a closing animation, disables this panel,
    /// and re-enables the title.
    /// </summary>
    private void OnCloseButtonClicked()
    {
        // Animate closing: scale down quickly.
        transform.DOScale(Vector3.zero, 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                // After the closing animation, disable the panel and re-enable the title.
                gameObject.SetActive(false);
                title.SetActive(true);
            });
    }
}
