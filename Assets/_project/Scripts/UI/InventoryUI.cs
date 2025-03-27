using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameDirector gameDirector;

    public RectTransform container;

    private bool _isShowing;

    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }
    public void MachinegunButtonPressed()
    {
        gameDirector.player.weapon.LoadMachineGun();
        HideContainer();
    }
    public void ShotgunButtonPressed()
    {
        gameDirector.player.weapon.LoadShotGun();
        HideContainer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_isShowing)
            {
                HideContainer();
            }
            else
            {
                ShowContainer();
            }
        }
    }

    private void ShowContainer()
    {
        container.DOKill();
        container.DOAnchorPosX(0, .2f);
        _isShowing = true;
    }
    private void HideContainer()
    {
        container.DOKill();
        container.DOAnchorPosX(230, .2f);
        _isShowing = false;
    }
}
