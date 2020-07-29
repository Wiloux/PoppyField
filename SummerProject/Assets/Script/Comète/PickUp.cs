using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventaire inventaire;
    public int tailleEmplacement = 1;
    public Vector2 size = new Vector2(1, 1);
    public bool inInventory;
    public GameObject lastOriginSlot;
    public bool isRotated;

    private void Start()
    {
        inventaire = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventaire>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventaire.addObject(size, gameObject);
            inInventory = true;
            //Destroy(gameObject);
        }
    }
}
