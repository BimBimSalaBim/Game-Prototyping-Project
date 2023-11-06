using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimationState { Idle, Walk, Jump, Attack, Damage }
public enum BehaviourState { Idle, Wandering, SeekingFood, Fleeing }

public class FoV : Entity
{
    public AnimationState currentAnimationState; 
    public BehaviourState currentBehaviourState;
    public Animator animator;
    public NavMeshAgent navMeshAgent; 
    public string CreatureType;
    public float ViewDistance;
    public float FieldOfViewAngle;
    public bool canSeeTarget;
    public float movementSpeed = 5.0f;
    public float hungerThreshold = 25f;
    public float wanderTimer = 5f; // Time in seconds to wait before picking a new wander destination
    public float foodChaseDistance = 10f; // Distance within which the entity will chase the food even if it's out of FOV
    public float safeDistance = 20f; // Distance to flee before stopping
    private Vector3 damageSourcePosition; // Position from where the damage came
    private float wanderTimerCounter;
    public float healthThresholdForFleeing = 25f; // Health level below which the entity should flee
    private Transform targetFood; // The food target that the entity is currently tracking

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay");
        currentBehaviourState = BehaviourState.Wandering;
        wanderTimerCounter = wanderTimer;
    }

    void Update()
    {
    // Check if the entity needs to flee due to being hurt
        if (mHealth < healthThresholdForFleeing) // healthThresholdForFleeing is the health level below which the entity should flee
        {
            FleeFromDamageSource(); // This method contains the logic for fleeing
        }
        else
        {
            // Existing hunger logic
            if (mHunger <= hungerThreshold)
            {
                // When the entity is hungry, it should continuously seek food or wander.
                wanderTimer = 0.5f;
                if (targetFood != null && Vector3.Distance(transform.position, targetFood.position) <= foodChaseDistance)
                {
                    // Continue chasing the food if it's within range
                    navMeshAgent.SetDestination(targetFood.position);
                    currentAnimationState = AnimationState.Walk;
                }
                else
                {
                    // If the food is no longer in range or no food was found, try to find new food
                    FindVisibleTargets();
                }
            }
            else
            {
                // If the entity is not hungry, manage its wandering with a cooldown.
                targetFood = null; // Clear the food target since we're not hungry
                if (currentBehaviourState == BehaviourState.Wandering && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                {
                    if (wanderTimerCounter <= 0)
                    {
                        MoveRandomly();
                        // Reset the wander timer with some randomness to make movement more natural
                        wanderTimerCounter = wanderTimer + Random.Range(-2.0f, 2.0f);
                    }
                    else
                    {
                        wanderTimerCounter -= Time.deltaTime;
                    }
                }
            }

            // Animation state management
            ManageAnimationState();
        }
    }

    private void ManageAnimationState()
    {
        if (animator != null)
        {
            switch (currentAnimationState)
            {
                case AnimationState.Idle:
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        animator.SetFloat("Speed", 0);
                    }
                    break;

                case AnimationState.Walk:
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        animator.SetFloat("Speed", movementSpeed);
                    }
                    break;
            }
        }
    }

    private IEnumerator FindTargetsWithDelay()
    {
        float delay = 0.2f;
        WaitForSeconds Wait = new WaitForSeconds(delay);
        while (true)
        {
            yield return Wait;
            if (currentBehaviourState == BehaviourState.SeekingFood)
            {
                FindVisibleTargets();
            }
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ViewDistance);
        Transform closestFood = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hitCollider in hitColliders)
        {
            customTags customTagScript = hitCollider.gameObject.GetComponent<customTags>();
            if (customTagScript != null && customTagScript.HasTag(CreatureType + "Food")) // Check if the collider is tagged as "Food"
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    Vector3 directionToTarget = hitCollider.transform.position - transform.position;
                    if (Vector3.Angle(transform.forward, directionToTarget) < FieldOfViewAngle / 2)
                    {
                        RaycastHit hit;
                        // Check if there is a line of sight to the food
                        if (Physics.Raycast(transform.position, directionToTarget, out hit, ViewDistance))
                        {
                            if (customTagScript.HasTag(CreatureType + "Food"))
                            {
                                closestFood = hitCollider.transform;
                                closestDistance = distance;
                            }
                        }
                    }
                }
            }
        }

         if (closestFood != null)
        {
            // Food is in sight and within the field of view. Set it as the new destination
            targetFood = closestFood; // Update the target food
            navMeshAgent.SetDestination(targetFood.position);
            currentAnimationState = AnimationState.Walk;
            Debug.Log("Food found, heading towards it.");
        }
        else
        {
            // If no food found, ensure the entity continues wandering
            MoveRandomly();
            Debug.Log("No food found, continuing to wander.");
        }
    }

    void MoveRandomly()
    {
        if (currentBehaviourState == BehaviourState.Wandering)
        {
            // Check for a cooldown
            if (wanderTimerCounter > 0)
            {
                wanderTimerCounter -= Time.deltaTime;
                return; // Still in cooldown, do not change destination
            }

            Vector3 randomDirection = Random.insideUnitSphere * ViewDistance;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMeshPath path = new NavMeshPath();

            if (NavMesh.SamplePosition(randomDirection, out hit, ViewDistance, NavMesh.AllAreas))
            {
                if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        navMeshAgent.SetPath(path); // Set the complete path for the agent to follow
                        currentAnimationState = AnimationState.Walk;
                        
                        // Reset the wander timer with a randomized cooldown
                        wanderTimerCounter = wanderTimer + Random.Range(-2.0f, 2.0f);
                    }
                }
            }
        }
    }
    private void FleeFromDamageSource()
    {
        // Calculate direction to flee
        Vector3 fleeDirection = transform.position - damageSourcePosition;
        fleeDirection.y = 0; // Keep the entity on the same plane
        Vector3 fleePosition = transform.position + fleeDirection.normalized * safeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, safeDistance, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }

        // Check if the entity has fled far enough
        if (Vector3.Distance(transform.position, damageSourcePosition) >= safeDistance)
        {
            // Consider stopping the fleeing state and doing something else, like hiding or returning to wandering
            currentBehaviourState = BehaviourState.Idle; // or any other state
        }
    }
        // Call this method when the entity takes damage
    public void TakeDamage(float amount, Vector3 damageSource)
    {
        mHealth -= amount; // Assuming mHealth is a field representing health
        damageSourcePosition = damageSource;
        // currentBehaviourState = BehaviourState.Fleeing;

        // Optionally, trigger an animation or effect here
    }


    private void Move(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
        navMeshAgent.speed = movementSpeed;
        currentAnimationState = AnimationState.Walk;
    }

    private void StopAgent()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
        currentAnimationState = AnimationState.Idle;
    }

    public string getCreatureType()
    {
        return CreatureType;
    }

}
