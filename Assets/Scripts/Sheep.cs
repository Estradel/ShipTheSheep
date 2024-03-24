using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public bool isConfined;
    private Animator animator;
    private GameController gameController;
    private float idleTimer;
    private Rigidbody rb;
    private List<Shepherd> shepherds;
    private SheepBehaviour sheepBehaviour;

    private Vector3? targetPosition;

    // Start is called before the first frame update
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sheepBehaviour = GetComponent<SheepBehaviour>();
        shepherds = gameController.Shepherds;

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameController.STATE == State.Pause) return;
        if (isConfined)
        {
            if (targetPosition.HasValue)
            {
                var direction = targetPosition.Value - transform.position;
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
                    var insideUnitCircle = Random.insideUnitCircle;
                    targetPosition = new Vector3(
                        gameController.confinedDetector.transform.position.x + insideUnitCircle.x *
                        gameController.confinedDetector.transform.localScale.x / 2f,
                        transform.position.y,
                        gameController.confinedDetector.transform.position.z + insideUnitCircle.y *
                        gameController.confinedDetector.transform.localScale.z / 2f);
                }
            }
        }
        else // If not confined
        {
            var force = Vector3.zero;

            var shepherdForce = Vector3.zero;
            var flockCenter = Vector3.zero;
            var avoidanceForce = Vector3.zero;
            var averageVelocity = Vector3.zero;
            var numNeighborsFlockCenter = 0;
            var numNeighborsAvoidOthers = 0;
            var numNeighborsAverageVelocity = 0;

            bool isScared = false;
            // Flees from the shepherds
            foreach (var shepherd in shepherds)
            {
                var direction = transform.position - shepherd.transform.position;
                var distance = direction.magnitude;
                if (distance < gameController.ShepherdPerceptionRadius)
                    shepherdForce += direction.normalized * (gameController.ShepherdPerceptionRadius - distance);

                if (distance < 4)
                {
                    sheepBehaviour.SetScared();
                    isScared = true;
                }
            }

            if (!isScared)
            {
                sheepBehaviour.SetIdle();
            }

                foreach (var sheep in gameController.Sheeps)
            {
                if (sheep == this || sheep.isConfined) continue;

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
                animator.SetBool("IsRunning", true);
            else
                animator.SetBool("IsRunning", false);

            // clamp velocity
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, gameController.SheepVelocity);

            if (gameController.levelController.isLevelSatisfactory)
            {
                var direction = new Vector3(15, 0, 0) - transform.position;
                direction.y = 0;
                rb.velocity += direction.normalized * 0.3f;
            }
        }
    }

    public void Confine()
    {
        if (!isConfined)
        {
            sheepBehaviour.SetHappy();
            isConfined = true;
            rb.velocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            idleTimer = 0;
            gameController.levelController.AddConfinedSheep();
        }
    }
}