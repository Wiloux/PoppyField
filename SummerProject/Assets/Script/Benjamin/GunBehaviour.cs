using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.HighDefinition;

public class GunBehaviour : MonoBehaviour
{

    public Camera cam;

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
    int gunID;
    float smooth = 5;

    private bool isAiming = false;

    private Transform GunTip;
    public GameObject Muzzle;
    private void Start()
    {
        for (int i = 0; i < gunArray.Length; i++)
        {
            gunArray[i].nbAmmo = gunArray[i].maxAmmo;
        }
        //    spd.walkSpeed = 0f;
        SwitchGun(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(currentGun.gunDamage);
        }
        if (Input.GetButtonDown("Fire2") && !GetComponent<PlayerController>().isFollowedByP2)
        {
            Aim();
        }

        if (isAiming)
        {
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

        Vector3 direction = gunTarget.position - gunObjects[gunID].transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        if (isAiming) { 
        gunObjects[gunID].transform.rotation = rotation;
        }
    }


    public GameObject Impact;

    void Shoot(int damage)
    {
        if (canShoot == true)
        {
            if (currentGun.nbAmmo > 0 && isAiming == true)
            {
                Vector3 shootDirection = cam.transform.forward;
                
                RaycastHit hit;
                GameObject _Muzzle = Instantiate(Muzzle, GunTip.position, GunTip.rotation);
                Destroy(_Muzzle, 0.1f);
                for (int i =0; i < currentGun.numberOfBullets; i++) { 
                    if (Physics.Raycast(GunTip.transform.position, shootDirection, out hit, currentGun.range))
                    {
                        if (currentGun.useSpread == true)
                        {
                            shootDirection.x += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                            shootDirection.y += Random.Range(-currentGun.spreadFactor, currentGun.spreadFactor);
                        }
                   
                        Debug.Log(hit.transform.name);

                        GameObject target = hit.transform.gameObject;
                        if (target.GetComponent<EnnemyBehaviour>() != null)
                        {
                            target.GetComponent<EnnemyBehaviour>().TakeDamage(damage);
                        } else if (target.GetComponent<ImpactOnProps>() != null)
                        {
                        GameObject NewImpact =  Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
                            NewImpact.GetComponent<DecalProjector>().size += new Vector3(0,0,0.1f);
                        //    NewImpact.GetComponent<SpriteRenderer>().sprite = target.GetComponent<ImpactOnProps>().ImpactSprite;
                        Destroy(NewImpact, 4f);
                        }

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
        bulletCounter.text = (currentGun.nbAmmo).ToString();
        Debug.Log(gunObjects[id].name);
        GunTip = GameObject.Find(gunObjects[id].name + "/GunTip").transform;
    }

    public Rig rig;
    void Aim()
    {
        isAiming = !isAiming;
        if (isAiming == true)
        {
            rig.weight = 1;
          //  gunObjects[gunID].SetActive(true);
        }
        else
        {
            rig.weight = 0;
         //   gunObjects[gunID].SetActive(false);
        }
        //    GetComponent<PlayerController>().stats.SetControllerMoveSpeed(0.0f);
    }


    private IEnumerator Reload()
    {
        if (currentGun.nbAmmo < currentGun.maxAmmo && canShoot == true)
        {
            Debug.Log("Currently reloading");
            audioManager.PlaySound(currentGun.reloadSound, currentGun.reloadVolume);
            yield return new WaitForSeconds(currentGun.reloadTime);
            Debug.Log("Done reloading");
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
}
