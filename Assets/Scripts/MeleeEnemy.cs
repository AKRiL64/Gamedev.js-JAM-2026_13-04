using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyController
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float seeDistance = 6;
    [SerializeField] private float battleDistance = 3;
    [SerializeField] private float followDistance = 10;
    [SerializeField] private float attackDistance = 2.2f;
    [SerializeField] private float attackDelay = 0;
    [SerializeField] private float attackRecovery = 2.2f;
    [SerializeField] public bool inParryWindow,parried;
    
    [Header("Decision")]
    [SerializeField] private float decisionTime = 3f;

    [Header("Strafe")]
    [SerializeField] private float strafeSpeed = 2f;
    [SerializeField] private float minStrafeRadius = 2f;
    [SerializeField] private float maxStrafeRadius = 2.5f;
    [SerializeField] private float minDistanceFromPlayer = 1.8f;

    private NavMeshAgent agent;
    private DamageSource hitBox;

    [SerializeField] private Transform hitboxPivot;

    private Vector3 strafeTarget;
    private bool hasStrafeTarget;
    private bool isAttacking;
    private bool isDeciding;
    private Coroutine attackCoroutine;
    public enum EnemyState
    {
        Idle,
        Battle,
        Attack,
        Recovery,
        Stagger
    }

    private EnemyState state;

     void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        hitBox = hitboxPivot.GetComponentInChildren<DamageSource>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = attackDistance;
        agent.updateRotation = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:

                if (distance < seeDistance)
                    state = EnemyState.Attack;
                break;

            case EnemyState.Battle:

                if (distance > followDistance)
                {
                    state = EnemyState.Idle;
                    break;
                }

                if (distance > battleDistance)
                {
                    MoveToPlayer();
                }
                else
                {
                    if (!hasStrafeTarget)
                        StartCoroutine(StrafeRoutine());

                    MoveAroundPlayer();
                }

                if (!isDeciding)
                    StartCoroutine(DecisionRoutine());

                break;

            case EnemyState.Attack:
                if (distance > attackDistance)
                    MoveToPlayer();
                else if(!isAttacking && attackCoroutine==null)
                    attackCoroutine = StartCoroutine(AttackRoutine());
                break;

            case EnemyState.Recovery:
                break;

            case EnemyState.Stagger:
                break;
        }
    }

    void MoveToPlayer()
    {
        if (player == null) return;
        
        agent.speed = moveSpeed;
        agent.SetDestination(player.position);
    }

    void MoveAroundPlayer()
    {
        if (!hasStrafeTarget) return;
        
        agent.speed = strafeSpeed;
        agent.SetDestination(strafeTarget);
    }
    

    IEnumerator DecisionRoutine()
    {
        isDeciding = true;

        yield return new WaitForSeconds(decisionTime);

        if (Random.Range(0, 2) == 1)
        {
            state = EnemyState.Attack;
        }

        isDeciding = false;
    }
    

    IEnumerator StrafeRoutine()
    {
        hasStrafeTarget = true;
        
        Vector3 toEnemy = (transform.position - player.position).normalized;

        float radius = Random.Range(minStrafeRadius, maxStrafeRadius);
        float angle = Random.Range(-90f, 90f);

        Vector3 rotatedDir = Quaternion.Euler(0, angle, 0) * toEnemy;

        float distance = Random.Range(minDistanceFromPlayer, radius);

        strafeTarget = player.position + rotatedDir * distance;

        agent.SetDestination(strafeTarget);

        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        hasStrafeTarget = false;
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        state = EnemyState.Attack;

        agent.isStopped = true;

        yield return new WaitForSeconds(attackDelay);
        DealDamage();

        state = EnemyState.Recovery;

        yield return new WaitForSeconds(attackRecovery);

        state = EnemyState.Battle;
        isAttacking = false;
        agent.isStopped = false;
    }

    void DealDamage()
    {
        Vector3 dir = GetPlayerDirection();
        hitboxPivot.rotation = Quaternion.LookRotation(dir);
        hitBox.InflictDamage(0.1f, 0.5f, 0.1f);
    }

    Vector3 GetPlayerDirection()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;

        return lookDir != Vector3.zero ? lookDir : Vector3.forward;
    }

    protected override void OnDamaged(Vector3 direction, float damage, GameObject attacker)
    {
        if (state != EnemyState.Stagger)
        {
            StartCoroutine(StaggerRoutine());
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            isAttacking = false;
        }
    }

    IEnumerator StaggerRoutine()
    {
        staggered = true;
        EnemyState lastState = state;
        state = EnemyState.Stagger;

        agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        agent.enabled = true;
        agent.Warp(transform.position);
        staggered = false;
        state = EnemyState.Idle;
    }
    IEnumerator StunRoutine()
    {
        staggered = true;
        state = EnemyState.Stagger;

        agent.enabled = false;

        yield return new WaitForSeconds(5f);

        agent.enabled = true;
        agent.Warp(transform.position);
        staggered = false;
        state = EnemyState.Idle;
    }

    override public void OnParried()
    {
        Debug.Log("Meelee enemy got parried");
        
        hitBox.Interrupt();
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        isAttacking = false;
        
        StartCoroutine(StunRoutine());
    }
}