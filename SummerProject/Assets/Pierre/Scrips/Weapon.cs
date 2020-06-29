using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float range = 100f;
    public int bulletsPerMag = 30;
    public int bulletsLeft;

    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public LineRenderer bulletTrail;

    public float fireRate = 0.1f;
    float fireTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;
        
    }

    private void Fire()
    {
        if (fireTimer < fireRate) return;
       

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit , range))
        {
            Debug.Log(hit.transform.name + " found!");
        }
        muzzleFlash.Play();
        SpawnBulletTrail(hit.point);



        fireTimer = 0.0f;
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, shootPoint.position, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();
        lineR.SetPosition(0, shootPoint.position);
        lineR.SetPosition(1, hitPoint);

        //Destroy(bulletTrailEffect, 1f);
    }
}
