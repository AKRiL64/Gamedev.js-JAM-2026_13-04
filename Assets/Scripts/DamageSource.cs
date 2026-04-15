using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackStrength = 100f;
    [SerializeField] private List<string> targetTags = new List<string> { "Hitable" };
    [SerializeField] private Transform collider;
    [SerializeField] private bool hitStop;
    private bool isBusy;

    private void Start()
    {
        //collider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("damage");
        if (targetTags.Contains(other.tag))
        {
            //TimeManager.Instance.HitStop(0.1f);
            if (other.TryGetComponent(out Hitable victim))
            {
                Vector3 hitDir = (other.transform.position - transform.position).normalized;
                hitDir.y = 0;

                victim.TakeDamage(damage, hitDir * knockbackStrength);
            }
        }
    }

    public void InflictDamage(float time, float delay, float cooldown)
    {
        if (!isBusy)
            StartCoroutine(InflictRoutine(time, delay,cooldown));
    }

    IEnumerator InflictRoutine(float time, float delay, float cooldown)
    {
        isBusy = true;
        yield return new WaitForSeconds(delay);
        collider.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        collider.gameObject.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        isBusy = false;
    }
}
