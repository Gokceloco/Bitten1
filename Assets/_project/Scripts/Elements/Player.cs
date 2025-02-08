using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameDirector gameDirector;

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
    }

    private void Update()
    {
        var direction = Vector3.zero;

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

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
    }
}
