using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private Animator anim;

    // Destroy upon death
    [SerializeField] private GameObject enemy;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0) Death();
    }

    private void Death()
    {
        Destroy(GetComponent<EnemyAI>());
        GetComponent<NavMeshAgent>().isStopped = true;

        anim.SetTrigger("Die");
        
        Destroy(enemy, 6);
    }
}
