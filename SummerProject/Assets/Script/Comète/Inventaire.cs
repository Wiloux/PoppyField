using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public GameObject inventory;
    public int nbPlace = 154;
    private int nbPlacesOccupees = 0;
    public GameObject[] slots;
    public List<GameObject> objects = new List<GameObject> { };
    public bool isActive;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            isActive = !isActive;
            if (isActive)
            {
                Time.timeScale = 0;
                player.GetComponent<vThirdPersonInput>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                player.GetComponent<vThirdPersonInput>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void addObject(int tailleObjet, GameObject objet)
    {
        if (nbPlacesOccupees + tailleObjet < nbPlace)
        {
            objet.transform.position = inventory.transform.position;
            objects.Add(objet);
            nbPlacesOccupees += tailleObjet;
            //Instantiate(objet, inventory.transform, true);
        }
        else
        {
            Debug.Log("Inventaire plein");
        }
    }
}
