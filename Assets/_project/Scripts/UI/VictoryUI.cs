using DG.Tweening;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    public GameObject loadNextLevelButton;
    public GameObject gameCompletedText;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(bool isLastLevel = false)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f).SetDelay(.5f);
        if (isLastLevel)
        {
            loadNextLevelButton.SetActive(false);
            gameCompletedText.SetActive(true);
        }
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void LoadNextLevel()
    {
        gameDirector.RestartLevel();
        Hide();
    }
}
