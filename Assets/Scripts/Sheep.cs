using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Sheep : MonoBehaviour
{
    private GameController gameController;
    private List<Shepherd> shepherds;
    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        shepherds = gameController.Shepherds;
        
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 force = Vector3.zero;
        
        Vector3 shepherdForce = Vector3.zero;
        Vector3 flockCenter = Vector3.zero;
        Vector3 avoidanceForce = Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        int numNeighborsFlockCenter = 0;
        int numNeighborsAvoidOthers = 0;
        int numNeighborsAverageVelocity = 0;
        
        // Flees from the shepherds
        foreach (Shepherd shepherd in shepherds)
        {
            Vector3 direction = transform.position - shepherd.transform.position;
            float distance = direction.magnitude;
            if (distance < 10)
            {
                shepherdForce += direction.normalized * (gameController.ShepherdPerceptionRadius - distance);
            }
        }


        if (shepherdForce == Vector3.zero) // Only align when no shepherd (a bit more chaotic)
        {
            foreach (Sheep sheep in gameController.Sheeps)
            {
                // TODO if sheep is still in game
                if (sheep == this)
                {
                    continue;
                }

                // Go the center of the flock
                if (Vector3.Distance(transform.position, sheep.transform.position) < gameController.PerceptionRadius)
                {
                    flockCenter += sheep.transform.position;
                    numNeighborsFlockCenter++;
                }

                // Avoid others
                if (Vector3.Distance(transform.position, sheep.transform.position) < gameController.SeparationDistance)
                {
                    avoidanceForce += (transform.position - sheep.transform.position).normalized;
                    numNeighborsAvoidOthers++;
                }

                // Match velocity
                if (Vector3.Distance(transform.position, sheep.transform.position) < gameController.PerceptionRadius)
                {
                    averageVelocity += sheep.rb.velocity;
                    numNeighborsAverageVelocity++;
                }

            }
        }

        force += shepherdForce;
        
        if (numNeighborsFlockCenter > 0)
        {
            flockCenter /= numNeighborsFlockCenter;
            force += (flockCenter - transform.position) * gameController.FlocCenterForce;
        }

        if (numNeighborsAverageVelocity > 0)
        {
            averageVelocity /= numNeighborsAverageVelocity;
            force += averageVelocity * gameController.MatchVelocityForce;

        }
        if (numNeighborsAvoidOthers > 0) {
            avoidanceForce /= numNeighborsAvoidOthers;
            force += avoidanceForce * gameController.AvoidanceForce;
        }
        
        force.y = 0;
        rb.velocity += force;
        if (rb.velocity.magnitude > 0.5)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        // clamp velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, gameController.SheepVelocity);
    }
}
