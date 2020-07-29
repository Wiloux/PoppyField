﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnnemy : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;

    public GameObject MainTarget;
    private GameObject Player;
    private GameObject Player2;

    private Vector3 NearPlayer;
    private Vector3 LastNearPlayer;
    public float MaxSpawnDistance;
    public Transform TeleportationStep;
    private float DistanceWithPlayer;

    public bool isChasing;
    public bool HasP2;

    public Transform[] retreatOptions = new Transform[1];

    private bool NeedNewPlaceToSpawn;

    public float OffSetX;
    public float OffSetZ;
    public float CoolDown;
    float timer;
    private Camera Cam;

    private Animator anim;
    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_Started = true;
        anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Player2 = FindObjectOfType<Player2Script>().gameObject;
        //    MainTarget = Player;
        timer = CoolDown;
    }

    public float AttackRange = 3f;

    void FixedUpdate()
    {
        if (I_Can_See() && !HasP2)
        {
            MainTarget = Player;
            Player2.transform.parent = null;
        }
        else
        {
            MainTarget = Player2;
        }


        if (Vector3.Distance(MainTarget.transform.position, transform.position) < AttackRange)
        {
            isChasing = false;
            Debug.Log("attack");
            StartCoroutine(Attack(2));
        }
        else
        {
            isChasing = true;
        }

        if (isChasing)
        {


            if (timer >= 0 && !NeedNewPlaceToSpawn)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                NeedNewPlaceToSpawn = true;
                if (NeedNewPlaceToSpawn)
                {
                    TeleportationStep.position = CheckIfTooFar(MainTarget.transform);
                    if (CanSpawn(TeleportationStep.transform))
                    {
                        //OnDrawGizmos(Color.green);
                        Teleport(TeleportationStep, MainTarget.transform);
                        NeedNewPlaceToSpawn = false;
                        timer = CoolDown;
                    }
                    else
                    {
                        //OnDrawGizmos(Color.red);
                    }
                }
            }
        }
        else if (HasP2)
        {

            if (timer >= 0 && !NeedNewPlaceToSpawn)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Retreat();
            }
        }
    }

    private Transform targetReatreat;
    void Retreat()
    {
        float MaxDistance = 0;

        //OffSetX = 0;
        //OffSetZ = 0;

        for (int i = 0; i < retreatOptions.Length; i++)
        {
            DistanceWithPlayer = Vector3.Distance(retreatOptions[i].transform.position, Player.transform.position);
            if (DistanceWithPlayer > MaxDistance)
            {
                MaxDistance = DistanceWithPlayer;
                targetReatreat = retreatOptions[i];
                Debug.Log(retreatOptions[i].name);
            }
        }

        TeleportationStep.position = CheckIfTooFar(targetReatreat);
        if (CanSpawn(TeleportationStep))
        {
            Teleport(TeleportationStep, targetReatreat);
            NeedNewPlaceToSpawn = false;
            timer = CoolDown;
        }
    }
    private bool isAttacking;
    private bool isGrabbing;
    public Transform grabDestination;

    private IEnumerator Attack(float attackSpeed)
    {
        //changer l'animation
        isAttacking = false;
        if (MainTarget.gameObject.GetComponent<Player2Script>() != null)
        {
            isGrabbing = true;
            //anim.SetBool("isGrabbing", true);
            yield return new WaitForSeconds(attackSpeed);
            //anim.SetBool("isCarrying", false);
            //mainTarget.position = grabDestination.transform.position;
            Player2.transform.parent = grabDestination.transform;
            Player2.GetComponent<Player2Script>().Player2Nav.isStopped = true;
            HasP2 = true;
            isGrabbing = false;
        }



    }

    Vector3 CheckIfTooFar(Transform target)
    {
        Vector3 DifPos = target.transform.position - transform.position;
        float distance = DifPos.magnitude;

        if (distance <= MaxSpawnDistance)
        {
            return target.position;
        }
        else
        {
            Vector3 Dir = DifPos.normalized;
            DifPos = transform.position + (Dir * MaxSpawnDistance);
            return DifPos;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == MainTarget)
        {
            TeleportationStep.position = CheckIfTooFar(Player.transform);
            if (CanSpawn(TeleportationStep))
            {
                Teleport(TeleportationStep, Player.transform);
                NeedNewPlaceToSpawn = false;
                timer = CoolDown;
            }
            else
            {
                timer = 0;
                NeedNewPlaceToSpawn = true;
            }
        }
    }
    public void TakeDamage()
    {
        if (HasP2)
        {
            HasP2 = false;
            isChasing = true;
            Player2.transform.parent = null;
        }
        MainTarget = Player;

        TeleportationStep.position = CheckIfTooFar(Player.transform);

        if (CanSpawn(TeleportationStep))
        {
            Teleport(TeleportationStep, Player.transform);
            NeedNewPlaceToSpawn = false;
            timer = CoolDown;
        }
        else
        {
            timer = 0;
            NeedNewPlaceToSpawn = true;
        }
    }

    void Teleport(Transform target, Transform ActualAim)
    {
        transform.position = new Vector3(NearPlayer.x, target.transform.position.y, NearPlayer.z);
        Vector3 relativePos = ActualAim.transform.position - transform.position;
        GetComponentInParent<Transform>().rotation = Quaternion.LookRotation(relativePos, Vector3.up);
    }
    bool CanSpawn(Transform target)
    {
        NearPlayer = new Vector3(target.transform.position.x + Random.Range(OffSetX * -1, OffSetX), target.transform.position.y + 1, target.transform.position.z + Random.Range(OffSetZ * -1, OffSetZ));
        Collider[] hitColliders = Physics.OverlapBox(NearPlayer, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            i++;
        }

        if (i == 0 && !Physics.Linecast(NearPlayer, new Vector3(NearPlayer.x, target.transform.position.y, NearPlayer.z), m_LayerMask) && NearPlayer != LastNearPlayer)
        {
            LastNearPlayer = NearPlayer;
            return true;
        }
        else
        {
            return false;
        }

    }

    public LayerMask CamMask;
    bool I_Can_See()
    {

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Cam);
        if (GeometryUtility.TestPlanesAABB(planes, gameObject.GetComponent<Collider>().bounds) && !Physics.Linecast(transform.position, Cam.transform.position, CamMask))
        {
            //!Physics.Linecast(gameObject.transform.position, Player.transform.position))
            return true;
        }
        else
        {
            return false;
        }
    }
    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    //void OnDrawGizmos( Color color)
    //{
    //    Gizmos.color = Color.red;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    if (m_Started)
    //        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //        Gizmos.DrawWireCube(NearPlayer, transform.localScale);
    //}
}
