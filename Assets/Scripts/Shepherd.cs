using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour
{
    private Nullable<Vector3> targetPosition;
    private Rigidbody rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (targetPosition.HasValue)
        {
            Vector3 direction = targetPosition.Value - transform.position;
            rb.velocity = direction.normalized * 5;
            if (Vector3.Distance(transform.position, targetPosition.Value) < 0.1f)
            {
                rb.velocity = Vector3.zero;
                transform.position = targetPosition.Value;
                targetPosition = null;
            }
        }
    }

    public void Select()
    {
        Debug.Log("Shepherd selected");
        // Play sound ouaf
    }

    public void Move(Vector3 targetPosition)
    {
        Debug.Log("Shepherd moving to " + targetPosition);
        this.targetPosition = targetPosition;
    }
}
