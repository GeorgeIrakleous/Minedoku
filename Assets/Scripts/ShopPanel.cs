using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    public Button continueButton;
    public event Action OnContinue;

    // Store the reference to the pulse tween so we can kill it later.
    private Tween pulseTween;

    [SerializeField] private float animationScaling = 0.015f;

    private void Start()
    {
        gameObject.SetActive(false);
        if (continueButton != null)
            continueButton.onClick.AddListener(HandleContinueClicked);

        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelCompleted += HandleLevelCompleted;
        else
            Debug.LogError("GameManager instance not found!");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelCompleted -= HandleLevelCompleted;
    }

    private void HandleLevelCompleted()
    {
        ShowPanel();
    }

    public void ShowPanel()
    {
        // Ensure no existing tweens interfere.
        if (pulseTween != null)
        {
            pulseTween.Kill();
            pulseTween = null;
        }

        // Activate panel and reset scale.
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;

        // Play the pop-up animation.
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetDelay(UnityEngine.Random.Range(0f, 0.3f))
            .OnComplete(() =>
            {
                // After the pop animation, start the subtle pulse loop.
                pulseTween = transform.DOScale(1.0f+animationScaling, 1f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }

    public void HidePanel()
    {
        // Kill the pulse tween if it exists.
        if (pulseTween != null)
        {
            pulseTween.Kill();
            pulseTween = null;
        }
        gameObject.SetActive(false);
    }

    private void HandleContinueClicked()
    {
        OnContinue?.Invoke();
        HidePanel();
    }
}
