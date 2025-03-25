using DG.Tweening;
using UnityEngine;

public class Serum : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(transform.position.y + 1, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
