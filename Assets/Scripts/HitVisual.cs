using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVisual : MonoBehaviour
{
    Renderer rend;
    MaterialPropertyBlock block;
    private Hitable hitable;
    [SerializeField] private Transform particleSystem;
    [SerializeField] private float flashDuration = 0.2f, scaleEffectFactor = 1.1f;
    private Vector3 originalScale;
    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        block = new MaterialPropertyBlock();
    }

    private void Start()
    {
        originalScale = transform.localScale;
        hitable = GetComponentInParent<Hitable>();
        hitable.OnDamaged += OnHit;
    }

    void OnHit(Vector3 direction, float damage, GameObject attacker)
    {
        Quaternion rot = Quaternion.LookRotation(direction);
        Instantiate(particleSystem, transform.position, rot);
        StartCoroutine(FlashRoutine());
        StartCoroutine(ScaleHitRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0f;

        while (t < flashDuration)
        {
            t += Time.deltaTime;

            float amount = Mathf.Lerp(1f, 0f, t / flashDuration);

            rend.GetPropertyBlock(block);
            block.SetFloat("_FlashAmount", amount);
            rend.SetPropertyBlock(block);

            yield return null;
        }
        
        rend.GetPropertyBlock(block);
        block.SetFloat("_FlashAmount", 0f);
        rend.SetPropertyBlock(block);
    }
    
    IEnumerator ScaleHitRoutine()
    {
        float duration = 0.3f;
        float maxScaleMultiplier = scaleEffectFactor;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float normalized = t / duration;
            
            float scale = Mathf.Lerp(maxScaleMultiplier, 1f, normalized);

            transform.localScale = originalScale * scale;

            yield return null;
        }

        transform.localScale = originalScale;
    }
}
