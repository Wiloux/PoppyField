using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlots : MonoBehaviour
{
    Functionnalities Inventory;
    [SerializeField]
    GameObject slotPrefab;
    public int gridSize = 0;

    void Start()
    {
        Inventory = Functionnalities._instance;
        gridSize = Inventory.numberSlots;
        for (int i = 0; i < Inventory.numberSlots; i++)
        {
            var itemUI = Instantiate(slotPrefab, transform);
        }
    }

    public void CalculSizeInventory()
    {
        if (Inventory.numberSlots >= 29)
        {
            if (gridSize > Inventory.numberSlots)
            {
                for (int j = gridSize; j > gridSize - (gridSize - Inventory.numberSlots); j--)
                {
                    Debug.Log("destroy");
                    Destroy(transform.GetChild(j).gameObject);
                }

            }
            else if (gridSize < Inventory.numberSlots)
            {
                for (int k = 0; k < Inventory.numberSlots - gridSize; k++)
                {
                    Debug.Log("add");
                    var itemUI = Instantiate(slotPrefab, transform); //generate the slots grid.
                }

            }
            Debug.Log("size : G : " + gridSize + " slootReq : " + Inventory.numberSlots);
            gridSize = Inventory.numberSlots;
        }
        
    }
}
