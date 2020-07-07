using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnnemy : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public GameObject Player;
    private Vector3 NearPlayer;

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
                    if (CanSpawn())
                    {
                        //OnDrawGizmos(Color.green);
                        Teleport();
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
        public void TakeDamage()
        {
            if (CanSpawn())
            {
                //OnDrawGizmos(Color.green);
                Teleport();
                NeedNewPlaceToSpawn = false;
                timer = CoolDown;
            }
        }

        void Teleport()
        {
            transform.position = new Vector3 (NearPlayer.x, Player.transform.position.y, NearPlayer.z);
        Vector3 relativePos = Player.transform.position - transform.position;
        GetComponentInParent<Transform>().rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
        bool CanSpawn()
        {
            NearPlayer = new Vector3(Player.transform.position.x + Random.Range(OffSetX * -1, OffSetX), Player.transform.position.y +1, Player.transform.position.z + Random.Range(OffSetZ * -1, OffSetZ));
            Collider[] hitColliders = Physics.OverlapBox(NearPlayer, transform.localScale /2, Quaternion.identity, m_LayerMask);
            int i = 0;
            //Check when there is a new collider coming into contact with the box
            while (i < hitColliders.Length)
            {
                //Increase the number of Colliders in the array
                if(hitColliders[i].gameObject != this)
                {

                    i++;
                }
                else { 
                    Debug.Log("fuck you");
                }
            }

            if (i == 0)
            {
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

