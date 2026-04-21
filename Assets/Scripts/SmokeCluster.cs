using System.Collections.Generic;
using UnityEngine;

public class SmokeCluster : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.2f;
    [SerializeField] private float damage = 1f;

    private readonly HashSet<Hitable> targets = new HashSet<Hitable>();

    private void Start()
    {
        InvokeRepeating(nameof(ApplySmoke), tickRate, tickRate);
    }

    public void Register(Hitable h)
    {
        if (h != null)
            targets.Add(h);
    }

    public void Unregister(Hitable h)
    {
        if (h != null)
            targets.Remove(h);
    }

    private void ApplySmoke()
    {
        targets.RemoveWhere(t => t == null);

        foreach (var t in targets)
        {
            t.TakeDamage(
                damage * tickRate,
                Vector3.zero,
                gameObject,
                DamageSource.DamageType.Smoke
            );
        }
    }
}