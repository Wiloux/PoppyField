using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator myAnim;
    List<Rigidbody> ragdollRigid;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        ragdollRigid = new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>());
        ragdollRigid.Remove(GetComponent<Rigidbody>());
        DesactivateRadoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateRadoll()
    {
        myAnim.enabled = false;

        for (int i = 0; i < ragdollRigid.Count; i++)
        {

            ragdollRigid[i].useGravity = true;
            ragdollRigid[i].isKinematic = false;
        }
    }
    void DesactivateRadoll()
    {
        myAnim.enabled = false;

        for (int i = 0; i < ragdollRigid.Count; i++)
        {

            ragdollRigid[i].useGravity = false;
            ragdollRigid[i].isKinematic = true;
        }
    }
}
