using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    protected PlayerController playerController;
    public float interruptDamage;

    [SerializeField] protected DamageSource damageSource;
    public event Action<Vector3> OnKnockback;
    public event Action OnAttackEnd;
    public event Action OnAttackStart;
    



    protected virtual void Attack()
    {
        
    }

    protected virtual void AttackInterrupt()
    {
        
    }

    public void SetPlayerController(PlayerController p)
    {
        playerController = p;
    }

    protected void InvokeOnAttackEnd()
    {
        OnAttackEnd?.Invoke();
    }

    protected void InvokeOnAttackStart()
    {
        OnAttackStart?.Invoke();
    }
    protected void InvokeKnockback(Vector3 dir, float str)
    {
        OnKnockback?.Invoke(dir * str);
    }


}
