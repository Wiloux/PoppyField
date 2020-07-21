using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.HighDefinition;

public class GunBehaviour : MonoBehaviour
{

    public Camera cam;
    public int life;

    public GunTemplate[] gunArray = new GunTemplate[1];
    GunTemplate currentGun;

    public GameObject[] gunObjects = new GameObject[1];
    public Transform gunTarget;

    public AudioManager audioManager;

    public Image bulletImg;
    public Image crossHairImg;

    public Text bulletCounter;

    bool canShoot = true;

    private Vector3 currentRotation;
    private Vector3 rotation;

    public int cameraZoom;
    public int normalZoom;

    private MeleeSystem MeleeSys;
    int gunID;
    float smooth = 5;

    private bool isAiming = false;

    public List<handPlacement> HandPlacements;

    private Transform GunTip;
    public GameObject Muzzle;
    public LineRenderer bulletTrail;
    private void Start()
    {
        for (int i = 0; i < gunArray.Length; i++)
        {
            gunArray[i].nbAmmo = gunArray[i].maxAmmo;
        }
        //    spd.walkSpeed = 0f;
        SwitchGun(0);
    }

    private RaycastHit hit;

    Vector3 shootDirection;

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerController>().isStruggling)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!currentGun.isMelee)
                {
                    Shoot(currentGun.gunDamage);
                }
                else
                {
                    if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Melee"))
                    {
                        Debug.Log("meleeee");
                        MeleeHit();
                    }
                }
            }
            if (Input.GetButtonDown("Fire2") && !GetComponent<PlayerController>().isFollowedByP2 && !currentGun.isMelee)
            {
                Aim();
            }
        } else
        {
            if (isAiming == true)
            {
                Aim();
            }
      
        }

        if (life <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("You are dead");
            //lancer le GameOver;
        }

        if (isAiming)
        {
            shootDirection = cam.transform.forward;
            SmartCrosshair();

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cameraZoom, Time.deltaTime * smooth);
            GetComponent<PlayerController>().stats.isStrafing = true;
            GetComponent<PlayerController>().stats.Sprint(false);
            GetComponent<Animator>().SetBool("isAiming", true);
            GetComponent<Animator>().SetLayerWeight(2, 0);


        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalZoom, Time.deltaTime * smooth);
            GetComponent<PlayerController>().stats.isStrafing = false;
            GetComponent<Animator>().SetBool("isAiming", false);
            GetComponent<Animator>().SetLayerWeight(2, 1);
        }

        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Melee"))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchGun(0);
                gunID = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchGun(1);
                gunID = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchGun(2);
                gunID = 2;
            }
        }

        if (currentGun.isMelee)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Melee"))
            {
                GetComponent<Animator>().SetLayerWeight(2, 0);
                MeleeSys.isAttacking = true;
                GetComponent<PlayerController>().stats.Sprint(false);
                GetComponent<PlayerController>().stats.canWalk = false;
                GetComponent<PlayerController>().stats.FreezeRotation = true;
                GetComponent<PlayerController>().stats.stopMove = true;
                // GetComponent<PlayerController>().stats.isStrafing = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                GetComponent<PlayerController>().stats.stopMove = false;

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                MeleeSys.isAttacking = false;
                GetComponent<PlayerController>().stats.canWalk = true;
                GetComponent<PlayerController>().stats.FreezeRotation = false;
                GetComponent<Animator>().SetLayerWeight(2, 1);
                // GetComponent<PlayerController>().stats.isStrafing = false;
            }
        }

        Vector3 direction = gunTarget.position - gunObjects[gunID].transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        if (isAiming)
        {
            gunObjects[gunID].transform.rotation = rotation;
        }


    }


    public GameObject Impact;

    public void TakeDamage(int damage)
    {
        life -= damage;
    }

    void MeleeHit()
    {
        GetComponent<Animator>().SetTrigger("isMelee");

    }

    void SmartCrosshair()
    {
        if (!currentGun.isMelee)
        {
            if (Physics.Raycast(GunTip.transform.position, shootDirection, out hit, currentGun.range))
            {
                crossHairImg.rectTransform.position = Camera.main.WorldToScreenPoint(hit.point);

                if (hit.transform.GetComponent<EnnemyBehaviour>() != null || hit.transform.GetComponent<TeleportEnnemy>() != null)
                {
                    crossHairImg.color = new Color(255, 0, 0, 255);
                }
                else if (hit.transform.GetComponent<Player2Script>())
                {
                    crossHairImg.color = new Color(0, 255, 0, 255);
                }
                else
                {
                    crossHairImg.color = new Color(0, 0, 0, 255);
                }
            }
            else
            {
                crossHairImg.color = new Color(155, 155, 155, 255);
            }
        }
    }

    void Shoot(int damage)
    {
        if (canShoot == true)
        {
            if (currentGun.nbAmmo > 0 && isAiming == true)
            {
                //Muzzle Flash

                GameObject _Muzzle = Instantiate(Muzzle, GunTip.position, GunTip.rotation);
                Destroy(_Muzzle, 0.1f);

                for (int i = 0; i < currentGun.numberOfBullets; i++)
                {
                    if (Physics.Raycast(GunTip.transform.position, shootDirection, out hit, currentGun.range))
                    {
                        //Change Crosshair pos
                        crossHairImg.rectTransform.position = Camera.main.WorldToScreenPoint(hit.point);

                        //Gun Spread
                        if (currentGun.useSpread == true)
                        {
                            shootDirection.x += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                            shootDirection.y += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                        }


                        //Trail Effect
                        SpawnBulletTrail(hit.point, GunTip.position);

                        //Dmg Calculation/ OnHitEffect

                        GameObject target = hit.transform.gameObject;
                        if (target.GetComponent<EnnemyBehaviour>() != null)
                        {
                            target.GetComponent<EnnemyBehaviour>().TakeDamage(damage);
                        }
                        else if (target.GetComponent<TeleportEnnemy>() != null)
                        {
                            target.GetComponent<TeleportEnnemy>().TakeDamage();
                        }
                        else if (target.GetComponent<ImpactOnProps>() != null)
                        {
                            GameObject NewImpact = Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
                            NewImpact.GetComponent<DecalProjector>().size += new Vector3(0, 0, 0.1f);
                            Destroy(NewImpact, 4f);

                        }

                    }
                    else
                    {

                        if (currentGun.useSpread == true)
                        {
                            shootDirection.x += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                            shootDirection.y += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                        }

                        Vector3 end = GunTip.position + shootDirection * currentGun.range;
                        SpawnBulletTrail(end, GunTip.position);
                    }

                }

                audioManager.PlaySound(currentGun.shootSound, currentGun.shootVolume);
                currentGun.nbAmmo--;
                bulletCounter.text = (currentGun.nbAmmo).ToString();
                StartCoroutine(Recovery(currentGun.recoveryTime));
            }
        }
    }


    void SwitchGun(int id)
    {
        crossHairImg.gameObject.SetActive(false);
        rig.weight = 0;
        Debug.Log("switched to " + gunArray[id].gunName);
        for (int i = 0; i < gunArray.Length; i++)
        {
            if (i != id)
            {
                gunObjects[i].SetActive(false);
            }
            else
            {
                gunObjects[i].SetActive(true);
            }
        }

        if (isAiming)
        {
            isAiming = false;
        }
        currentGun = gunArray[id];
        bulletImg.sprite = currentGun.bulletSprite;
        crossHairImg.sprite = currentGun.crossHairSprite;

        //Find HandPlacements 
        HandPlacements[0].HandAim = GameObject.Find(gunObjects[id].name + "/LeftHandPlacement").transform;
        HandPlacements[1].HandAim = GameObject.Find(gunObjects[id].name + "/RightHandPlacement").transform;
        Debug.Log(gunObjects[id].name + "/LeftHandPlacement");

        Debug.Log(gunObjects[id].name);

        if (!currentGun.isMelee)
        {
            bulletCounter.text = (currentGun.nbAmmo).ToString();
            GunTip = GameObject.Find(gunObjects[id].name + "/GunTip").transform;
        }
        else
        {
            MeleeSys = gunObjects[id].GetComponent<MeleeSystem>();
            MeleeSys.Dmg = currentGun.gunDamage;
            MeleeSys.Behaviour = this;
            bulletCounter.text = null;
        }
    }

    public Rig rig;
    void Aim()
    {
        isAiming = !isAiming;
        if (isAiming == true)
        {
            crossHairImg.gameObject.SetActive(true);
            rig.weight = 1;
            //  gunObjects[gunID].SetActive(true);
        }
        else
        {
            crossHairImg.gameObject.SetActive(false);
            rig.weight = 0;

            //   gunObjects[gunID].SetActive(false);
        }
        //    GetComponent<PlayerController>().stats.SetControllerMoveSpeed(0.0f);
    }

    private bool isReloading;
    private IEnumerator Reload()
    {
        if (currentGun.nbAmmo < currentGun.maxAmmo && canShoot && !isReloading)
        {
            isReloading = true;
            audioManager.PlaySound(currentGun.reloadSound, currentGun.reloadVolume);
            yield return new WaitForSeconds(currentGun.reloadTime);
            isReloading = false;
            currentGun.nbAmmo = currentGun.maxAmmo;
            bulletCounter.text = (currentGun.nbAmmo).ToString();
        }
    }

    private IEnumerator Recovery(float waitTime)
    {
        canShoot = false;
        yield return new WaitForSeconds(currentGun.recoveryTime);
        canShoot = true;
    }
    private void SpawnBulletTrail(Vector3 hitPoint, Vector3 startposition)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, startposition, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();
        lineR.SetPosition(0, startposition);
        lineR.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 0.1f);
    }
}
