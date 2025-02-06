using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask groundLayerMask;

    public float speed;

    private Rigidbody _rb;

    public Transform cameraHolder;

    public float sensitivity;

    public Vector2 turn;
    public void RestartPlayer()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.position = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        /*var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        _rb.position += direction.normalized * speed * Time.deltaTime;

        var pos = transform.position;
        cameraHolder.position = pos;
        */



        /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            transform.LookAt(hit.point);
        }*/

        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");

        transform.localRotation = Quaternion.Euler(-turn.y * sensitivity, turn.x * sensitivity, 0);

        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= transform.right;
        }
        _rb.position += direction.normalized * speed * Time.deltaTime;

        var pos = transform.position;
        cameraHolder.position = pos;
        cameraHolder.rotation = transform.rotation;
    }
}
