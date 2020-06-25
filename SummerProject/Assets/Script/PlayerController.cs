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

    private AudioManager AM;
    public List<AudioClip> footstep;
    // Start is called before the first frame update
    void Start()
    {
        AM = FindObjectOfType<AudioManager>();
        Player2 = GameObject.FindGameObjectWithTag("Player2");
        stats = GetComponent<vThirdPersonController>();
    }

    public void foostepsfx()
    {
        int rdm = Random.Range(0, footstep.Count);
        AM.PlaySoundRDMPitch(footstep[rdm], 0.4f, 0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
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

        if(DistanceWithP2 <= minDistance && Input.GetKeyDown(KeyCode.E))
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
    }
}
