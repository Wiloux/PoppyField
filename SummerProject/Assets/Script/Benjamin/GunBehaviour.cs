
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GunBehaviour : MonoBehaviour
{

    public GameObject cam;

    public GunTemplate[] gunArray = new GunTemplate[1];
    GunTemplate currentGun;

    public GameObject[] gunObjects = new GameObject[1];

    public AudioManager audioManager;

    public Image bulletImg;

    public Text bulletCounter;

    bool canShoot = true;

    public bool isAiming;

    private Vector3 currentRotation;
    private Vector3 rotation;

    private void Start()
    {
        for (int i = 0; i < gunArray.Length; i++)
        {
            gunArray[i].nbAmmo = gunArray[i].maxAmmo;
        }
        SwitchGun(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(currentGun.gunDamage);
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
        if (currentGun.nbAmmo > 0)
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
