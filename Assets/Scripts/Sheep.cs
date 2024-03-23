using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using DefaultNamespace;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

public class Sheep : MonoBehaviour
{
    private GameController gameController;
    private List<Shepherd> shepherds;
    private Rigidbody rb;
    private Animator animator;
    
    private Nullable<Vector3> targetPosition;
    private float idleTimer = 0;

    public bool isConfined = false;

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
        if (GameController.STATE == State.Pause)
        {
            return;
        }
        if (isConfined)
        {
            if (targetPosition.HasValue)
            {
                Vector3 direction = targetPosition.Value - transform.position;
                rb.velocity = direction.normalized * gameController.ShepherdVelocity;
                animator.SetBool("IsRunning", true);
                if (Vector3.Distance(transform.position, targetPosition.Value) < 0.8f)
                {
                    rb.velocity = Vector3.zero;
                    transform.position = targetPosition.Value;
                    targetPosition = null;
                    animator.SetBool("IsRunning", false);
                    idleTimer = Random.Range(1, 1.5f);
                }
            }
            else
            {
                idleTimer -= Time.fixedDeltaTime;
                if (idleTimer <= 0)
                {
                    Vector2 insideUnitCircle = Random.insideUnitCircle;
                    targetPosition = new Vector3(
                        gameController.confinedDetector.transform.position.x + (insideUnitCircle.x) * gameController.confinedDetector.transform.localScale.x / 2f,
                        this.transform.position.y,
                        gameController.confinedDetector.transform.position.z + (insideUnitCircle.y) * gameController.confinedDetector.transform.localScale.z / 2f);
                }
            }
        }
        else // If not confined
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
                if (distance < gameController.ShepherdPerceptionRadius)
                {
                    shepherdForce += direction.normalized * (gameController.ShepherdPerceptionRadius - distance);
                }
            }


            if (shepherdForce == Vector3.zero || true) // Only align when no shepherd (a bit more chaotic)
            {
                foreach (Sheep sheep in gameController.Sheeps)
                {
                    if (sheep == this || sheep.isConfined)
                    {
                        continue;
                    }

                    // Go the center of the flock
                    if (Vector3.Distance(transform.position, sheep.transform.position) <
                        gameController.PerceptionRadius)
                    {
                        flockCenter += sheep.transform.position;
                        numNeighborsFlockCenter++;
                    }

                    // Avoid others
                    if (Vector3.Distance(transform.position, sheep.transform.position) <
                        gameController.SeparationDistance)
                    {
                        avoidanceForce += (transform.position - sheep.transform.position).normalized;
                        numNeighborsAvoidOthers++;
                    }

                    // Match velocity
                    if (Vector3.Distance(transform.position, sheep.transform.position) <
                        gameController.PerceptionRadius)
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

            if (numNeighborsAvoidOthers > 0)
            {
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

    public void Confine()
    {
        if (!isConfined)
        {
            isConfined = true;
            rb.velocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            idleTimer = 0;
            gameController.levelController.AddConfinedSheep();
        }
    }
}