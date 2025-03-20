using DG.Tweening;
using TMPro;
using UnityEngine;

public class FailUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;
    public TextMeshProUGUI timeUpTMP;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(bool isTimeUp)
    {
        timeUpTMP.gameObject.SetActive(isTimeUp);

        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;

        //_canvasGroup.DOKill();
        //_canvasGroup.DOFade(1, .2f).SetDelay(2);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void RestartGameButtonPressed()
    {
        gameDirector.RestartLevel(Vector3.zero);
        Hide();
    }
}
