﻿
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnnemyBehaviour : MonoBehaviour
{
    public float health = 50f;
    public bool canMove = false;
    public float speed;
    public NavMeshAgent agent;


    public Transform Maintarget;

    public Transform P1Target;
    private float D1;
  
    public Transform P2Target;
    private float D2;


    public float stunTime;
    public Animator anim;
    bool isMoving = true;

    public bool isChasing = true;

    List<Rigidbody> ragdollRigid;
    private void Start()
    {
        agent.speed = speed;
        P1Target = FindObjectOfType<GunBehaviour>().transform;
        P2Target = FindObjectOfType<Player2Script>().transform;
        ragdollRigid = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        ragdollRigid.Remove(GetComponent<Rigidbody>());
    }
    private void Update()
    {
        Maintarget = CheckNearestTarget();

        if (isChasing)
        {
            agent.speed = speed;
            if (canMove && isMoving == true)
            {

                agent.SetDestination(Maintarget.position);
            }
        }
        else
        {
            agent.speed = 0;
            isMoving = false;
            anim.SetBool("isFalling", true);
        }
        
    }

    private Transform CheckNearestTarget()
    {
        D1 = Vector3.Distance(transform.position, P1Target.transform.position);
        D2 = Vector3.Distance(transform.position, P2Target.transform.position);
        if (D1 <= D2)
        {
            return P1Target;
        }
        else
        {
            return P2Target;
        }
    }
    public void TakeDamage(float amount)
    {
        Debug.Log("L'ennemi prend un dégat");
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
        ActivateRadoll();
        agent.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
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

    void ActivateRadoll()
    {
        anim.enabled = false;

        for (int i = 0; i < ragdollRigid.Count; i++)
        {

            ragdollRigid[i].useGravity = true;
            ragdollRigid[i].isKinematic = false;
        }
    }
}
