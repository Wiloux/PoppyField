using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAimScript : MonoBehaviour
{
    public Transform HandAim;
    private GameObject p1;
    private GameObject p2;

    private void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player");
        p2 = GameObject.FindGameObjectWithTag("Player2");
    }
    void Update()
    {
        transform.position = new Vector3((HandAim.transform.position.x + p1.transform.position.x + p2.transform.position.x) / 3, HandAim.transform.position.y, (HandAim.transform.position.z + p1.transform.position.z + p2.transform.position.z) / 3);
    }
}
