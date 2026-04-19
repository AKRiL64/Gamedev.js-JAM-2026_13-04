using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticle : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Collider collider;
    
    [Header("Lifetime")]
    [SerializeField] private int stepsLeft = 5, maxSteps = 5;

    [SerializeField] private float spawnDelay = 0.2f, lifetime = 5, destroyDelay = 5;
    

    [Header("Shape")] 
    private Vector3 baseDirection;

    [SerializeField] private Vector3 planeNormal;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] private float turnAngle = 45f;
    [SerializeField] private float spreadAngleDepleteion = 2f;
    [SerializeField] private int branchCount = 7;
    [SerializeField] private float spawnDistance = 1f;

    [Header("Collision Prevention")]
    [SerializeField] private float checkRadius = 0.4f;
    [SerializeField] private LayerMask smokeMask;

    private Vector3 planeForward;
    private Vector3 planeRight;
    private float spawnChance;
    private bool hasSpawned;

    private void Start()
    {
        if (stepsLeft <= 0) return;

        BuildPlane();
        StartCoroutine(Spawn());
        StartCoroutine(DieDelayed());
        float t = (float)stepsLeft / maxSteps;
        spawnChance = Mathf.Lerp(0.2f, 1f, t);
        var mainModule = particleSystem.main;
        mainModule.duration = lifetime;
        mainModule.startLifetime = lifetime;
        particleSystem.Play();
        
    }

    public void SetDirection(Vector3 direction)
    {
        baseDirection = direction;
    }
    private void BuildPlane()
    {
        planeNormal = planeNormal.normalized;

        planeForward = Vector3.ProjectOnPlane(baseDirection, planeNormal).normalized;

        if (planeForward.sqrMagnitude < 0.001f)
        {
            planeForward = Vector3.Cross(planeNormal, Vector3.up);
            if (planeForward.sqrMagnitude < 0.001f)
                planeForward = Vector3.Cross(planeNormal, Vector3.right);

            planeForward.Normalize();
        }

        planeRight = Vector3.Cross(planeNormal, planeForward).normalized;
    }

    private System.Collections.IEnumerator Spawn()
    {
        Debug.Log("spawn");
        if (hasSpawned) yield break;
        hasSpawned = true;

        yield return new WaitForSeconds(spawnDelay + spawnDelay*Random.Range(0.1f,0.4f));

        float step = (branchCount > 1)
            ? (spreadAngle * 2f) / (branchCount - 1)
            : 0f;

        float startAngle = -spreadAngle;

        for (int i = 0; i < branchCount; i++)
        {
            float angle = startAngle + step * i;

            Vector3 baseDir = Quaternion.AngleAxis(angle, planeNormal) * planeForward;
            
            Vector3 forwardDir = baseDir;
            Vector3 leftDir = Quaternion.AngleAxis(-turnAngle, planeNormal) * baseDir;
            Vector3 rightDir = Quaternion.AngleAxis(turnAngle, planeNormal) * baseDir;
            
            bool forwardFree = !Physics.CheckSphere(transform.position + forwardDir * spawnDistance, checkRadius, smokeMask);
            bool leftFree = !Physics.CheckSphere(transform.position + leftDir * spawnDistance, checkRadius, smokeMask);
            bool rightFree = !Physics.CheckSphere(transform.position + rightDir * spawnDistance, checkRadius, smokeMask);
            
            if (!forwardFree && !leftFree && !rightFree)
                continue;
            
            Vector3 dir = baseDir;

            if (forwardFree)
                dir = forwardDir;
            else if (leftFree)
                dir = leftDir;
            else if (rightFree)
                dir = rightDir;
            
            float jitter = Random.Range(-4, 4);
            dir = Quaternion.AngleAxis(jitter, planeNormal) * dir;

            Vector3 nextPos = transform.position + dir * spawnDistance;

            if (Physics.CheckSphere(nextPos, checkRadius, smokeMask))
                continue;

            GameObject next = Instantiate(smokePrefab, nextPos, Quaternion.identity);

            SmokeParticle sp = next.GetComponent<SmokeParticle>();
            if (sp != null)
            {
                sp.stepsLeft = stepsLeft - 1;
                sp.baseDirection = planeForward;
                sp.planeNormal = planeNormal;
                sp.spreadAngle = spreadAngle-spreadAngleDepleteion;
                sp.branchCount = branchCount;
                sp.spawnDistance = spawnDistance;
                sp.spawnDelay = spawnDelay;
                sp.smokePrefab = smokePrefab;
            }
        }
    }

    private IEnumerator DieDelayed()
    {
        yield return new WaitForSeconds(lifetime);
        collider.enabled = false;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}