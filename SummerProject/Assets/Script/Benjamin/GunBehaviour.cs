using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Invector.vCharacterController;
public class GunBehaviour : MonoBehaviour
{

    public Camera cam;

    public GunTemplate[] gunArray = new GunTemplate[1];
    GunTemplate currentGun;

    public GameObject[] gunObjects = new GameObject[1];

    public AudioManager audioManager;

    public Image bulletImg;

    public Text bulletCounter;

    bool canShoot = true;

    private Vector3 currentRotation;
    private Vector3 rotation;

    public int cameraZoom;
    public int normalZoom;
    float smooth = 5;

    private bool isZoomed = false;
    vThirdPersonMotor.vMovementSpeed spd;
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
        Debug.Log(GetComponent<PlayerController>().stats.freeSpeed.walkSpeed);
        Debug.Log(GetComponent<PlayerController>().stats.strafeSpeed.walkSpeed);
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(currentGun.gunDamage);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Aim();
        }

        if (isZoomed)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cameraZoom, Time.deltaTime * smooth);
            GetComponent<PlayerController>().stats.isStrafing = true;
            GetComponent<PlayerController>().stats.Sprint(false); 
            GetComponent<PlayerController>().stats.freeSpeed.rotateWithCamera = true;
            GetComponent<PlayerController>().stats.strafeSpeed.rotateWithCamera = true;
         
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalZoom, Time.deltaTime * smooth);
            GetComponent<PlayerController>().stats.isStrafing = false;
            GetComponent<PlayerController>().stats.freeSpeed.rotateWithCamera = false;
            GetComponent<PlayerController>().stats.strafeSpeed.rotateWithCamera = false;
           
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGun(1);
        }
    }

   

    void Shoot(int damage)
    {
        if (canShoot == true) { 
        if (currentGun.nbAmmo > 0 && isZoomed==true)
        {
                
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentGun.range))
            {
                Debug.Log(hit.transform.name);

                EnnemyBehaviour target = hit.transform.GetComponent<EnnemyBehaviour>();
                if (target != null)
                {
                    target.TakeDamage(damage);
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
        Debug.Log("switched to " + gunArray[id].gunName);
        for (int i = 0; i < gunArray.Length; i++)
        {
            if (i != id)
            {
                gunObjects[i].SetActive(false);
            }
        }
        gunObjects[id].SetActive(true);
        currentGun = gunArray[id];
        bulletImg.sprite = currentGun.bulletSprite;
        bulletCounter.text = (currentGun.nbAmmo).ToString();

    }

    void Aim()
    {
        isZoomed = !isZoomed;
        
    }


    private IEnumerator Reload()
    {
        if (currentGun.nbAmmo < currentGun.maxAmmo)
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
