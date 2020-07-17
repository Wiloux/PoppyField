using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public int nbPlaceX;
    public int nbPlaceY;
    public GameObject slot;
    public float offsetX=0;
    public float offsetY=0;
    public Camera camInventory;
    public Camera camPlayer;
    public LayerMask mask;
    public (int,int) currentCoord = (0, 0);
    public GameObject[,] matriceSlot;

    // Start is called before the first frame update
    void Start()
    {
        matriceSlot = new GameObject[nbPlaceX, nbPlaceY];
        player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < nbPlaceX ; i++)
        {
            offsetX += 0.13f;
            for (int j=0; j<nbPlaceY; j++)
            {
                offsetY += 0.13f;
                GameObject newSlot = Instantiate(slot, new Vector3(inventory.transform.position.x + offsetX, inventory.transform.position.y + offsetY, inventory.transform.position.z), Quaternion.identity, inventory.transform);
                newSlot.GetComponent<Slot>().x = i;
                newSlot.GetComponent<Slot>().y = j;
                matriceSlot[i, j] = newSlot;
            }
            offsetY = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camInventory.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, mask) && Input.GetMouseButtonDown(0))
        {
            currentCoord = (hit.transform.gameObject.GetComponent<Slot>().x, hit.transform.gameObject.GetComponent<Slot>().y);
            Debug.Log(currentCoord);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
            if (isActive)
            {
                Time.timeScale = 0;
                player.GetComponent<vThirdPersonInput>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                camPlayer.gameObject.SetActive(false);
                camInventory.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                player.GetComponent<vThirdPersonInput>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                camPlayer.gameObject.SetActive(true);
                camInventory.gameObject.SetActive(false);
            }
        }
    }

    public void addObject(Vector2 size, GameObject objet)
    {
        for (int i=0; i < nbPlaceX; i++) 
        {
            for(int j=0; j < nbPlaceY; j++)
            {
                if (matriceSlot[i, j].GetComponent<Slot>().isEmpty)
                {
                    for(int k = i; k < size.y + i; k++)
                    {
                        if (!matriceSlot[k, j].GetComponent<Slot>().isEmpty)
                        {
                            break;
                        }
                        for(int l=j; l<size.x + j; l++)
                        {
                            if (!matriceSlot[k, l].GetComponent<Slot>().isEmpty)
                            {
                                break;
                            }
                        }
                        for(int x=i; x < size.x + i; x++)
                        {
                            for(int y=j; y < size.y + j; y++)
                            {
                                matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                            }
                        }
                        objet.transform.position = (matriceSlot[i + (int)size.x-1, j + (int)size.y-1].transform.position + matriceSlot[i,j].transform.position) / 2;
                        Debug.Log(objet.transform.position);
                        objects.Add(objet);
                        return;
                    }
                }
            }
            
            
            //Instantiate(objet, inventory.transform, true);
        }
    }
}
