using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isEmpty=true;
    public int x;
    public int y;
    public GameObject containedObject;
    public Vector2 originalCoord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEmpty)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }
}
