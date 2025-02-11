using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillBarParent;
    public SpriteRenderer fillBar;

    public List<SpriteRenderer> healthUnits;

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

    /*public void SetHealthBar(int remainingHealth)
    {
        for (int i = 0; i < healthUnits.Count; i++) 
        {
            if (i < remainingHealth)
            {
                healthUnits[i].gameObject.SetActive(true);
                healthUnits[i].DOColor(flashColor, .1f).SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                healthUnits[i].gameObject.SetActive(false);
            }
        }
    }*/

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        
    }
}
