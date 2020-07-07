using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Player2Script : MonoBehaviour
{
    public GameObject Player;
    public NavMeshAgent Player2Nav;
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
               Outils.SmoothRigWeight(false, rig);
                anim.SetFloat("InputMagnitude", 0);
                break;
            case Player2State.Follow:
                Walking();
                Outils.SmoothRigWeight(true, rig);
                break;
            case Player2State.Abducted:

                break;
        }
        
    }

    public Transform HandAim;
    private float DistanceWithP1;
  
    
    void Walking()
    {
        Vector3 turnspeed = Player2Nav.steeringTarget + transform.forward;

        DistanceWithP1 = Vector3.Distance(HandAim.position, transform.position);
   //     float step = 500f* Time.deltaTime;

        if (DistanceWithP1 > 0.4f)
        {

            Debug.Log("needtomove");
            //Movement
            Vector3 Movement = transform.forward * Time.deltaTime * DistanceWithP1 * 2f;
            Player2Nav.Move(Movement);

            //Direction
            Vector3 _direction = (HandAim.position - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_direction.x, 0f, _direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 20f);
        }
        
    
        //transform.rotation = Player.transform.rotation;
     //   Player2Nav.SetDestination(HandAim.position);
        float speed = Player.GetComponent<Animator>().GetFloat("InputMagnitude");
        anim.SetFloat("InputMagnitude", speed);
        //    transform.position = HandAim.transform.position;

    }
}
