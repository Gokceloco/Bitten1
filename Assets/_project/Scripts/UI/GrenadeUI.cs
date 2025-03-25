using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeUI : MonoBehaviour
{
    public GrenadeManager grenadeManager;
    public Image coloredGrenadeImage;
    public Image grayGrenadeImage;

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void ShowDisabledImage()
    {
        coloredGrenadeImage.color = new Color(1,1,1,0);
        coloredGrenadeImage.DOFade(1 ,grenadeManager.grenadeCoolDown).SetEase(Ease.Linear);
        coloredGrenadeImage.transform.DOKill();
        coloredGrenadeImage.transform.localScale = Vector3.one;
    }
    public void ShowEnabledImage()
    {
        coloredGrenadeImage.transform.DOKill();
        coloredGrenadeImage.transform.localScale = Vector3.one;
        coloredGrenadeImage.transform.DOScale(1.2f, .2f).SetLoops(2, LoopType.Yoyo);
    }
}
