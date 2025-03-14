using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leaf1;
    public Transform leaf2;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }
    public void Open()
    {
        leaf1.DOLocalMoveZ(1.35f, .5f);
        leaf2.DOLocalMoveZ(-2.8f, .5f);
        _collider.isTrigger = true;
    }
    public void Close()
    {
        leaf1.DOLocalMoveZ(0f, .5f);
        leaf2.DOLocalMoveZ(-1.4f, .5f);
        _collider.isTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();
        }
    }
}
