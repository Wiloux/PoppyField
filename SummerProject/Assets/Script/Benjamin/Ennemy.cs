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
    public float detectionRadius;
    float detectionOffset;
    public LayerMask blockLoSLayer;
    public bool inLineOfSight;

    [Header("Animations")]
    private Animator anim;
    public Transform grabDestination;

    bool isAttacking;
    private bool isStruggling;
    bool tookDamage;
    bool isGrabbing;
    bool isRetreating;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = runSpeed;
        agent.stoppingDistance = attackRange;
        attackCoolDown = attackSpeed;
        Player = FindObjectOfType<GunBehaviour>().transform;
        Player2 = FindObjectOfType<Player2Script>().transform;
        agent.updateRotation = false;
    }

    private void FixedUpdate()
    {


        float distance = Vector3.Distance(currentTarget.position, transform.position);
        if (isRetreating == false)
        {
            if (distance <= detectionRadius + detectionOffset)
            {
                currentTarget = CheckNearestTarget();
                Debug.Log(currentTarget + "is being targetted");
                agent.SetDestination(currentTarget.position);
                RaycastHit hit;
                if (Physics.Linecast(currentTarget.position, transform.position, out hit, blockLoSLayer))
                {
                    inLineOfSight = false;
                }
                else
                {
                    inLineOfSight = true;
                    FollowTarget();
                }
            }

            //if (currentTarget == null)
            //{
            //    Debug.Log("looking for a target");

            //}
            //else
            //{
                
            //}
        }
        else
        {
            FollowTarget();
        }

    }

    bool HasFallen;
    void FollowTarget()
    {
        Debug.Log(agent.isStopped);
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (attackCoolDown > 0)
        {
            isAttacking = false;
        }
        if (distanceToTarget < attackRange && !HasFallen)
        {
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            agent.isStopped = true;
            Debug.Log("The ennemy is attacking");

            if (isStruggling && !currentTarget.gameObject.GetComponent<PlayerController>().isStruggling)
            {
                StartCoroutine(Fallen(5f));
                isStruggling = false;
                anim.SetBool("isGrabbing", false);

                //A ajouter : Stun L'ennemi
            }
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

                if (currentTarget == Player)
                {

                    isAttacking = true;
                    attackCoolDown = attackSpeed;
                    if (Player.GetComponent<PlayerController>().CStats.Health <= 0 && !Player.GetComponent<PlayerController>().isStruggling)
                    {
                        //Start Struggle
                        isStruggling = true;
                        anim.SetBool("isGrabbing", true);

                        Player.GetComponent<PlayerController>().isStruggling = true;
                        Player.GetComponent<PlayerController>().CStats.StruggleMax *= 1.7f;
                    }
                    else if (Player.GetComponent<PlayerController>().CStats.Health > 0)
                    {
                        //Attack
                        anim.SetTrigger("isAttacking");
                        Player.GetComponent<PlayerController>().CStats.Health -= attackDamage;
                    }
                    else if (Player.GetComponent<PlayerController>().CStats.Health <= 0 && Player.GetComponent<PlayerController>().isStruggling)
                    {
                        //Add to already Struggle
                        isStruggling = true;
                        anim.SetBool("isGrabbing", true);

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
        else if (!HasFallen && distanceToTarget > attackRange)
        {
            if (isAttacking == false)
            {

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                agent.isStopped = false;
                Debug.Log("The ennemy is running towards you");
                //  agent.isStopped = false;
                Vector3 Movement = transform.forward * Time.deltaTime * Vector3.Distance(currentTarget.position, transform.position);
                Movement = Movement.normalized * runSpeed;
                agent.Move(Movement);
                Vector3 _direction = (currentTarget.position - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_direction.x, 0f, _direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 20f);

            }
        }
        else if (HasFallen)
        {

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("fell");
            agent.isStopped = false;
            isAttacking = false;
        }
    }

    IEnumerator Fallen(float dur)
    {
        HasFallen = true;
        anim.SetTrigger("isFalling");
        yield return new WaitForSeconds(dur);
        HasFallen = false;
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
                isRetreating = true;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(detectionOffset, 0, 0), detectionRadius);
    }

}
