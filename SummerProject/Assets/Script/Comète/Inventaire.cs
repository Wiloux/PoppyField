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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
