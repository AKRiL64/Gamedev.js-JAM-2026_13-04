using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Rigidbody rb;
    private Hitable hitable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitable = GetComponent<Hitable>();
    }

    void OnEnable()
    {
        hitable.OnDamaged += ApplyKnockBack;
    }

    void OnDisable()
    {
        hitable.OnDamaged -= ApplyKnockBack;
    }

    private void ApplyKnockBack(Vector3 force, float damage)
    {
        if (rb == null) return;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }
}