using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    [SerializeField] private Animator animator;
    private float runSpeed;
    [SerializeField] private float runSpeedMultiplier;

    // Patrolling
    [SerializeField] private Transform[] points;
    private int currentPoint;
    
    // Attacking
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentPoint = 0;
        runSpeed = agent.speed * runSpeedMultiplier;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        animator.SetBool("isWalking", true);
        if (agent.transform.position.x != points[currentPoint].position.x && agent.transform.position.y != points[currentPoint].position.y)
        {

            agent.SetDestination(points[currentPoint].transform.position);
        }

       else
        {

            if (currentPoint < points.Count()-1)
            {
                currentPoint++;

            }
            else
            {
                currentPoint = 0;
            }
        }
     
    }

    private void ChasePlayer()
    {        
        animator.SetBool("isWalking", false);
        agent.SetDestination(player.position);
        agent.speed = runSpeed;
        animator.SetBool("isRunning", true);

    }

    private void AttackPlayer()
    {
        animator.SetBool("isRunning", false);
        Debug.Log("Attacking");
        // Make sure enemy doesn't move while attacking
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetBool("isPunching", true);
            /* Attack code here
            *
            *
            *
            */
            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        animator.SetBool("isIdle", true);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
