using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private float interval = 0.5f;

    private readonly HashSet<Hitable> targets = new HashSet<Hitable>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hitable hit))
        {
            targets.Add(hit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Hitable hit))
        {
            targets.Remove(hit);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(ApplySmoke), 0f, interval);
    }

    private void ApplySmoke()
    {
        foreach (var target in targets)
        {
            if (target == null) continue;

            target.TakeDamage(
                damagePerSecond * interval,
                Vector3.zero,
                gameObject,
                DamageSource.DamageType.Smoke
            );
        }
    }
}