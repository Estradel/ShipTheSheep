using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour
{
    private Nullable<Vector3> targetPosition;
    private Rigidbody rb;
    private GameController gameController;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        if (targetPosition.HasValue)
        {
            Vector3 direction = targetPosition.Value - transform.position;
            rb.velocity = direction.normalized * gameController.ShepherdVelocity;
            if (Vector3.Distance(transform.position, targetPosition.Value) < 0.1f)
            {
                rb.velocity = Vector3.zero;
                transform.position = targetPosition.Value;
                targetPosition = null;
            }
        }
        
        if (rb.velocity.magnitude > 0.5)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    public void Select()
    {
        // Play sound ouaf
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
