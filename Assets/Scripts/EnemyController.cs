using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    protected Rigidbody rb;
    protected Hitable hitable;

    protected Transform player;
    protected bool staggered;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitable = GetComponent<Hitable>();
    }
    
    void Start()
    {
        player = PlayerController.Instance.transform;
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

    public virtual void OnParried()
    {
        Debug.Log("Enemy got parried");
    }

    protected virtual void OnDamaged(Vector3 direction, float damage, GameObject attacker)
    {
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
