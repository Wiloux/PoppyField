using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public GameObject Player2;

    public float DistanceWithP2;
    public float minDistance = 5f;

    public bool isFollowedByP2;
    public TwoBoneIKConstraint rig;
    public vThirdPersonController stats;
    public PlayerStats CStats;

    private AudioManager AM;
    public List<AudioClip> footstep;
    // Start is called before the first frame update
    void Start()
    {
        AM = FindObjectOfType<AudioManager>();
        Player2 = GameObject.FindGameObjectWithTag("Player2");
        stats = GetComponent<vThirdPersonController>();
        CStats = GetComponent<PlayerStats>();
        StruggleBar.SetActive(false);
    }

    public void foostepsfx()
    {
        int rdm = Random.Range(0, footstep.Count);
        AM.PlaySoundRDMPitch(footstep[rdm], 0.4f, 0.8f, 1.2f);
    }

    public GameObject Pointer;
    private Quaternion PointerQ;
    void RotateToP2()
    {
        if (Player2.GetComponent<Player2Script>().isGettingKiddnaped)
        {
            Pointer.SetActive(true);
            Vector3 direction = transform.position - Player2.transform.position;

           PointerQ = Quaternion.LookRotation(direction);
            PointerQ.z = -PointerQ.y;
            PointerQ.x = 0;
            PointerQ.y = 0;

            Vector3 North = new Vector3(0, 0, GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles.y);
            Pointer.transform.localRotation = PointerQ * Quaternion.Euler(North);
        }
        else
        {
            Pointer.SetActive(false);
        }
    }

    public bool isStruggling;
    private float StrugleState;
   // public float StrugleMax;
    public GameObject StruggleBar;
    public float StrugleTimer;
    private float timer;
    private bool Left = true;
    public void Struggle()
    {
        StruggleBar.SetActive(true);
        stats.Sprint(false);
        stats.canWalk = false;
        stats.FreezeRotation = true;
        StruggleBar.transform.Find("Bar").transform.localScale = new Vector2(StrugleState / CStats.StruggleMax, 1f);
        stats.stopMove = true;
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {

            if (StrugleState > 0)
            {          
                StrugleState -= CStats.StruggleMax * 0.1f * Time.deltaTime; 
                if (StrugleState < 0)
                {
                    StrugleState = 0f;
                }
            }
        }
        if (StrugleState >= CStats.StruggleMax)
        {
            StruggleBar.SetActive(false);
            isStruggling = false;
            stats.canWalk = true;
            stats.FreezeRotation = false;
            stats.stopMove = false;
            StrugleState = 0f;
        }
        else
        {
           
            if (Left)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StrugleState += 0.1f;
                    timer = StrugleTimer;
                    Left = !Left;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    timer = StrugleTimer;
                    StrugleState += 0.1f;
                    Left = !Left;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateToP2();
        if (isStruggling)
        {
            Struggle();
        }
        DistanceWithP2 = Vector3.Distance(transform.position, Player2.transform.position);

        if (isFollowedByP2)
        {
            Outils.SmoothRigWeight(true, rig);
            stats.freeSpeed.rotationSpeed = 6f;
        }
        else
        {
            Outils.SmoothRigWeight(false, rig);
            stats.freeSpeed.rotationSpeed = 12f;
        }

        if (DistanceWithP2 <= minDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (isFollowedByP2)
            {
                isFollowedByP2 = false;
                Player2.GetComponent<Player2Script>().CurrentState = Player2Script.Player2State.Idle;
            }
            else
            {
                isFollowedByP2 = true;
                Player2.GetComponent<Player2Script>().CurrentState = Player2Script.Player2State.Follow;
            }
        }
        else if (DistanceWithP2 >= minDistance * 2f)
        {
            isFollowedByP2 = false;
            Player2.GetComponent<Player2Script>().CurrentState = Player2Script.Player2State.Idle;
        }
    }
}
