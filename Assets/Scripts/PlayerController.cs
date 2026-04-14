using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Default, Attacking, Dashing, Dead, Stunned }

    public PlayerState currentState;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private DamageSource currentWeapon;
    private Vector2 moveInput;
    
    private Rigidbody rb;
    private Hitable hitable;
    private Camera mainCamera;
    
    public event Action<Vector2> HandleMovement;
    public event Action OnDisableVisual;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitable = GetComponent<Hitable>();
        mainCamera = Camera.main;
    }
    
    void OnEnable()
    {
        hitable.OnDamaged += OnDamaged;
        hitable.OnDeath += OnDeath;
    }

    void OnDisable()
    {
        hitable.OnDamaged -= OnDamaged;
        hitable.OnDeath -= OnDeath;
    }

    void OnDamaged(Vector3 dir)
    {
        StartCoroutine(StunRoutine(0.5f));
    }

    private void OnDeath()
    {
        Debug.Log("dead");
        OnDisableVisual?.Invoke();
        currentState = PlayerState.Dead;
    }

    IEnumerator StunRoutine(float time)
    {
        currentState = PlayerState.Stunned;
        yield return new WaitForSeconds(time);
        currentState = PlayerState.Default;
    }
    

    void Update()
    {
        if (currentState == PlayerState.Default)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetMouseButtonDown(0))
        {
            RotateWeaponToMouse();
            currentWeapon.InflictDamage(0.1f,0,1);    
        }
        
        HandleMovement?.Invoke(moveInput);
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Default:
            {
                Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;
                rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
                break;
            }
            case PlayerState.Stunned:
            {
                break;
            }
        }
    }

    private void RotateWeaponToMouse()
    {
        Plane plane = new Plane(Vector3.up, currentWeapon.transform.position);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            Vector3 direction = hitPoint - currentWeapon.transform.position;
            direction.y = 0f;

            currentWeapon.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
