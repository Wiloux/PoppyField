using Boo.Lang;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemInSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 size = new Vector2(95f, 95f);
    public Item _item;

    public Vector2 startPos;
    public Vector2 oldPos;
    public GameObject spriteItem;

    Slots _slots;

    private void Start()
    {
        _slots = FindObjectOfType<Slots>();
    }
    private void Update()
    {
        //SpriteItem.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        //Debug.Log("Item : " + SpriteItem.transform.position);
        //Debug.Log("Cam viewPort : " + Camera.main.WorldToViewportPoint(this.transform.position));
        //Debug.Log("Cam screenPoint : " + Camera.main.WorldToScreenPoint(this.transform.position));
        //Debug.Log("slot : " + this.transform.position);
        //Debug.Log("Mouse : " + Input.mousePosition);
        
    }
    
    public void clicked()
    {
        if (_item.usable)
        {
            _item.Use();

            //for (int i = 0; i < _item.itemSize.y; i++)
            //{
            //    for (int j = 0; j < _item.itemSize.x; j++)
            //    {
            //        _slots.grid[(int)startPos.x + j, (int)startPos.y + j] = 0;
            //    }
            //}
            //Destroy(this.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData _eventData)
    {
        Debug.Log("beg");
        oldPos = transform.GetComponent<RectTransform>().anchoredPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData _eventData)
    {
        Debug.Log("on drag");

        transform.position = _eventData.position;

        for (int i = 0; i < _item.itemSize.y; i++)
        {
            for (int j = 0; j < _item.itemSize.x; j++)
            {
                _slots.grid[(int)startPos.x + j, (int)startPos.y + j] = 0;
            }
        }
        
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 _finalPos = GetComponent<RectTransform>().anchoredPosition;

            Vector2 _slotEnd;
            _slotEnd.x = (float)Math.Floor(_finalPos.x / size.x);
            _slotEnd.y = (float)Math.Floor(_finalPos.y / size.y);
            Debug.Log("Slot : " + _slotEnd);

            //if (((int)(_slotEnd.x) + (int)(_item.itemSize.x) - 1) < _slots.maxGridX && ((int)(_slotEnd.y) + (int)(_item.itemSize.y) - 1) < _slots.maxGridY && ((int)(_slotEnd.x)) >= 0 && (int)_slotEnd.y >= 0) 
            //{
            //    List<Vector2> newPosItem = new List<Vector2>();
            //}
        }
        else
        {
            //ref player
            //ref obj
            Debug.Log("Drop Item");
        }
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

   

}
