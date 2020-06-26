using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_saveSystem : MonoBehaviour
{
    public GameObject panelSave;
    public GameObject panelLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            panelSave.SetActive(!panelSave.activeSelf);
            if (panelSave.activeSelf)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                //Cursor.visible = false;
            }
        }
        if (Input.GetKeyDown("f"))
        {
            panelLoad.SetActive(!panelLoad.activeSelf);
            if (panelLoad.activeSelf)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                //Cursor.visible = false;
            }
        }
    }
}
