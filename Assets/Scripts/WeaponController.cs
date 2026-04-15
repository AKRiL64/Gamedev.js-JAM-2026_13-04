using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController playerController;
    public float interruptDamage;

    [SerializeField] protected DamageSource damageSource;
    void Awake()
    {

    }

    private void Start()
    {
        playerController.OnAttackInput += Attack;
        playerController.OnAttackInterruption += AttackInterrupt;
    }

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
}
