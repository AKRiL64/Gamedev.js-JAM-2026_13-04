using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{public float walkBobSpeed = 12f;
    public float walkBobAmount = 0.15f;
    public float tiltAmount = 5f;
    
    private PlayerController playerController;
    [SerializeField] private Transform spriteTransform;

    private Vector3 originalScale;
    private float timer;
    private float lastDirection = 1f;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        if (spriteTransform == null) spriteTransform = transform;
        originalScale = spriteTransform.localScale;
        
        lastDirection = Mathf.Sign(spriteTransform.localScale.x);
        
        playerController.OnMove += HandleMovement;
    }

    private void OnDestroy()
    {
        if (playerController != null)
            playerController.OnMove -= HandleMovement;
    }

    private void HandleMovement(Vector2 input)
    {
        if (input.x != 0)
        {
            lastDirection = input.x > 0 ? 1f : -1f;
        }

        if (input.magnitude > 0.1f)
        {
            timer += Time.deltaTime * walkBobSpeed;
            float bob = Mathf.Sin(timer) * walkBobAmount;
            
            spriteTransform.localScale = new Vector3(
                (Mathf.Abs(originalScale.x) + bob) * lastDirection, 
                originalScale.y - bob, 
                originalScale.z
            );
            
            float tilt = -input.x * tiltAmount;
            spriteTransform.localRotation = Quaternion.Euler(0, 0, tilt);
        }
        else
        {
            timer = 0;
            Vector3 targetIdleScale = new Vector3(Mathf.Abs(originalScale.x) * lastDirection, originalScale.y, originalScale.z);
            
            spriteTransform.localScale = Vector3.Lerp(spriteTransform.localScale, targetIdleScale, Time.deltaTime * 10f);
            spriteTransform.localRotation = Quaternion.Lerp(spriteTransform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
        }
    }
}
