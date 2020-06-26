using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public Vector3 posToGo;
    public Item _item;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                bool wasPickedUp = false;
                wasPickedUp = Slots.instanceSlot.addInFirstSpace(_item);
                if (wasPickedUp)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("getItem");
            Slots.instanceSlot.addInFirstSpace(_item);
            
        }
    }
}
