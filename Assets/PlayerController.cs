using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
public float moveSpeed = 10f;
    private Rigidbody rb;
    private Vector2 moveInput;
    
    public event Action<Vector2> OnMove;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        OnMove?.Invoke(moveInput);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
