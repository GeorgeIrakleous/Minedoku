using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuUI: MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button howToPlayButton;

    public event Action OnHowToPlay;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        howToPlayButton.onClick.AddListener(() =>
        {
            OnHowToPlay?.Invoke();
        });
    }
}
