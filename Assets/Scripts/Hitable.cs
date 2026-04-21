using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hitable : MonoBehaviour
{
    [Serializable]
    public struct Resistance
    {
        public DamageSource.DamageType type;
        public float multiplier;
    }
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float currentHP;
    [FormerlySerializedAs("resistances")] [SerializeField]
    private List<Resistance> multipliers;
    private Dictionary<DamageSource.DamageType, float> multiplierMap;
    public event Action<Vector3, float, GameObject> OnDamaged; 
    public event Action OnDeath;

    private bool isHitable = true;
    private float hitRecovery = 0.3f;

    void Start() => currentHP = maxHp;
    void Awake()
    {
        multiplierMap = new Dictionary<DamageSource.DamageType, float>();

        foreach (var r in multipliers)
            multiplierMap[r.type] = r.multiplier;
        foreach (DamageSource.DamageType type in Enum.GetValues(typeof(DamageSource.DamageType)))
        {
            if (!multiplierMap.ContainsKey(type))
                multiplierMap[type] = 1f;
        }
    }
    public void TakeDamage(float damage, Vector3 knockbackVector, GameObject attacker, DamageSource.DamageType damageType)
    {
        //TimeManager.Instance.HitStop(0.08f + damage * 0.01f);
        if (isHitable)
        {
            float finalDamage = damage * GetResistance(damageType);
            if (finalDamage != 0)
            {
                currentHP -= finalDamage;
                OnDamaged?.Invoke(knockbackVector, finalDamage, attacker);

                if (currentHP <= 0) Die();
                GiveIFrames(hitRecovery);
            }
        }
    }
    public void SetMultiplier(DamageSource.DamageType type, float multiplier)
    {
        multiplierMap[type] = multiplier;
    }
    private float GetResistance(DamageSource.DamageType type)
    {
        return multiplierMap.TryGetValue(type, out var value) ? value : 1f;
    }
    private void Die() 
    {
        TimeManager.Instance.HitStop(0.3f);
        OnDeath?.Invoke();
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

    public float GetHealthFactor()
    {
        return currentHP / maxHp;
    }
}
