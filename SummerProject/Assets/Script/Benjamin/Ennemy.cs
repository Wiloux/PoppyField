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
    float distanceToP1;
    public Transform Player2;
    float distanceToP2;
    public Transform[] retreatOptions = new Transform[1];

    [Header("Animations")]
    private Animator anim;
    public Transform grabDestination;

    bool isAttacking;
    private bool isStruggling;
    bool tookDamage;
    bool isGrabbing;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = runSpeed;
        agent.stoppingDistance = attackRange;
        attackCoolDown = attackSpeed;
        Player = FindObjectOfType<GunBehaviour>().transform;
        Player2 = FindObjectOfType<Player2Script>().transform;
        currentTarget = CheckNearestTarget();

    }

    private void Update()
    {
        agent.SetDestination(currentTarget.position);
        if (currentTarget == null)
        {
            Debug.Log("looking for a target");
            currentTarget = CheckNearestTarget();
        }
        else
        {
            FollowTarget();
        }

    }
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
                if (tookDamage)
                {
                    attackCoolDown = attackSpeed;
                    tookDamage = false;
                }
            }
            else
            {
                Debug.Log("Damage landed");
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
                    isGrabbing = true;
                    anim.SetBool("isGrabbing", true);
                    anim.SetBool("isCarrying", false);
                    currentTarget.position = grabDestination.transform.position;
                    Player2.transform.parent = grabDestination.transform;
                    Player2.GetComponent<Player2Script>().Player2Nav.isStopped = true;
                    Debug.Log("This ennemy is grabbing lil' sis");
                    Retreat();
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
        tookDamage = true;
        Debug.Log("The ennemy is taking damage");
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

    private Transform CheckNearestTarget()
    {
        distanceToP1 = Vector3.Distance(transform.position, Player.transform.position);
        distanceToP2 = Vector3.Distance(transform.position, Player2.transform.position);
        if (distanceToP1 <= distanceToP2)
        {
            return Player.transform;
        }
        else
        {
            return Player2.transform;
        }
    }

    void Retreat()
    {
        float distance = 0;
        Transform target;
        for (int i = 0; i < retreatOptions.Length; i++)
        {
            distanceToP1 = Vector3.Distance(retreatOptions[i].transform.position, Player.transform.position);
            if (distanceToP1 > distance)
            {
                distance = distanceToP1;
                target = retreatOptions[i];
                agent.SetDestination(target.position);
            }
        }

    }

}
