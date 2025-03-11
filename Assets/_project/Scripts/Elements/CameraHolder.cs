using DG.Tweening;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Camera mainCamera;

    public void ShakeCamera(float magnitude, float duration)
    {
        mainCamera.DOKill();
        mainCamera.transform.localPosition = new Vector3(0,7,-5);
        mainCamera.DOShakePosition(duration, magnitude, 10);
    }
}
