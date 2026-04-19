using System.Collections;
using UnityEngine;

public class SmokeWeapon : WeaponController
{
    [SerializeField] private float attackLength;
    private Camera mainCamera;
    private Vector3 direction;
    private Coroutine _attackCoroutine;
    private bool canHit = true;

    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private Transform pivot;
    void Awake()
    {
        mainCamera = Camera.main;

    }

    void Start()
    {
        playerController.OnSpecialInput += Attack;
        playerController.OnAttackInterruption += AttackInterrupt;
    }
    // Update is called once per frame
    protected override void Attack()
    {    
        
        RotateToMouse();
        if (_attackCoroutine == null && canHit)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        InvokeOnAttackStart();
        var s = Instantiate(smokePrefab, pivot.position, Quaternion.LookRotation(direction, Vector3.up));
        s.GetComponent<SmokeParticle>().SetDirection(direction);
        yield return new WaitForSeconds(attackLength);
        InvokeOnAttackEnd();
    }

    private void RotateToMouse()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            direction = hitPoint - transform.position;
            direction.y = 0f;

            transform.rotation = Quaternion.LookRotation(direction);

        }
    }
}
