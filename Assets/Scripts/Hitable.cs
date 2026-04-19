using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float currentHP;
    

    public event Action<Vector3, float, GameObject> OnDamaged; 
    public event Action OnDeath;

    private bool isHitable = true;
    private float hitRecovery = 0.3f;

    void Start() => currentHP = maxHp;

    public void TakeDamage(float damage, Vector3 knockbackVector, GameObject attacker)
    {
        //TimeManager.Instance.HitStop(0.08f + damage * 0.01f);
        if (isHitable)
        {
            currentHP -= damage;
            OnDamaged?.Invoke(knockbackVector, damage, attacker);

            if (currentHP <= 0) Die();
            GiveIFrames(hitRecovery);
        }
    }

    private void Die() 
    {
        TimeManager.Instance.HitStop(0.3f);
        OnDeath.Invoke();
    }

    IEnumerator GiveIFramesRoutine(float time)
    {
        isHitable = false;
        yield return new WaitForSeconds(time);
        isHitable = true;
    }

    public void GiveIFrames(float length)
    {
        StartCoroutine(GiveIFramesRoutine(length));
    }
}
