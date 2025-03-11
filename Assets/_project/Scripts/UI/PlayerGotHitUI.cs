using DG.Tweening;
using UnityEngine;

public class PlayerGotHitUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f).OnComplete(Hide);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0,.2f);
    }
}
