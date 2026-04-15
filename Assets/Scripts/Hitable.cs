using System;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float currentHP;
    

    public event Action<Vector3, float> OnDamaged; 
    public event Action OnDeath; 

    void Start() => currentHP = maxHp;

    public void TakeDamage(float damage, Vector3 knockbackVector)
    {
        currentHP -= damage;
        OnDamaged?.Invoke(knockbackVector, damage);

        if (currentHP <= 0) Die();
    }

    private void Die() 
    {
        OnDeath.Invoke();
    }
}
