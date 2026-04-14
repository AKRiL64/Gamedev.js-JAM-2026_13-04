using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pivotTransform!=null)
            transform.position = Vector3.Lerp(transform.position, pivotTransform.position, speed * Time.deltaTime);
    }
}
