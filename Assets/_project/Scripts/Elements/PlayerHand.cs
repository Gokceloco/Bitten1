using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private Door _touchingDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            _touchingDoor = other.GetComponent<Door>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            _touchingDoor = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _touchingDoor)
        {
            _touchingDoor.Open();
        }
    }
}
