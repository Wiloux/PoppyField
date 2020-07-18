﻿using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public GameObject inventory;
    public int nbPlace = 154;
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
    public GameObject currentSlot;
    public GameObject[,] matriceSlot;
    public bool pickingObject;
    public GameObject objectPicked;
    private float mZCoord;
    private Vector3 mOffset;
    public GameObject lastOriginSlot;

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

        if (Physics.Raycast(rayObject, out hitObject, mask) && Input.GetMouseButtonDown(0))
        {
            currentSlot = hitObject.transform.gameObject;
            objectPicked = currentSlot.GetComponent<Slot>().containedObject;
            if (objectPicked != null)
            {
                selectObject(objectPicked.GetComponent<PickUp>().size, objectPicked);
                mZCoord = camInventory.WorldToScreenPoint(objectPicked.transform.position).z;
                mOffset = objectPicked.transform.position - lastOriginSlot.transform.position;
                pickingObject = true;
            }
        }

        if (pickingObject && Physics.Raycast(rayObject, out hitObject, mask))
        {
            currentCoord = (hitObject.transform.gameObject.GetComponent<Slot>().x, hitObject.transform.gameObject.GetComponent<Slot>().y);
            objectPicked.transform.position = GetMouseWorldPos() + mOffset;
            Debug.Log(currentCoord);
        }

        if(pickingObject && Input.GetMouseButtonDown(1))
        {
            mOffset = objectPicked.transform.position - lastOriginSlot.transform.position;
            putObjectDown();
            pickingObject = false;
            objectPicked = null;
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
        Vector2 originCoord;
        for (int i=0; i < nbPlaceX; i++) 
        {
            for(int j=0; j < nbPlaceY; j++)
            {
                if (matriceSlot[i, j].GetComponent<Slot>().isEmpty)
                {
                    originCoord = new Vector2(i, j);
                    for (int k = i; k < size.y + i; k++)
                    {
                        if (!matriceSlot[k, j].GetComponent<Slot>().isEmpty)
                        {
                            break;
                        }
                        for(int l=j; l<size.x + j; l++)
                        {
                            if (!matriceSlot[k, l].GetComponent<Slot>().isEmpty)
                            {
                                break; //break tout ?
                            }
                        }
                        for(int x=i; x < size.x + i; x++)
                        {
                            for(int y=j; y < size.y + j; y++)
                            {
                                matriceSlot[x, y].GetComponent<Slot>().originalCoord = originCoord;
                                matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                                matriceSlot[x, y].GetComponent<Slot>().containedObject = objet;
                            }
                        }
                        lastOriginSlot = matriceSlot[(int)originCoord.x, (int)originCoord.y].gameObject;
                        objet.transform.position = (matriceSlot[i + (int)size.x-1, j + (int)size.y-1].transform.position + matriceSlot[i,j].transform.position) / 2;
                        Debug.Log(objet.transform.position);
                        objects.Add(objet);
                        return;
                    }
                }
            }
        }
    }

    public void selectObject(Vector2 size, GameObject objet)
    {
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

    public void putObjectDown()
    {
        for(int i = currentCoord.Item1; i < currentCoord.Item1 + objectPicked.GetComponent<PickUp>().size.x; i++)
        {
            for(int j = currentCoord.Item2; j < currentCoord.Item2 + objectPicked.GetComponent<PickUp>().size.y; j++)
            {
                if(i>=nbPlaceX || j >= nbPlaceY)
                {
                    for (int x = (int)currentSlot.GetComponent<Slot>().originalCoord.x; x < currentSlot.GetComponent<Slot>().originalCoord.x + objectPicked.GetComponent<PickUp>().size.x; x++)
                    {
                        for (int y = (int)currentSlot.GetComponent<Slot>().originalCoord.y; y < currentSlot.GetComponent<Slot>().originalCoord.y + objectPicked.GetComponent<PickUp>().size.y; y++)
                        {
                            matriceSlot[x, y].GetComponent<Slot>().originalCoord = currentSlot.GetComponent<Slot>().originalCoord;
                            matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                            matriceSlot[x, y].GetComponent<Slot>().containedObject = objectPicked;
                        }
                    }
                    objectPicked.transform.position = (matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x + (int)objectPicked.GetComponent<PickUp>().size.x - 1, (int)currentSlot.GetComponent<Slot>().originalCoord.y + (int)objectPicked.GetComponent<PickUp>().size.y - 1].transform.position + matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].transform.position) / 2;
                    return;
                }
                if (!matriceSlot[i, j].GetComponent<Slot>().isEmpty)
                {
                    for (int x = (int)currentSlot.GetComponent<Slot>().originalCoord.x; x < currentSlot.GetComponent<Slot>().originalCoord.x + objectPicked.GetComponent<PickUp>().size.x; x++)
                    {
                        for (int y = (int)currentSlot.GetComponent<Slot>().originalCoord.y; y < currentSlot.GetComponent<Slot>().originalCoord.y + objectPicked.GetComponent<PickUp>().size.y; y++)
                        {
                            matriceSlot[x, y].GetComponent<Slot>().originalCoord = currentSlot.GetComponent<Slot>().originalCoord;
                            matriceSlot[x, y].GetComponent<Slot>().isEmpty = false;
                            matriceSlot[x, y].GetComponent<Slot>().containedObject = objectPicked;
                        }
                    }
                    objectPicked.transform.position = (matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x + (int)objectPicked.GetComponent<PickUp>().size.x - 1, (int)currentSlot.GetComponent<Slot>().originalCoord.y + (int)objectPicked.GetComponent<PickUp>().size.y - 1].transform.position + matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].transform.position) / 2;
                    return;
                }
            }
        }
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
        lastOriginSlot = matriceSlot[(int)currentSlot.GetComponent<Slot>().originalCoord.x, (int)currentSlot.GetComponent<Slot>().originalCoord.y].gameObject;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return camInventory.ScreenToWorldPoint(mousePoint);
    }
}
