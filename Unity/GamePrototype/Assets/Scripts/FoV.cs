using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState { Idle,Walk,Jump,Attack,Damage}
public class FoV : MonoBehaviour
{

    public AnimationState currentState; 
    public Animator animator;
    public string CreatureType;
    public float ViewDistance;
    public float FieldOfViewAngle;
    public bool canSeeTarget;
    

    public float movementSpeed = 5.0f;

    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    private void Start()
    {
        
        StartCoroutine("FindTargetsWithDelay");
        
    }
     void Update()
    {
        switch (currentState)
        {
            case AnimationState.Idle:
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                StopAgent();
                // SetFace(faces.Idleface);
                break;

            case AnimationState.Walk:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

                navMeshAgent.isStopped = false;
                navMeshAgent.updateRotation = true;

                // navMeshAgent.SetDestination(originPos);
                // Debug.Log("WalkToOrg");
                // SetFace(faces.WalkFace);
                // agent reaches the destination
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    

                    //facing to camera
                    transform.rotation = Quaternion.identity;

                    currentState = AnimationState.Idle;
                }
                       
            
               
                // set Speed parameter synchronized with agent root motion moverment
                animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
                

                break;
        }
    }


    private IEnumerator FindTargetsWithDelay()
    {
        float delay = 0.2f;
        WaitForSeconds Wait = new WaitForSeconds(delay);
        Debug.Log("Coroutine started");
        while (true)
        {
            yield return Wait;
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        bool foodFound = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ViewDistance);

        foreach (var hitCollider in hitColliders)
        {
            customTags customTagScript = hitCollider.gameObject.GetComponent<customTags>();

            if (customTagScript != null)
            {
                Vector3 directionToTarget = hitCollider.transform.position - transform.position;

                if (Vector3.Angle(transform.forward, directionToTarget) < FieldOfViewAngle / 2)
                {
                    if (customTagScript.HasTag(CreatureType + "Food"))
                    {
                        // Food is in sight. Set it as the new destination
                        navMeshAgent.SetDestination(hitCollider.transform.position);
                        Debug.Log("Food found");
                        foodFound = true;
                        break;
                    }
                }
            }
        }
        
        // If no food found, move in a random direction
        if (!foodFound && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            MoveRandomly();
        }
    }

    void MoveRandomly()
    {
        currentState = AnimationState.Walk;
        Vector3 randomDirection = Random.insideUnitSphere * ViewDistance; 
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, ViewDistance, 1); 
        navMeshAgent.SetDestination(hit.position);
    }

    private void StopAgent()
    {
        navMeshAgent.isStopped = true;
        animator.SetFloat("Speed", 0);
        navMeshAgent.updateRotation = false;
    }


}
