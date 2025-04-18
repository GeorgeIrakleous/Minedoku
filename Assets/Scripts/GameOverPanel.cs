using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class GameOverPanel : MonoBehaviour
{
    public Button playAgainButton;
    public Button backToMenuButton;
    public event Action OnPlayAgain;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject newHighScoreText;

    private bool showNewHighScoreText;

    // Store the reference to the pulse tween so we can kill it later.
    private Tween pulseTween;

    [SerializeField] private float animationScaling = 0.015f;


    private void Awake()
    {
        if(backToMenuButton!=null)
            backToMenuButton.onClick.AddListener(() =>
            {
                OnPlayAgain?.Invoke();
                gameObject.SetActive(false);
                showNewHighScoreText = false;
            });

        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(() =>
            {
                showNewHighScoreText = false;
                SceneManager.LoadScene(0);
            });

        

    }
    private void Start()
    {
        gameObject.SetActive(false);

        

        if (GameManager.Instance != null) 
        { 
            GameManager.Instance.OnGameOver += HandleLevelCompleted;
            GameManager.Instance.OnNewHighScore += SetNewHighScoreTextTrue;
        }
        else
            Debug.LogError("GameManager instance not found!");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameOver -= HandleLevelCompleted;
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

        
        newHighScoreText.SetActive(showNewHighScoreText);
    
        transform.localScale = Vector3.zero;

        // Play the pop-up animation.
        transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetDelay(UnityEngine.Random.Range(0f, 0.3f))
            .OnComplete(() =>
            {
                // After the pop animation, start the subtle pulse loop.
                pulseTween = transform.DOScale(1.0f + animationScaling, 1f)
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

    private void SetNewHighScoreTextTrue()
    {
        showNewHighScoreText = true;
    }

}
