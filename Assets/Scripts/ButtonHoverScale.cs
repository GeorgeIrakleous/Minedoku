using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FlexibleHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = Vector3.one * 1.1f;
    public float tweenDuration = 0.2f;

    private Tween currentTween;

    public GameManager gameManager;

    private void Start()
    {
        gameManager=GameObject.FindFirstObjectByType<GameManager>();

        if (gameManager != null) { 
            gameManager.OnGameOver += DisableHoverEffect;
        }
        else
        {
            Debug.Log("Couldn't find GameManager game object");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(hoverScale, tweenDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTween?.Kill();
        currentTween = transform.DOScale(normalScale, tweenDuration).SetEase(Ease.OutQuad);
    }
    public void DisableHoverEffect()
    {
        currentTween?.Kill();
        transform.localScale = normalScale;
    }
}
