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
    private NavMeshAgent agent;
    public float attackRange;
    public Transform currentTarget;
    public Transform Player;
    public Transform Player2;

    [Header("Animations")]
    private Animator anim;

    bool isAttacking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = runSpeed;
        agent.stoppingDistance = attackRange;
        attackCoolDown = attackSpeed;
        Player = FindObjectOfType<GunBehaviour>().transform;
        Player2 = FindObjectOfType<Player2Script>().transform;
        
    }

    private void Update()
    {
        agent.SetDestination(currentTarget.position);
        if (currentTarget == null)
        {
            //Put detection here
        }
        else
        {
            FollowTarget();
        }

    }
    private bool isStruggling;
    void FollowTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (attackCoolDown > 0)
        {
            isAttacking = false;
        }
        if (distanceToTarget < attackRange)
        {

            Debug.Log("The ennemy is attacking");
            agent.isStopped = true;

            if (isStruggling && !currentTarget.gameObject.GetComponent<PlayerController>().isStruggling)
            {
                isStruggling = false;
                //A ajouter : Stun L'ennemi
            }
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isStruggling", isStruggling);
            if (attackCoolDown > 0)
            {
                isAttacking = false;
            
                attackCoolDown -= Time.deltaTime;
            }
            else
            {
                if (currentTarget == Player)
                {
                    isAttacking = true;
                    attackCoolDown = attackSpeed;
                    if (Player.GetComponent<PlayerController>().CStats.Health <= 0 && !Player.GetComponent<PlayerController>().isStruggling)
                    {
                        //Start Struggle

                        isStruggling = true;
                        Player.GetComponent<PlayerController>().isStruggling = true;
                        Player.GetComponent<PlayerController>().CStats.StruggleMax *= 1.7f;
                    }
                    else if (Player.GetComponent<PlayerController>().CStats.Health > 0)
                    {
                        //Attack
                        Player.GetComponent<PlayerController>().CStats.Health -= attackDamage;
                    }
                    else if (Player.GetComponent<PlayerController>().CStats.Health <= 0 && Player.GetComponent<PlayerController>().isStruggling)
                    {
                        //Add to already Struggle
                        isStruggling = true;
                 //       Player.GetComponent<PlayerController>().isStruggling = true;
                    }
                }
                else if (currentTarget == Player2)
                {
                    isAttacking = true;
                    Player2.GetComponent<Player2Script>().isGettingKiddnaped = true;
                    //A ajouter : le reste du kidnapping xD
                }
            }
        }
        else
        {
            if (isAttacking == false)
            Debug.Log("The ennemy is running towards you");
            agent.isStopped = false;
            anim.SetBool("isAttacking", false);
        }
    }


    public void TakeDamage(int damageReceived)
    {
        Debug.Log("keres?");
        health -= damageReceived;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("This ennemy is dead");
        Destroy(gameObject);
    }


}
