using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.VisualScripting;
using StarterAssets;


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
    public float Hunger = 100f;
    private Coroutine hungerDamageCoroutine;
    public float healthThresholdForFleeing = 25f;
    public float safeDistance = 20f;
    public float foodChaseDistance = 10f;
        // Attack Properties
    public float attackRange = 2.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.0f;
    private float attackTimer;
    public Image agitationIndicator;
    private Color calmColor = Color.green;
    private Color agitatedColor = Color.red;
    private Color fleeingColor = Color.yellow;
    [SerializeField] private GameObject hitParticlePrefab; // Assign this in the Inspector


    
    // List of entities that have agitated this creature
    public List<GameObject> agitatedBy = new List<GameObject>();

    // State Management
    public AnimationState currentAnimationState; 
    public BehaviourState currentBehaviourState;

    // Private Variables
    private Transform targetFood;
    private Vector3 damageSourcePosition;
    private float wanderTimerCounter;
    public float wanderTimer = 5f; // Time in seconds to wait before picking a new wander destination
    
    public float health;
    public GameObject closestTarget;
    public Transform closestFood = null;
    // private GameObject gameController;
    // private AudioSource audioSource ;
     private AudioManager audioManager;
     private bool isAttackingPlayer = false;
    #endregion

    #region Unity Methods
    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay());
        SetInitialBehaviorState();
        agitationIndicator = GetComponentInChildren<Image>();
        // GameObject gameController = GameObject.Find("GameController");
            //add audio source
        // AudioSource audioSource = gameController.GetComponent<AudioSource>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    void Update()
    {
        CheckHealthStatus();
        CheckHungerStatus();
        CheckAgitationStatus();
        ManageAnimationState();
        Hunger -= Time.deltaTime;
        health = mHealth;

        if (currentBehaviourState == BehaviourState.Attacking && closestTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, closestTarget.transform.position);
            bool isTargetInViewDistance = distanceToTarget <= ViewDistance;

            if (isTargetInViewDistance)
            {
                if (distanceToTarget <= attackRange)
                {
                    StopAgent(); // Stop moving if within attack range
                }
                else
                {
                    Move(closestTarget.transform.position); // Move towards the target if it's out of attack range
                }
            }
            else
            {
                // Target is out of view distance, consider switching to another behavior
                currentBehaviourState = BehaviourState.Wandering;
            }
        }
        if (currentBehaviourState == BehaviourState.Attacking && closestTarget != null)
        {
            // Existing attack logic...

            // Check if the target is the player
            if (closestTarget.tag == "Player")
            {
                // Notify AudioManager when starting to attack the player
                if (!isAttackingPlayer)
                {
                    audioManager.StartAttack();
                    isAttackingPlayer = true;
                }
            }
            else if (isAttackingPlayer)
            {
                // Notify AudioManager when no longer attacking the player
                audioManager.StopAttack();
                isAttackingPlayer = false;
            }
        }
        else if (isAttackingPlayer)
        {
            // Ensure to notify AudioManager when the creature stops attacking
            audioManager.StopAttack();
            isAttackingPlayer = false;
        }
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
    // Update agitation indicator color based on whether the creature is agitated
    if(BehaviourState.Fleeing == currentBehaviourState)
    {
        agitationIndicator.color = fleeingColor;
    }
    else{

    agitationIndicator.color = agitatedBy.Count > 0 ? agitatedColor : calmColor;
    }

    if (agitatedBy.Count > 0 && currentBehaviourState != BehaviourState.Fleeing)
    {
        // //get game controller
        GameObject gameController = GameObject.Find("GameController");
        //add audio source
        AudioSource audioSource = gameController.GetComponent<AudioSource>();
        // if(audioSource.clip.name != "battle")
        // {
        //     //set audio clip
        //     audioSource.loop = true;
        //     audioSource.clip = Resources.Load<AudioClip>("Music/battle");
        //     //play audio
        //     audioSource.Play();
        // }

        GameObject target = ChooseAttackTarget();
        if (target != null)
        {
            currentBehaviourState = BehaviourState.Attacking;
            // if(target.tag == "Player")
            // {
            //     if(audioSource.clip.name != "battle")
            //     {
            //         //set audio clip
            //         audioSource.loop = true;
            //         audioSource.clip = Resources.Load<AudioClip>("Music/battle");
            //         //play audio
            //         audioSource.Play();
            //     }
            // }
            Move(target.transform.position);
            AttackTarget(target);
        }
        else
        {
            // if(targetFood.tag =="Player")
            // {
                // if(audioSource.clip.name == "battle")
                // {
                //     //set audio clip
                //     audioSource.loop = true;
                //     audioSource.clip = Resources.Load<AudioClip>("Music/nature");
                //     //play audio
                //     audioSource.Play();
                // }
            // }
            // Consider other behaviors if no valid target for attack
            EvaluateOtherBehaviors();
        }
    }
    else
    {
        // If not agitated, consider other behaviors
        EvaluateOtherBehaviors();
    }
}

private void EvaluateOtherBehaviors()
{
    if (mHealth < healthThresholdForFleeing)
    {
        currentBehaviourState = BehaviourState.Fleeing;
    }
    else if (mHunger <= hungerThreshold)
    {
        currentBehaviourState = BehaviourState.SeekingFood;
    }
    else if (currentBehaviourState != BehaviourState.Attacking)
    {
        currentBehaviourState = BehaviourState.Wandering;
        wanderTimer = 3.0f;
    }
    else if (currentBehaviourState == BehaviourState.Attacking)
    {
        wanderTimer = 0.0f;
    }
}
    private void CheckHealthStatus()
    {
        // Handle the entity's behavior when health is below a certain threshold
        if (mHealth < healthThresholdForFleeing && currentBehaviourState == BehaviourState.Fleeing)
        {
            FleeFromDamageSource();
        }
    }

    private void CheckHungerStatus()
    {
        // Handle the entity's behavior based on its hunger level
        if (mHunger <= hungerThreshold)
        {
            wanderTimer = 0.0f;
            SeekFood();
            if (hungerDamageCoroutine == null)
                hungerDamageCoroutine = StartCoroutine(HungerDamageRoutine());
        }
        else
        {
            if (hungerDamageCoroutine != null)
            {
                StopCoroutine(hungerDamageCoroutine);
                hungerDamageCoroutine = null;
            }
            Wander();
        }
    }
    private IEnumerator HungerDamageRoutine()
    {
        while (mHunger < hungerThreshold)
        {
            yield return new WaitForSeconds(5f);
            TakeDamage(1, transform.position); 
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

        // //Debug.Log("Distance to food: " + Vector3.Distance(transform.position, targetFood.position));
        // //Debug.Log("Attack range: " + attackRange);
        if (Vector3.Distance(transform.position, targetFood.position) <= attackRange)
        {
            // Initiate attack
            currentBehaviourState = BehaviourState.Attacking;
            Debug.Log("Attacking food"+ targetFood.tag);
            
        }
        
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
        if (currentBehaviourState == BehaviourState.Wandering || currentBehaviourState == BehaviourState.SeekingFood)
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
        agitatedColor = fleeingColor;
        Vector3 fleeDirection = transform.position + damageSourcePosition;
        try{foreach (GameObject entity in agitatedBy) { agitatedBy.Remove(entity); }}catch{Debug.Log("Target not found in list");}
        closestFood = null;
        targetFood = null;
        closestTarget = null;
        fleeDirection.y = 0;
        Vector3 fleePosition = transform.position + fleeDirection.normalized * safeDistance;
        navMeshAgent.speed = movementSpeed;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, safeDistance, NavMesh.AllAreas))
        {
            Move(fleePosition);
        }

        StartCoroutine(FleeForDuration(5f)); // Flee for 5 seconds
    }

    private IEnumerator FleeForDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentBehaviourState = BehaviourState.Wandering; // After 5 seconds, start wandering
    }
    #endregion

    #region Combat Methods
    public void TakeDamage(float amount, Vector3 damageSource)
    {
        
        mHealth -= amount;
        if (mHealth <= 0)
        {
            // Health has dropped to 0 or below, handle the collectible item
            HandleCollectibleItem();
            
        }
        else
        {
            damageSourcePosition = damageSource;
            currentBehaviourState = BehaviourState.Fleeing;
        }
        StartCoroutine(FlashEffect());
        PlayHitParticleEffect();
    }
    private void HandleCollectibleItem()
    {
        GenericAnimal genericAnimal = GetComponentInChildren<GenericAnimal>();
        if (genericAnimal != null )
        {
            genericAnimal.ActivateCollectibleItem();
        }
        else
        {
            //Debug.Log("GenericAnimal or CollectibleItem not found!");
        }
    }

    private IEnumerator FlashEffect()
    {
        Renderer creatureRenderer = GetComponentInChildren<Renderer>();
        if (creatureRenderer != null)
        {
            Color originalColor = creatureRenderer.material.color;
            creatureRenderer.material.color = Color.red; // Flash color
            yield return new WaitForSeconds(0.1f); // Duration of the flash
            creatureRenderer.material.color = originalColor; // Revert to original color
        }
    }
    private void PlayHitParticleEffect()
    {
        if (hitParticlePrefab != null)
        {
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        }
    }
    #endregion
    // Attack Methods region
    #region Attack Methods

    private GameObject ChooseAttackTarget()
    {
        // Logic to choose the closest agitating entity as the target
        
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject entity in agitatedBy )
        {
            if (entity == null){
                agitatedBy.Remove(entity);
                continue;
            }
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
    private void AttackTarget(GameObject target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        bool isTargetInViewDistance = distanceToTarget <= ViewDistance;

        if (distanceToTarget <= attackRange && isTargetInViewDistance && attackTimer <= 0)
        {
            // Reset the attack timer
            attackTimer = attackCooldown;

            // Face the target
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * movementSpeed);
            
            // Perform the attack
            //Debug.Log("Attacking the target!");
            try
            {
                target.GetComponent<FoV>().TakeDamage(attackDamage, transform.position);
                if(target.GetComponent<FoV>().mHealth <= 0)
                {
                    try
                    {
                        agitatedBy.Remove(target);
                    }
                    catch
                    {
                        //Debug.Log("Target not found in list");
                    }
                    GameObject.Destroy(target);
                    closestFood = null;
                    targetFood = null;
                    mHunger = 100f;
                    currentBehaviourState = BehaviourState.Wandering;

                }
                target.GetComponent<FoV>().Agitate(gameObject);
            }
            catch
            {
                try{
                    target.GetComponent<ThirdPersonController>().TakeDamage(attackDamage, transform.position);
                }
                catch
                {
                    //Debug.Log("Target not found in list");
                }
            }
            
            // animator.SetTrigger("Attack");
            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Agitate(GameObject agitator)
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
                                
                                break;
                            }
                        }
                    }
                }
            }
        }

         if (closestFood != null)
        {
            // get distance to closest food and check if it is within the chase distance
            float distanceToFood = Vector3.Distance(transform.position, closestFood.position);
            closestTarget = closestFood.gameObject;
            //Debug.Log("Distance to food: " + distanceToFood);
            //Debug.Log("Attack range: " + attackRange);
            if(distanceToFood <= attackRange)
            {
                // Initiate attack
                currentBehaviourState = BehaviourState.Attacking;
                targetFood = closestFood; // Set the target food as the current target
                
                Agitate(closestTarget);
                return;
            }
            // Food is in sight and within the field of view. Set it as the new destination
            targetFood = closestFood; // Update the target food
            navMeshAgent.SetDestination(targetFood.position);
            currentAnimationState = AnimationState.Walk;
            //Debug.Log("Food found, heading towards it.");
        }
        else
        {
            // If no food found, ensure the entity continues wandering
            MoveRandomly();
            //Debug.Log("No food found, continuing to wander.");
        }
    }


    private void Move(Vector3 targetPosition)
    {
        // Logic to move the entity to a target position
        navMeshAgent.SetDestination(targetPosition);
        navMeshAgent.isStopped = false;
        navMeshAgent.updateRotation = true;
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
