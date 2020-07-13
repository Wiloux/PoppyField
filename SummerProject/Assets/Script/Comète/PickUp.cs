using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventaire inventaire;
    public int tailleEmplacement = 1;

    private void Start()
    {
        inventaire = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventaire>();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if(Physics.Raycast(ray, out hit, 100.0f))
        //    {
        //        inventaire.addObject(tailleEmplacement, transform.gameObject);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventaire.addObject(tailleEmplacement, gameObject);
            //Destroy(gameObject);
        }
    }
}
