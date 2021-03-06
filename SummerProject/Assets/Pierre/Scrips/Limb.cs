﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Limb : MonoBehaviour
{
    [SerializeField] Limb[] childLimbs;
    [SerializeField] GameObject limbPrefab;
    [SerializeField] GameObject woundHole;
    [SerializeField] GameObject bloodPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHit()
    {
        if (childLimbs.Length <= 0)
        {
            foreach (Limb limb in childLimbs)
            {
                if (limb != null)
                {
                    limb.GetHit();
                }
            }

            if (woundHole != null)
            {
                woundHole.SetActive(true);
                if(bloodPrefab != null)
                {
                    Instantiate(bloodPrefab, woundHole.transform.position, woundHole.transform.rotation);
                }

            }

            if (limbPrefab != null)
            {
                Instantiate(limbPrefab, transform.position, transform.rotation);
            }

        }
        transform.localScale = Vector3.zero;

        Destroy(this);
    }
}






