using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelTMP;

    public void SetLevelText(int levelNo)
    {
        levelTMP.text = "LEVEL " + levelNo;
    }
}
