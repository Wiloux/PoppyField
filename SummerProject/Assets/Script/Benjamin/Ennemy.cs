using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float runSpeed;
    public float attackSpeed;
    float attackCoolDown;
    public int attackDamage;

    [Header("NavMesh")]
    public NavMeshAgent agent;
    public float attackRange;
    public Transform currentTarget;

    [Header("Animations")]
    public Animator anim;

    bool isAttacking;

    private void Start()
    {
        agent.speed = runSpeed;
        agent.stoppingDistance = attackRange;
        attackCoolDown = attackSpeed;
    }

    private void Update()
    {
        agent.SetDestination(currentTarget.position);
        if (currentTarget == null)
        {
            //Put detection here
        }
        else {
            FollowTarget();
        }

    }

    void FollowTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        if(distanceToTarget < attackRange)
        {
            isAttacking = true;
            Debug.Log("The ennemy is attacking");
            agent.isStopped = true;
            anim.SetBool("isAttacking", true);

            if (attackCoolDown > 0)
            {
                attackCoolDown -= Time.deltaTime;
            }
            else
            {
                attackCoolDown = attackSpeed;
                Debug.Log("this ennemy is attacking");
                currentTarget.gameObject.GetComponent<GunBehaviour>().TakeDamage(attackDamage);
            }
        }
        else
        {
            while(isAttacking == false) { 
            Debug.Log("The ennemy is running towards you");
            agent.isStopped = false;
            anim.SetBool("isAttacking", false);
            }
        }
    }

    public void TakeDamage(int damageReceived)
    {
        health -= damageReceived;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("This ennemy is dead");
    }


}
