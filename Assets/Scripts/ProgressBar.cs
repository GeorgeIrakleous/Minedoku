using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;

    [Tooltip("How fast the bar moves toward the new value (bigger = snappier)")]
    [SerializeField] private float fillSpeed = 5f;

    private float targetProgress = 0f;

    [SerializeField] private GridManager gridManager;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0f;
        targetProgress = 0f;
    }

    private void Start()
    {
        if (gridManager != null)
        {
            gridManager.OnProgressBarUpdate += OnProgressBarUpdate;
            gridManager.OnClearProgressBar += OnClearProgressBar;
        }
        else
        {
            Debug.LogError("ProgressBar: GridManager not assigned!");
        }
    }

    private void OnDestroy()
    {
        if (gridManager != null)
        {
            gridManager.OnProgressBarUpdate -= OnProgressBarUpdate;
            gridManager.OnClearProgressBar -= OnClearProgressBar;
        }
    }

    private void Update()
    {
        // Every frame, smoothly move the slider.value toward targetProgress
        if (!Mathf.Approximately(slider.value, targetProgress))
        {
            slider.value = Mathf.Lerp(
                slider.value,
                targetProgress,
                fillSpeed * Time.deltaTime
            );
        }
    }

    private void OnProgressBarUpdate(float progress)
    {
        // clamp just in case, and set as the new goal
        targetProgress = Mathf.Clamp01(progress);
    }

    private void OnClearProgressBar()
    {
        targetProgress = 0f;
        slider.value = 0f;
    }
}
