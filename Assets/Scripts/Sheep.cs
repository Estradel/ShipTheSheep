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
    void Update()
    {
        Vector3 force = Vector3.zero;
        // Flees from the shepherds
        foreach (Shepherd shepherd in shepherds)
        {
            Vector3 direction = transform.position - shepherd.transform.position;
            float distance = direction.magnitude;
            if (distance < 10)
            {
                force += direction.normalized * (10 - distance);
            }
        }
        // Applies the force
        rb.velocity = Vector3.zero;
        force.y = 0;




        Vector3 flockCenter = Vector3.zero;
        Vector3 avoidanceForce = Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        int numNeighbors = 0;
        int numNeighborsAvoidOthers = 0;
        int numNeighborsAverageVelocity = 0;

        foreach (Sheep sheep in gameController.Sheeps)
        {
            // TODO if sheep is still in game
            if (sheep == this)
            {
                continue;
            }
            // Fly towards the center of the flock
            flockCenter += sheep.transform.position;
            
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
            
            numNeighbors++;
        }
        
        if (numNeighbors > 0)
        {
            flockCenter /= numNeighbors;
            averageVelocity /= numNeighborsAverageVelocity;
            avoidanceForce /= numNeighborsAvoidOthers;
            force += (flockCenter - transform.position) * 0.1f;
            force += avoidanceForce * 0.1f;
            force += (averageVelocity - rb.velocity) * 0.1f;
        }
        
        






        Debug.Log(force);
        // Rotation should be pointed to the direction of the force
        transform.localRotation = quaternion.RotateY(Mathf.Atan2(force.x, force.z));
        rb.velocity += force.normalized * 5;
    }
}
