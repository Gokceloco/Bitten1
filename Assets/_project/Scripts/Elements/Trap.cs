using DG.Tweening;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Transform saw;

    private void Start()
    {
        saw.localPosition = new Vector3(-3,0,0);
        saw.DOLocalMoveX(3, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
