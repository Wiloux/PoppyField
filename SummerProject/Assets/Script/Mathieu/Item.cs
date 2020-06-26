using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

[CreateAssetMenu(fileName = "New Item", menuName = "Iventory/Item")]
public class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    public GameObject IconItem;
    public bool usable;
    public int currentStackSize;
    public int MaxStackSize;
    public Vector2 itemSize;

    public virtual void Use()
    {
        Debug.Log("using");
    }
}
