using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{ 
    public static PlayerController Instance { get; private set; }
    public enum PlayerState { Default, Attacking, Dashing, Dead, Stunned }

    private PlayerActionRestrictor playerActionRestrictor;
    
    public PlayerState currentState;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private WeaponController specialWeapon;
    [SerializeField] private LightMeleeWeapon currentWeapon;
    private Vector2 moveInput;
    
    private Rigidbody rb;
    private Hitable hitable;


    
    public event Action<Vector2> HandleMovement;
    public event Action OnDisableVisual;
    public event Action OnAttackInput, OnAttackInterruption, OnSpecialInput;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        rb = GetComponent<Rigidbody>();
        hitable = GetComponent<Hitable>();
        currentWeapon.SetPlayerController(this);
        specialWeapon.SetPlayerController(this);
    }

    private void Start()
    {
        playerActionRestrictor = PlayerActionRestrictor.GetInstance();
    }
    
    void OnEnable()
    {
        hitable.OnDamaged += OnDamaged;
        hitable.OnDeath += OnDeath;
        currentWeapon.OnAttackStart += SetAttackingStateTrue;
        currentWeapon.OnAttackEnd += SetAttackingStateFalse;
        specialWeapon.OnAttackStart += SetAttackingStateTrue;
        specialWeapon.OnAttackEnd += SetAttackingStateFalse;
        
        currentWeapon.OnKnockback += ApplyKnockBack;
        currentWeapon.OnParry += OnParry;
    }

    void OnDisable()
    {
        hitable.OnDamaged -= OnDamaged;
        hitable.OnDeath -= OnDeath;
    }
    
    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void OnDamaged(Vector3 dir, float damage, GameObject attacker)
    {
        StartCoroutine(StunRoutine(0.5f));
        if (damage > currentWeapon.interruptDamage)
        {
            OnAttackInterruption?.Invoke();
        }
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
        if (playerActionRestrictor.IsRestricted()) return;
        
        if (currentState == PlayerState.Default)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackInput?.Invoke();
        }
            
        if (Input.GetMouseButtonDown(1))
        {
            OnSpecialInput?.Invoke();
        }
        
        HandleMovement?.Invoke(moveInput);
    }

    public void OnParry()
    {
        Debug.Log("player parried an attack");
        TimeManager.Instance.HitStop(0.5f);
        hitable.GiveIFrames(0.5f);
    }
    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Default:
            {
                Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;

                Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                Vector3 velocityChange = targetVelocity - currentVelocity;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
                break;
            }
            case PlayerState.Stunned:
            {
                break;
            }
        }
    }

    void SetAttackingStateTrue()
    {
        currentState = PlayerState.Attacking;
    }

    void SetAttackingStateFalse()
    {
        currentState = PlayerState.Default;
    }
    
    private void ApplyKnockBack(Vector3 force)
    {
        //rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }
    
}
