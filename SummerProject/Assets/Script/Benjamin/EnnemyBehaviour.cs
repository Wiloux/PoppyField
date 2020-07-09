
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnnemyBehaviour : MonoBehaviour
{
    public float health = 50f;
    public bool canMove = false;
    public float speed;
    public NavMeshAgent agent;
    

    public Transform mainTarget;
    public Transform[] retreatOptions = new Transform[1];

    public GameObject P1Target;
    private float D1;
    public float maxDistance;
  
    public GameObject P2Target;
    private float D2;

    public float attackSpeed;
    public int damage;
    public GameObject grabDestination;

    public float stunTime;
    public Animator anim;
    bool isMoving = true;

    public bool isChasing = true;
    private bool isAttacking;
    private bool isGrabbing = false;

    private void Start()
    {
        agent.speed = speed;
        P1Target = FindObjectOfType<GunBehaviour>().gameObject;
        P2Target = FindObjectOfType<Player2Script>().gameObject;
    }
    private void Update()
    {
        if (isGrabbing == false) { 
        mainTarget = CheckNearestTarget();
        }

        float distance = Vector3.Distance(transform.position, mainTarget.transform.position);

        if(distance < maxDistance)
        {
            agent.isStopped = true;
            //agent.speed = 0;
  //          Debug.Log("Too close dude");
            if(isAttacking == false && isGrabbing == false) { 
            StartCoroutine(Attack(attackSpeed));
            }
        }

        else
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isCarrying", false);
            anim.SetBool("isGrabbing", false);
//            Debug.Log("Finally some time to breathe");
        }
        if (isChasing)
        {
            //agent.speed = speed;
            if (canMove && isGrabbing == false)
            {
                //agent.isStopped = false;
                agent.SetDestination(mainTarget.position);
            }
        }
        else
        {
            
            //agent.speed = 0;
            isMoving = false;
            //anim.SetBool("isFalling", true);
        }

    }

    private Transform CheckNearestTarget()
    {
        D1 = Vector3.Distance(transform.position, P1Target.transform.position);
        D2 = Vector3.Distance(transform.position, P2Target.transform.position);
        if (D1 <= D2)
        {
            return P1Target.transform;
        }
        else
        {
            return P2Target.transform;
        }
    }
    public void TakeDamage(float amount)
    {
        Debug.Log("L'ennemi prend un dégat");
        
            StartCoroutine(Stun(stunTime));
        
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void Retreat()
    {
        float distance = 0;
        Transform target;
        for(int i =0; i<retreatOptions.Length; i++)
        {
            D1 = Vector3.Distance(retreatOptions[i].transform.position, P1Target.transform.position);
            if(D1 > distance)
            {
                distance = D1;
                target = retreatOptions[i];
                agent.SetDestination(target.position);
            } 
        }
        
    }

    private IEnumerator Stun(float stunTime)
    {
        agent.isStopped = true;
        isMoving = false;
        anim.SetBool("isFalling", true);
        StopCoroutine(Attack(attackSpeed));
        yield return new WaitForSeconds(stunTime);
        isMoving = true;
        anim.SetBool("isFalling", false);
        agent.isStopped = false;
    }

    private IEnumerator Attack(float attackSpeed)
    {
        //changer l'animation
        isAttacking = false;

        yield return new WaitForSeconds(attackSpeed);
        if(mainTarget.gameObject.GetComponent<GunBehaviour>() != null){
            anim.SetBool("isAttacking", true);
            mainTarget.gameObject.GetComponent<GunBehaviour>().TakeDamage(damage);
            Debug.Log("This ennemy is attacking");
            isAttacking = true;
        }
        else if (mainTarget.gameObject.GetComponent<Player2Script>() != null)
        {
            isGrabbing = true;
            //mainTarget.position = grabDestination.transform.position;
            P2Target.transform.parent = grabDestination.transform;
            P2Target.GetComponent<Player2Script>().Player2Nav.isStopped = true;
            Debug.Log("This ennemy is grabbing lil' sis");
            Retreat();
        }
        
        
        
    }
}
