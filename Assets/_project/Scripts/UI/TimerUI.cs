using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Image timerBar;
    public TextMeshProUGUI remainingTimeTMP;
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
    public void UpdateTimer(float ratio, float remainingTime)
    {
        timerBar.fillAmount = ratio;
        remainingTimeTMP.text = Mathf.RoundToInt(remainingTime).ToString();
    }
}
