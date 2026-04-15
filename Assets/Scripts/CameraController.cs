using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private float speed;
    [SerializeField] private float offsetX = 0;
    [SerializeField] private float offsetY = 0;
    [SerializeField] private float offsetZ = 0;
    
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(offsetX, offsetY, offsetZ);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pivotTransform!=null)
            transform.position = Vector3.Lerp(transform.position, pivotTransform.position + offset, speed * Time.deltaTime);
    }
}
