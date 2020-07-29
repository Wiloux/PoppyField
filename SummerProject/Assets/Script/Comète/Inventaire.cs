using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public GameObject inventory;
    public GameObject babyInventory;
    public List<GameObject> objects = new List<GameObject> { };
    private GameObject player;
    public int nbPlaceX;
    public int nbPlaceY;
    public int nbPlaceXpetit;
    public int nbPlaceYpetit;

    //Variables pour crétation inventaire
    public GameObject slot; 
    public float offsetX=0;
    public float offsetY=0;
    //

    public Camera camInventory;
    public Camera camPlayer;
    public LayerMask mask;
    public (int,int) currentCoord = (0, 0); //Coordonnées de la case de l'inventaire où pointe la souris
    public GameObject currentSlot;
    public GameObject[,] matriceSlot; //Matrice contenant toutes les cases de l'inventaire
    public bool pickingObject;
    public GameObject objectPicked;
    private float mZCoord;
    private Vector3 mOffset;
    public GameObject lastOriginSlot; //Position précédente de l'objet pick
    public bool rotateNow;

    public AudioSource pick;
    public AudioSource drop;

    // Start is called before the first frame update
    void Start()
    {
        //Création et instanciation du grand inventaire 
        matriceSlot = new GameObject[nbPlaceX+nbPlaceXpetit, nbPlaceY+nbPlaceYpetit];
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
                newSlot.name = "Emplacement " + i + j;
            }
            offsetY = 0;
        }
        //Création et instanciation du petit inventaire 
        offsetX += 0.39f;
        for (int i = nbPlaceX; i < nbPlaceX + nbPlaceXpetit; i++)
        {
            offsetX += 0.13f;
            for (int j = nbPlaceY; j < nbPlaceY + nbPlaceYpetit; j++)
            {
                offsetY += 0.13f;
                GameObject newSlot = Instantiate(slot, new Vector3(inventory.transform.position.x + offsetX, inventory.transform.position.y + offsetY, inventory.transform.position.z), Quaternion.identity, babyInventory.transform);
                newSlot.GetComponent<Slot>().x = i;
                newSlot.GetComponent<Slot>().y = j;
                matriceSlot[i, j] = newSlot;
                newSlot.name = "Emplacement " + i + j;
            }
            offsetY = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitObject;
        Ray rayObject = camInventory.ScreenPointToRay(Input.mousePosition);

        //Vérifie si on a cliqué sur un objet de l'inventaire ou non
        if (Physics.Raycast(rayObject, out hitObject, mask) && Input.GetMouseButtonDown(0))
        {
            if (hitObject.transform.gameObject.GetComponent<Slot>() != null)
            {
                currentSlot = hitObject.transform.gameObject;
                objectPicked = currentSlot.GetComponent<Slot>().containedObject;
                if (objectPicked != null)
                {
                    selectObject(objectPicked.GetComponent<PickUp>().size, objectPicked);
                    mZCoord = camInventory.WorldToScreenPoint(objectPicked.transform.position).z;
                    mOffset = objectPicked.transform.position - objectPicked.GetComponent<PickUp>().lastOriginSlot.transform.position;
                    pickingObject = true;
                }
            }
        }

        if (pickingObject)
        {
            objectPicked.transform.position = GetMouseWorldPos() + mOffset;
            try
            {
                currentCoord = (hitObject.transform.gameObject.GetComponent<Slot>().x, hitObject.transform.gameObject.GetComponent<Slot>().y);
            }
            catch
            {
                currentCoord = (-1, -1);
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (objectPicked.GetComponent<PickUp>().isRotated)
                {
                    objectPicked.transform.Rotate(0, 0, -90);
                    objectPicked.GetComponent<PickUp>().isRotated = false;
                }
                else
                {
                    objectPicked.transform.Rotate(0, 0, 90);
                    objectPicked.GetComponent<PickUp>().isRotated = true;
                }
                rotateNow = !rotateNow;
                objectPicked.GetComponent<PickUp>().size = new Vector2(objectPicked.GetComponent<PickUp>().size.y, objectPicked.GetComponent<PickUp>().size.x);
            }
        }

        //Si on pick un objet alors on peut le rotate avec le clic droit
        if(pickingObject && Input.GetMouseButtonUp(0))
        {
            if(Physics.Raycast(rayObject, out hitObject, mask))
            {
                if(hitObject.transform.gameObject.GetComponent<Slot>() != null)
                {
                    currentSlot = hitObject.transform.gameObject;
                }
            }
            if (currentSlot == null)
            {
                mOffset = objectPicked.transform.position - objectPicked.GetComponent<PickUp>().lastOriginSlot.transform.position;
                putObjectDown();
                pickingObject = false;
                objectPicked = null;
            }
            else
            {
                mOffset = objectPicked.transform.position - objectPicked.GetComponent<PickUp>().lastOriginSlot.transform.position;
                putObjectDown();
                pickingObject = false;
                objectPicked = null;
            }
            rotateNow = false;
        }

        //Activer/désactiver l'inventaire
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.SetActive(!inventory.activeSelf);
            if (inventory.activeSelf)
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

        //Enregistrement de l'inventaire sur un fichier txt Json
        if (Input.GetKey("escape"))
        {
            Json_Save_Load.ins.Save();
            //Xml_Manager.ins.saveInventory();
            Application.Quit();
        }

    }

    //Fonction ajoutant un objet au petit inventaire si assez de place
    public void addObject(Vector2 size, GameObject objet)
    {
        bool hasBroken=false;
        Vector2 originCoord;
        for (int i = nbPlaceX; i < nbPlaceX + nbPlaceXpetit; i++)
        {
            for (int j = nbPlaceY; j < nbPlaceY + nbPlaceYpetit; j++)
            {
                if (matriceSlot[i, j].GetComponent<Slot>().isEmpty)
                {
                    originCoord = new Vector2(i, j);

                    for (int k = i; k < size.x + i; k++)
                    {
                        Debug.Log(k);
                        if (k >= nbPlaceX + nbPlaceXpetit)
                        {
                            hasBroken = true;
                            Debug.Log("break 1");
                            break;
                        }
                        for (int l = j; l < size.y + j; l++)
                        {
                            Debug.Log(l);
                            if (l >= nbPlaceY + nbPlaceYpetit)
                            {
                                hasBroken = true;
                                Debug.Log("break 2");
                                break;
                            }
                            else
                            {
                                if (!matriceSlot[k, l].GetComponent<Slot>().isEmpty)
                                {
                                    hasBroken = true;
                                    Debug.Log("break 2");
                                    break; //break tout ?
                                }
                            }
                        }
                        if (hasBroken)
                        {
                            break;
                        }
                    }

                    if (!hasBroken)
                    {
                        for (int x = i; x < size.x + i; x++)
                        {
                            Debug.Log("x : " + x);
                            for (int y = j; y < size.y + j; y++)
                            {
                                Debug.Log("y : " + y);
                                matriceSlot[x, y].GetComponent<Slot>().originalCoord = originCoord;
                                matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                                matriceSlot[x, y].GetComponent<Slot>().containedObject = objet;
                            }
                        }
                        objet.GetComponent<PickUp>().lastOriginSlot = matriceSlot[(int)originCoord.x, (int)originCoord.y].gameObject;
                        objet.transform.position = (matriceSlot[i + (int)size.x - 1, j + (int)size.y - 1].transform.position + matriceSlot[i, j].transform.position) / 2;
                        Debug.Log(objet.transform.position);
                        objects.Add(objet);
                        return;
                    }
                    else
                    {
                        hasBroken = false;
                    }
                }
            }
        }
    }

    //Pick un objet
    public void selectObject(Vector2 size, GameObject objet)
    {
        pick.Play();
        if (currentSlot.GetComponent<Slot>().containedObject != null)
        {
            for(int i= (int)currentSlot.GetComponent<Slot>().originalCoord.x; i<size.x+ currentSlot.GetComponent<Slot>().originalCoord.x; i++)
            {
                for(int j= (int)currentSlot.GetComponent<Slot>().originalCoord.y; j < size.y + currentSlot.GetComponent<Slot>().originalCoord.y; j++)
                {
                    matriceSlot[i, j].GetComponent<Slot>().isEmpty = true;
                    matriceSlot[i, j].GetComponent<Slot>().containedObject = null;
                }
            }
        }
    }

    //Resposer l'objet à sa nouvelle position ou à son ancienne
    public void putObjectDown()
    {
        drop.Play();

        //Ancienne position de l'objet s'il n'y a pas la place ou si on est en dehors de l'inventaire
        for(int i = currentCoord.Item1; i < currentCoord.Item1 + objectPicked.GetComponent<PickUp>().size.x; i++)
        {
            for(int j = currentCoord.Item2; j < currentCoord.Item2 + objectPicked.GetComponent<PickUp>().size.y; j++)
            {
                if(i >= 0)
                {
                    if (i >= nbPlaceX + nbPlaceXpetit || j >= nbPlaceY + nbPlaceYpetit || !matriceSlot[i, j].GetComponent<Slot>().isEmpty)
                    {
                        if (rotateNow)
                        {
                            objectPicked.transform.Rotate(0, 0, -90);
                            objectPicked.GetComponent<PickUp>().isRotated = false;
                            objectPicked.GetComponent<PickUp>().size = new Vector2(objectPicked.GetComponent<PickUp>().size.y, objectPicked.GetComponent<PickUp>().size.x);
                        }
                        Debug.Log("Pas la place");
                        for (int x = (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x; x < objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x + objectPicked.GetComponent<PickUp>().size.x; x++)
                        {
                            for (int y = (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y; y < objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y + objectPicked.GetComponent<PickUp>().size.y; y++)
                            {
                                matriceSlot[x, y].GetComponent<Slot>().originalCoord = objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord;
                                matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                                matriceSlot[x, y].GetComponent<Slot>().containedObject = objectPicked;
                            }
                        }
                        objectPicked.transform.position = (matriceSlot[(int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x + (int)objectPicked.GetComponent<PickUp>().size.x - 1, (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y + (int)objectPicked.GetComponent<PickUp>().size.y - 1].transform.position + matriceSlot[(int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x, (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y].transform.position) / 2;
                        //objectPicked.GetComponent<PickUp>().lastOriginSlot = matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].gameObject;
                        currentSlot = objectPicked.GetComponent<PickUp>().lastOriginSlot;
                        return;
                    }
                }
                else
                {
                    if (rotateNow)
                    {
                        objectPicked.transform.Rotate(0, 0, -90);
                        objectPicked.GetComponent<PickUp>().isRotated = false;
                        objectPicked.GetComponent<PickUp>().size = new Vector2(objectPicked.GetComponent<PickUp>().size.y, objectPicked.GetComponent<PickUp>().size.x);
                    }
                    Debug.Log("En dehors de l'inventaire");
                    for (int x = (int)currentSlot.GetComponent<Slot>().originalCoord.x; x < currentSlot.GetComponent<Slot>().originalCoord.x + objectPicked.GetComponent<PickUp>().size.x; x++)
                    {
                        for (int y = (int)currentSlot.GetComponent<Slot>().originalCoord.y; y < currentSlot.GetComponent<Slot>().originalCoord.y + objectPicked.GetComponent<PickUp>().size.y; y++)
                        {
                            matriceSlot[x, y].GetComponent<Slot>().originalCoord = currentSlot.GetComponent<Slot>().originalCoord;
                            matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                            matriceSlot[x, y].GetComponent<Slot>().containedObject = objectPicked;
                        }
                    }
                    objectPicked.transform.position = (matriceSlot[(int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x + (int)objectPicked.GetComponent<PickUp>().size.x - 1, (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y + (int)objectPicked.GetComponent<PickUp>().size.y - 1].transform.position + matriceSlot[(int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.x, (int)objectPicked.GetComponent<PickUp>().lastOriginSlot.GetComponent<Slot>().originalCoord.y].transform.position) / 2;
                    //objectPicked.GetComponent<PickUp>().lastOriginSlot = matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].gameObject;
                    currentSlot = objectPicked.GetComponent<PickUp>().lastOriginSlot;
                    return;
                }
            }
        }

        //Nouvelle position de l'objet 
        for (int x = currentCoord.Item1; x < currentCoord.Item1 + objectPicked.GetComponent<PickUp>().size.x; x++)
        {
            for (int y = currentCoord.Item2; y < currentCoord.Item2 + objectPicked.GetComponent<PickUp>().size.y; y++)
            {
                matriceSlot[x, y].GetComponent<Slot>().originalCoord = new Vector2 (currentCoord.Item1, currentCoord.Item2);
                matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                matriceSlot[x, y].GetComponent<Slot>().containedObject = objectPicked;
            }
        }
        objectPicked.transform.position = (matriceSlot[currentCoord.Item1 + (int)objectPicked.GetComponent<PickUp>().size.x - 1, currentCoord.Item2 + (int)objectPicked.GetComponent<PickUp>().size.y - 1].transform.position + matriceSlot[currentCoord.Item1, currentCoord.Item2].transform.position) / 2;
        objectPicked.GetComponent<PickUp>().lastOriginSlot = matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].gameObject;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return camInventory.ScreenToWorldPoint(mousePoint);
    }
}
