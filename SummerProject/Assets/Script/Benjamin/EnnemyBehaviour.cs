
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnnemyBehaviour : MonoBehaviour
{
    public float health = 50f;
    public bool canMove = false;
    public float speed;
    public NavMeshAgent agent;
    public Transform target;
    public float stunTime;
    public Animator anim;
    bool isMoving = true;
    private void Start()
    {
        agent.speed = speed;
    }
    private void Update()
    {
        if (canMove && isMoving == true) { 
        agent.SetDestination(target.position);
        }
        
        
    }

    public void TakeDamage(float amount)
    {
        if (canMove)
        {
            StartCoroutine(Stun(stunTime));
        }
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

    private IEnumerator Stun(float stunTime)
    {
        agent.speed = 0;
        isMoving = false;
        anim.SetBool("isFalling", true);
        yield return new WaitForSeconds(stunTime);
        isMoving = true;
        anim.SetBool("isFalling", false);
        agent.speed = speed;
    }
}
