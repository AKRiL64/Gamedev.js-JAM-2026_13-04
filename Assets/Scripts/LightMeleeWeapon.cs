using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightMeleeWeapon : WeaponController
{
    [SerializeField] private float slashDuration = 0.2f;
    [SerializeField] private float slashRecover = 1f;
    [SerializeField] private float slashDelay = 1f;
    [SerializeField] private float comboReset = 0.5f;
    [SerializeField] private float slashAngle = 90f;
    [SerializeField] private float slashStrength = 10f;
    public float weight = 0.5f;
    private Vector3 direction;
    private int comboIndex = 0, maxCombo;
    private bool queuedNextAttack = false, isQueueAllowed = false, canHit = true;
    private Func<IEnumerator>[] comboAttacks;

    [SerializeField] private GameObject[] trails;
    private Coroutine _attackCoroutine;

    private Camera mainCamera;
    public event Action OnParry;
    void Awake()
    {
        mainCamera = Camera.main;
        comboAttacks = new Func<IEnumerator>[]
        {
            SlashRoutine,
            SlashRoutine
        };
        maxCombo = comboAttacks.Length;

    }
    
    void OnEnable()
    {
        damageSource.OnParry += InvokeParry;
    }

    private void OnDisable()
    {
        damageSource.OnParry -= InvokeParry;
    }

    void Start()
    {
        playerController.OnAttackInput += Attack;
        playerController.OnAttackInterruption += AttackInterrupt;
    }

    protected override void Attack()
    {    
        
        RotateToMouse();
        if (_attackCoroutine == null && canHit)
        {
            comboIndex = 0;
            StartNextAttack();
        }
        else if (isQueueAllowed)
        {
            queuedNextAttack = true;
        }
    }
    private void InvokeParry()
    {
        OnParry?.Invoke();
    }
    protected override void AttackInterrupt()
    {
        
    }
    
    private void StartNextAttack()
    {
        if (comboIndex < maxCombo)
        {
            _attackCoroutine = StartCoroutine(comboAttacks[comboIndex]());
        }
        else
        {
            InvokeOnAttackEnd();
            _attackCoroutine = null;
            canHit = false;
            StartCoroutine(WaitForDelay());
        }
    }
private IEnumerator SlashRoutine()
{
    float elapsed = 0f;
    
    isQueueAllowed = false;
    
    trails[comboIndex].SetActive(true);
    damageSource.InflictDamage(slashDuration,0,0);
    int index = comboIndex;
    InvokeOnAttackStart();
    if (comboIndex == 0)
    {
        InvokeKnockback(direction, slashStrength);
    }
    else
    {
        InvokeKnockback(direction, slashStrength * 0.5f);
    }
    while (elapsed < slashDuration)
    {
        if (elapsed > slashDuration * 0.4f)
        {
            isQueueAllowed = true;
        }
        elapsed += Time.deltaTime;
        yield return null;
    }
    trails[index].SetActive(false);
    comboIndex++;
    yield return new WaitForSeconds(slashRecover);
    if (queuedNextAttack)
    {
        queuedNextAttack = false; 
        StartNextAttack();
    }
    else
    {
        InvokeOnAttackEnd();
        _attackCoroutine = null;
        StartCoroutine(WaitForCombo());
    }
}

private IEnumerator WaitForDelay()
{
    yield return new WaitForSeconds(slashRecover);
    comboIndex = 0;
    canHit = true;
}
private IEnumerator WaitForCombo()
{
    float elapsed = 0f;
    while (elapsed < slashDuration)
    {
        elapsed += Time.deltaTime;
        if (queuedNextAttack)
        {
            StartNextAttack();
            isQueueAllowed = false;
            yield break;
        }
        yield return null;
    }
    isQueueAllowed = false;
    _attackCoroutine = null;
    canHit = false;
    StartCoroutine(WaitForDelay());
}




private void RotateToMouse()
{
    Plane plane = new Plane(Vector3.up, transform.position);
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

    if (plane.Raycast(ray, out float enter))
    {
        Vector3 hitPoint = ray.GetPoint(enter);
        direction = hitPoint - transform.position;
        direction.y = 0f;
        
        transform.rotation = Quaternion.LookRotation(direction);

    }

}
}
