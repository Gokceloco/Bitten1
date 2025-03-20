using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public TextMeshProUGUI msgTMP;
    private CanvasGroup _canvasGroup;

    private float _lastMsgShowTime;
    private float _msgDuration;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(string msg, float duration)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f);
        msgTMP.text = msg;

        _lastMsgShowTime = Time.time;
        _msgDuration = duration;
        //Invoke(nameof(Hide), duration);
    }

    private void Update()
    {
        if (Time.time - _lastMsgShowTime > _msgDuration)
        {
            Hide();
        }
    }

    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }
}
