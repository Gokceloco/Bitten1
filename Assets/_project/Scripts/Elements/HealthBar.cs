using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillBarParent;
    public SpriteRenderer fillBar;

    public Color flashColor;
    public void SetHealthBar(float ratio)
    {
        if (ratio >= 1f)
        {
            gameObject.SetActive(false);
        }
        else if(ratio <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        fillBarParent.DOScaleX(ratio, .3f);
        fillBar.DOColor(flashColor, .05f).SetLoops(2, LoopType.Yoyo);
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
