﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventaire inventaire;
    public int tailleEmplacement = 1;
    public Vector2 size = new Vector2(1, 1);
    private Vector3 mOffset;
    private float mZCoord;
    public bool inInventory;
    public GameObject lastOriginSlot;
    public bool isRotated;

    private void Start()
    {
        inventaire = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventaire>();
    }

    private void Update()
    {

    }

    //private void OnMouseDown()
    //{
    //    if (inventaire.isActive && inInventory)
    //    {
    //        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //        mOffset = gameObject.transform.position - GetMouseWorldPos();
    //    }
    //}

    //private void OnMouseDrag()
    //{
    //    if (inventaire.isActive && inInventory)
    //    {
    //        transform.position = GetMouseWorldPos() + mOffset;
    //    } 
    //}

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
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
