using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnnemy : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public GameObject Player;
    private Vector3 NearPlayer;
    private Vector3 LastNearPlayer;

    public bool isChasing;

    private bool NeedNewPlaceToSpawn;

    public float OffSetX;
    public float OffSetZ;
    public float CoolDown;
    float timer;
    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        Player = GameObject.FindGameObjectWithTag("Player");
        timer = CoolDown;
    }

    void FixedUpdate()
    {
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
                   
                    if (CanSpawn(Player.transform))
                    {
                        //OnDrawGizmos(Color.green);
                        Teleport(Player.transform);
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
    }

    
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == Player)
        {
            if (CanSpawn(Player.transform))
            {
                //OnDrawGizmos(Color.green);
                Teleport(Player.transform);
                NeedNewPlaceToSpawn = false;
                timer = CoolDown;
            }
        }
    }
    public void TakeDamage()
        {
            if (CanSpawn(Player.transform))
            {
                //OnDrawGizmos(Color.green);
                Teleport(Player.transform);
                NeedNewPlaceToSpawn = false;
                timer = CoolDown;
            }
        }

        void Teleport(Transform target)
        {
            transform.position = new Vector3 (NearPlayer.x, target.transform.position.y, NearPlayer.z);
        Vector3 relativePos = target.transform.position - transform.position;
        GetComponentInParent<Transform>().rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
        bool CanSpawn(Transform target)
        {
            NearPlayer = new Vector3(target.transform.position.x + Random.Range(OffSetX * -1, OffSetX), target.transform.position.y +1, target.transform.position.z + Random.Range(OffSetZ * -1, OffSetZ));
            Collider[] hitColliders = Physics.OverlapBox(NearPlayer, transform.localScale /2, Quaternion.identity, m_LayerMask);
            int i = 0;
            //Check when there is a new collider coming into contact with the box
            while (i < hitColliders.Length)
            {
            Debug.Log(hitColliders[i].gameObject.name + i);
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

