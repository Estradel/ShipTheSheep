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

    public float PerceptionRadius = 10f;
    public float SeparationDistance = 2f;
    
    
    
    private GameController gameController;
    private List<Shepherd> shepherds;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        shepherds = gameController.Shepherds;
        
        rb = GetComponent<Rigidbody>();
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
                shepherdForce += direction.normalized * (10 - distance);
            }
        }



        foreach (Sheep sheep in gameController.Sheeps)
        {
            // TODO if sheep is still in game
            if (sheep == this)
            {
                continue;
            }

            // Go the center of the flock
            if (Vector3.Distance(transform.position, sheep.transform.position) < PerceptionRadius)
            {
                flockCenter += sheep.transform.position;
                numNeighborsFlockCenter++;
            }

            // Avoid others
            if (Vector3.Distance(transform.position, sheep.transform.position) < SeparationDistance)
            {
                avoidanceForce += (transform.position - sheep.transform.position).normalized;
                numNeighborsAvoidOthers++;
            }
            
            // Match velocity
            if (Vector3.Distance(transform.position, sheep.transform.position) < PerceptionRadius)
            {
                averageVelocity += sheep.rb.velocity;
                numNeighborsAverageVelocity++;
            }
            
        }

        force += shepherdForce;
        
        if (numNeighborsFlockCenter > 0)
        {
            flockCenter /= numNeighborsFlockCenter;
            force += (flockCenter - transform.position) * 0.1f;
        }

        if (numNeighborsAverageVelocity > 0)
        {
            averageVelocity /= numNeighborsAverageVelocity;
            //force += (averageVelocity - rb.velocity) * 0.01f;

        }
        if (numNeighborsAvoidOthers > 0) {
            avoidanceForce /= numNeighborsAvoidOthers;
            force += avoidanceForce * 0.1f;
        }
        
        force.y = 0;
        transform.localRotation = quaternion.RotateY(Mathf.Atan2(force.x, force.z));
        rb.AddForce(force.normalized * 10);
        // clamp velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
    }
}
