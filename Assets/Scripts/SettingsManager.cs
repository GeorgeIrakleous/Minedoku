using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    // Reference to the settings panel that will be animated.
    public GameObject settingsPanel;
    // Reference to the full-screen overlay that dims the background.
    public GameObject overlayPanel;

    public GridManager gridManager;  // Assigned in the Inspector.
    public Button backToMenuButton;
    public Button exitButton;

    private void Start()
    {
        // Ensure both the settings panel and overlay start disabled.
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (overlayPanel != null)
        {
            overlayPanel.SetActive(false);
        }

        backToMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        // Subscribe to the GridManager's event.
        if (gridManager != null)
        {
            gridManager.OnSettingsPanel += TogglePanel;
        }
        else
        {
            Debug.LogError("GridManager is not assigned in SettingsManager.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks.
        if (gridManager != null)
        {
            gridManager.OnSettingsPanel -= TogglePanel;
        }
    }

    // This method toggles the settings panel and overlay when the event is fired.
    public void TogglePanel()
    {
        // Check if the settings panel is currently active.
        if (settingsPanel.activeSelf)
        {
            // Animate pop-out: Scale from 1 to 0, then disable both panels.
            settingsPanel.transform.DOScale(0f, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    settingsPanel.SetActive(false);
                    overlayPanel.SetActive(false);
                });
        }
        else
        {
            // Activate the overlay first to block background interactions.
            overlayPanel.SetActive(true);
            // Activate the settings panel.
            settingsPanel.SetActive(true);
            // Reset the scale to zero before animating in.
            settingsPanel.transform.localScale = Vector3.zero;
            settingsPanel.transform.DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack);
        }
    }
}
