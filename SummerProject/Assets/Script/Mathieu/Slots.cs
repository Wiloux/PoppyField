using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    #region Singleton
    public static Slots instanceSlot;

    void Awake()
    {
        if (instanceSlot != null)
        {
            Debug.LogWarning("More than one inventory");
            return;
        }
        instanceSlot = this;
    }
    #endregion

    public int[,] grid;

    public int maxGridX;
    public int maxGridY;

    public Functionnalities inventoryStats;
    List<ItemInSlot> itemsInInventory = new List<ItemInSlot>();

    public ItemInSlot prefab;
    Vector2 cellSize = new Vector2(94f, 94f);
    List<Vector2> newPosItemInBag = new List<Vector2>();
    private void Start()
    {
        inventoryStats = FindObjectOfType<Functionnalities>();
        maxGridX = 14;
        maxGridY = (int)((inventoryStats.numberSlots + 1) / maxGridX);
        grid = new int[maxGridX, maxGridY];
    }
    public bool addInFirstSpace(Item currentItem)
    {
        int contX = (int)currentItem.itemSize.x;
        int contY = (int)currentItem.itemSize.y;

        for (int i = 0; i < maxGridX; i++)
        {
            for (int j = 0; j < maxGridY; j++)
            {
                if (newPosItemInBag.Count != (contX * contY))
                {
                    for (int sizeY = 0; sizeY < contY; sizeY++)
                    {
                        for (int sizeX = 0; sizeX < contX; sizeX++)
                        {
                            if ((i + sizeX) < maxGridX && (j + sizeY) < maxGridY && grid[i + sizeX, j + sizeY] != 1)
                            {
                                Vector2 pos;
                                pos.x = i + sizeX;
                                pos.y = j + sizeY;
                                newPosItemInBag.Add(pos);
                            }else
                            {
                                sizeX = contX;
                                sizeY = contY;
                                newPosItemInBag.Clear();
                            }
                        }
                    }
                }else
                {
                    break;
                }
            }
        }

        if (newPosItemInBag.Count == (contX * contY))
        {
            ItemInSlot myItem = Instantiate(prefab);
            myItem.startPos = new Vector2(newPosItemInBag[0].x, newPosItemInBag[0].y);
            myItem._item = currentItem;
            myItem.spriteItem = currentItem.IconItem;
            myItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //change pos Item
            //myItem.SpriteItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Change pos ItemIcon (3dObj in inventory)
            myItem.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            myItem.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            myItem.transform.SetParent(this.GetComponent<RectTransform>(), false);
            myItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            myItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(myItem.startPos.x * cellSize.x, -myItem.startPos.y * cellSize.y);
            itemsInInventory.Add(myItem);

            for (int i = 0; i < newPosItemInBag.Count; i++)
            {
                int posToAddx = (int)newPosItemInBag[i].x;
                int posToAddy = (int)newPosItemInBag[i].y;
                grid[posToAddx, posToAddy] = 1;
            }
            newPosItemInBag.Clear();
            Debug.Log("NbItemInBag : " + itemsInInventory.Count);
        }
        return false;
    }

}
