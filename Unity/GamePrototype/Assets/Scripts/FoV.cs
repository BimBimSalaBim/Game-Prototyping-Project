using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AnimationState { Idle, Walk, Jump, Attack, Damage }
public enum BehaviourState { Idle, Wandering, SeekingFood, Fleeing, Attacking }

public class FoV : Entity
{
    #region Fields and Properties
    // Components
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    // Attributes and Stats
    public string CreatureType;
    public float ViewDistance;
    public float FieldOfViewAngle;
    public float movementSpeed = 5.0f;
    public float hungerThreshold = 25f;
    public float healthThresholdForFleeing = 25f;
    public float safeDistance = 20f;
    public float foodChaseDistance = 10f;
        // Attack Properties
    public float attackRange = 2.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.0f;
    private float attackTimer;
    
    // List of entities that have agitated this creature
    private List<Entity> agitatedBy = new List<Entity>();

    // State Management
    public AnimationState currentAnimationState; 
    public BehaviourState currentBehaviourState;

    // Private Variables
    private Transform targetFood;
    private Vector3 damageSourcePosition;
    private float wanderTimerCounter;
    private float wanderTimer = 5f; // Time in seconds to wait before picking a new wander destination
    
    #endregion

    #region Unity Methods
    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay());
        SetInitialBehaviorState();
    }

    void Update()
    {
        CheckHealthStatus();
        CheckHungerStatus();
        CheckAgitationStatus();
        ManageAnimationState();
    }
    #endregion

    #region Behavior Methods
    private void SetInitialBehaviorState()
    {
        // Set the initial behavior state when the game starts
        currentBehaviourState = BehaviourState.Wandering;
        wanderTimerCounter = wanderTimer;
    }

    private void CheckAgitationStatus()
    {
        // Check if there are any agitators
        if (agitatedBy.Count > 0)
        {
            currentBehaviourState = BehaviourState.Attacking;
        }

        // State machine for behavior
        switch (currentBehaviourState)
        {
            case BehaviourState.Attacking:
                Entity target = ChooseAttackTarget();
                if (target != null)
                {
                    AttackTarget(target);
                }
                else
                {
                    // If there's no valid target, revert to another state, like wandering
                    currentBehaviourState = BehaviourState.Wandering;
                }
                break;
            case BehaviourState.Fleeing:
                FleeFromDamageSource();
                break;
            case BehaviourState.SeekingFood:
                Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewDistance);
                foreach (var localtarget in targetsInViewRadius)
                {
                    customTags customTagScript = localtarget.GetComponent<customTags>();
                    if (customTagScript != null && customTagScript.HasTag(CreatureType + "Food"))
                    {
                        float distance = Vector3.Distance(transform.position, localtarget.transform.position);
                        if (distance <= attackRange)
                        {
                            // Initiate attack
                            currentBehaviourState = BehaviourState.Attacking;
                            targetFood = localtarget.transform; // Set the target food as the current target
                            break;
                        }
                    }
                }
                break;

        }
    }
    private void CheckHealthStatus()
    {
        // Handle the entity's behavior when health is below a certain threshold
        if (mHealth < healthThresholdForFleeing)
        {
            FleeFromDamageSource();
        }
    }

    private void CheckHungerStatus()
    {
        // Handle the entity's behavior based on its hunger level
        if (mHunger <= hungerThreshold)
        {
            SeekFood();
        }
        else
        {
            Wander();
        }
    }

    private void SeekFood()
    {
        // Logic to seek food when hungry
        if (targetFood != null && Vector3.Distance(transform.position, targetFood.position) <= foodChaseDistance)
        {
            ChaseFood();
        }
        else
        {
            FindVisibleTargets();
        }
    }

    private void ChaseFood()
    {
        // Logic to chase the food if it has been found
        navMeshAgent.SetDestination(targetFood.position);
        currentAnimationState = AnimationState.Walk;
    }

    private void Wander()
    {
        // Logic to wander randomly when not hungry
        targetFood = null;
        if (currentBehaviourState == BehaviourState.Wandering && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            if (wanderTimerCounter <= 0)
            {
                MoveRandomly();
                ResetWanderTimer();
            }
            else
            {
                wanderTimerCounter -= Time.deltaTime;
            }
        }
    }
    #endregion

    #region Animation Methods
    private void ManageAnimationState()
    {
        // Logic to manage the animation states based on current behavior
        if (animator == null) return;

        switch (currentAnimationState)
        {
            case AnimationState.Idle:
                SetAnimatorSpeed(0, "Idle");
                break;
            case AnimationState.Walk:
                SetAnimatorSpeed(movementSpeed, "Walk");
                break;
        }
    }

    private void SetAnimatorSpeed(float speed, string animName)
    {
        // Helper method to set the animation speed
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animator.SetFloat("Speed", speed);
        }
    }
    #endregion

    #region Movement Methods
    private void MoveRandomly()
    {
        // Logic for moving the entity randomly
        if (currentBehaviourState == BehaviourState.Wandering)
        {
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
                        ResetWanderTimer();
                    }
                }
            }
        }
    }

    private void ResetWanderTimer()
    {
        // Helper method to reset the wander timer
        wanderTimerCounter = wanderTimer + Random.Range(-2.0f, 2.0f);
    }
    #endregion

    #region Fleeing Methods
    private void FleeFromDamageSource()
    {
        // Logic to flee from the damage source
        Vector3 fleeDirection = transform.position - damageSourcePosition;
        fleeDirection.y = 0;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * safeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, safeDistance, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }

        if (Vector3.Distance(transform.position, damageSourcePosition) >= safeDistance)
        {
            currentBehaviourState = BehaviourState.Idle;
        }
    }
    #endregion

    #region Combat Methods
    public void TakeDamage(float amount, Vector3 damageSource)
    {
        // Logic to handle when the entity takes damage
        mHealth -= amount;
        damageSourcePosition = damageSource;
        currentBehaviourState = BehaviourState.Fleeing;
    }
    #endregion
    // Attack Methods region
    #region Attack Methods

    private Entity ChooseAttackTarget()
    {
        // Logic to choose the closest agitating entity as the target
        Entity closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Entity entity in agitatedBy)
        {
            float distanceSqr = (entity.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestTarget = entity;
            }
        }

        // Check if the target is within attack range
        if (closestTarget != null && closestDistanceSqr <= attackRange * attackRange)
        {
            return closestTarget;
        }

        return null;
    }
    private void AttackTarget(Entity target)
    {
        if (attackTimer <= 0)
        {
            // Reset the attack timer
            attackTimer = attackCooldown;

            // Face the target
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * movementSpeed);

            // Perform the attack
            Debug.Log("Attacking the target!");
            // Here you would call a method like `target.TakeDamage(attackDamage)` to apply damage to the target
            // For now, let's just simulate it with a debug message
            target.transform.parent.GetComponent<FoV>().TakeDamage(attackDamage, transform.position);

            // Optionally, trigger an attack animation
            animator.SetTrigger("Attack");
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Agitate(Entity agitator)
    {
        // Logic to add an entity to the list of agitators
        if (!agitatedBy.Contains(agitator))
        {
            agitatedBy.Add(agitator);
        }
    }
    #endregion
    #region Utility Methods
    private IEnumerator FindTargetsWithDelay()
    {
        // Coroutine to find targets with a delay
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            yield return wait;
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


    private void Move(Vector3 targetPosition)
    {
        // Logic to move the entity to a target position
        navMeshAgent.SetDestination(targetPosition);
        navMeshAgent.speed = movementSpeed;
        currentAnimationState = AnimationState.Walk;
    }

    private void StopAgent()
    {
        // Logic to stop the NavMeshAgent
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
        currentAnimationState = AnimationState.Idle;
    }
    #endregion

    #region Accessors
    public string GetCreatureType()
    {
        // Logic to get the creature type
        return CreatureType;
    }
    #endregion
}
