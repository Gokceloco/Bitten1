using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    public Image bg;

    public Color bgStartColor;
    public Color bgEndColor;
    public float fadeTime;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);

        bg.DOKill();
        bg.color = bgStartColor;
        bg.DOColor(bgEndColor, fadeTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(()=>gameObject.SetActive(false));
    }

    public void PlayGameButtonPressed()
    {
        Hide();
        gameDirector.RestartLevel(Vector3.zero);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
