using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMeleeWeapon : WeaponController
{
    [SerializeField] private float slashDuration = 0.2f;
    [SerializeField] private float slashRecover = 1f;
    [SerializeField] private float comboReset = 0.5f;
    [SerializeField] private float slashAngle = 90f;
    
    private int comboIndex = 0, maxCombo;
    private bool queuedNextAttack = false, isQueueAllowed = false, canHit = true;
    private Func<IEnumerator>[] comboAttacks;

    [SerializeField] private GameObject trail;
    private Coroutine _attackCoroutine;

    private Camera mainCamera;
    
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

    protected override void AttackInterrupt()
    {
        
    }
    
    private void StartNextAttack()
    {
        if (comboIndex < maxCombo)
        {
            _attackCoroutine = StartCoroutine(comboAttacks[comboIndex]());
            comboIndex++;
        }
        else
        {
            _attackCoroutine = null;
            canHit = false;
            StartCoroutine(WaitForRecover());
        }
    }
private IEnumerator SlashRoutine()
{
    float elapsed = 0f;
    
    trail.SetActive(true);
    damageSource.SetColliderActive(true);

    while (elapsed < slashDuration)
    {
        if (elapsed > slashDuration * 0.4f)
        {
            isQueueAllowed = true;
        }
        elapsed += Time.deltaTime;
        yield return null;
    }
    
    damageSource.SetColliderActive(false);
    trail.SetActive(false);
    if (queuedNextAttack)
    {
        queuedNextAttack = false; 
        StartNextAttack();
    }
    else
    {
        _attackCoroutine = null;
        canHit = false;
        StartCoroutine(WaitForRecover());
    }
}

private IEnumerator WaitForRecover()
{
    yield return new WaitForSeconds(slashRecover);
    comboIndex = 0;
    canHit = true;
}private IEnumerator WaitForCombo()
{
    float elapsed = 0f;
    while (elapsed < slashDuration)
    {
        if (elapsed > slashDuration * 0.4f)
        {
            isQueueAllowed = true;
        }
        elapsed += Time.deltaTime;
        yield return null;
    }
}




private void RotateToMouse()
{
    Plane plane = new Plane(Vector3.up, transform.position);
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

    if (plane.Raycast(ray, out float enter))
    {
        Vector3 hitPoint = ray.GetPoint(enter);
        Vector3 direction = hitPoint - transform.position;
        direction.y = 0f;
        
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
}
