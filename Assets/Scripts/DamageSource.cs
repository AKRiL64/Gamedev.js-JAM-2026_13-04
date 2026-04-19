using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float knockbackStrength = 100f;
    [SerializeField] private Transform collider;
    [SerializeField] private Collider toIgnore;
    [SerializeField] private bool hitStop, canParry;
    [SerializeField] private GameObject owner;
    private bool isBusy, inWindow;
    private HashSet<Hitable> hitTargets = new HashSet<Hitable>();
    public event Action OnParry; 
    private void Start()
    {
        if (toIgnore != null && collider != null)
        {
            Collider myCol = collider.GetComponent<Collider>();

            if (myCol != null && toIgnore != null)
            {
                Physics.IgnoreCollision(myCol, toIgnore);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hitable victim) && !hitTargets.Contains(victim) && !inWindow)
        {
            Vector3 hitDir = (other.transform.position - transform.position).normalized;
            hitDir.y = 0;

            victim.TakeDamage(damage, hitDir * knockbackStrength, owner);
            hitTargets.Add(victim);
        }

        if (other.CompareTag("Parriable") && inWindow && canParry)
        {
            other.GetComponentInParent<EnemyController>().OnParried();
            OnParry?.Invoke();
        }
    }

    public void InflictDamage(float time, float delay, float cooldown)
    {
        if (!isBusy)
            StartCoroutine(InflictRoutine(time, delay,cooldown));
    }

    public void Interrupt()
    {
        StopAllCoroutines();
        collider.gameObject.SetActive(false);
        isBusy = false;
        inWindow = false;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    IEnumerator InflictRoutine(float time, float parryWindow, float cooldown)
    {
        isBusy = true;

        hitTargets.Clear();

        inWindow = true;
        Debug.Log("In window");
        collider.gameObject.SetActive(true);

        yield return new WaitForSeconds(parryWindow);

        inWindow = false;
        Debug.Log("Not window");

        yield return new WaitForSeconds(time);

        collider.gameObject.SetActive(false);

        yield return new WaitForSeconds(cooldown);

        isBusy = false;
    }
}
