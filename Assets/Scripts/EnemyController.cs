using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    private Rigidbody rb;
    private Hitable hitable;
    public event Action<Vector3> OnHit;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitable = GetComponent<Hitable>();
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

    void OnDamaged(Vector3 direction, float damage)
    {
        OnHit?.Invoke(direction);
        Debug.Log("takeDamahgr");
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
