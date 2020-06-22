using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Player2Script : MonoBehaviour
{
    public GameObject Player;
    private NavMeshAgent Player2Nav;
    public TwoBoneIKConstraint rig;

    private Animator anim;
    public enum Player2State { Idle, Follow, Abducted }
    public Player2State CurrentState;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Player2Nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        CurrentState = Player2State.Idle;
        Player2Nav.updateRotation = false;
    }


    void Update()
    {
        switch (CurrentState)
        {
            case Player2State.Idle:
               rig.weight = 0;
                anim.SetFloat("InputMagnitude", 0);
                break;
            case Player2State.Follow:
                Walking();
               rig.weight = 1;
                break;
            case Player2State.Abducted:

                break;
        }

    }


    public Transform HandAim;
  
    void Walking()
    {
        Vector3 turnspeed = Player2Nav.steeringTarget + transform.forward;
        float step = turnspeed.z * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Player.transform.rotation, -step);
        //transform.rotation = Player.transform.rotation;
        Player2Nav.SetDestination(HandAim.position);
        float speed = Player.GetComponent<Animator>().GetFloat("InputMagnitude");
        anim.SetFloat("InputMagnitude", speed);
        //    transform.position = HandAim.transform.position;

    }
}
