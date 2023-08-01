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
    private Rigidbody rb;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerHealth playerHealth;
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
        Debug.Log("Chasing");
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        agent.SetDestination(player.position);
        agent.speed = runSpeed;

    }

    private void AttackPlayer()
    {
        animator.SetBool("isRunning", false);
        
        Debug.Log("Attacking"); 
        // Make sure enemy doesn't move while attacking

        agent.SetDestination(agent.transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Punch");
            Invoke("DealDamage", 0.37f);
            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
        animator.SetBool("isIdle", true);
    }

    private void DealDamage()
    {
        playerHealth.TakeDamage(20);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        Debug.Log("Hej");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
