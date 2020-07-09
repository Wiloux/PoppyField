using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_saveSystem : MonoBehaviour
{
    private GameObject Player;
    private GameObject Cam;
    public GameObject InGameUI;
    public GameObject panelSave;
    public GameObject panelLoad;

    // Update is called once per frame
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void OnTriggerStay(Collider collision)
    {
        if (Input.GetKeyDown("f") && collision.gameObject.tag == "Player")
        {
            ActivateSaveUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 1;
            Player.GetComponent<vThirdPersonInput>().enabled = true;
            Cam.GetComponent<vThirdPersonCamera>().enabled = true;
        }
    }


    public void ActivateSaveUI()
    {
        panelLoad.SetActive(!panelLoad.activeSelf);
        InGameUI.SetActive(!InGameUI.activeSelf);
        if (panelLoad.activeSelf)
        {
            Time.timeScale = 0;
            Player.GetComponent<vThirdPersonInput>().enabled = false;
           // Cam.GetComponent<vThirdPersonCamera>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Player.GetComponent<vThirdPersonInput>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Cam.GetComponent<vThirdPersonCamera>().enabled = true;
            //     Cursor.visible = false;
        }
    }

    void LoadSystem()
    {
        if (Input.GetKeyDown("tab"))
        {
            panelSave.SetActive(!panelSave.activeSelf);
            if (panelSave.activeSelf)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                this.GetComponent<vThirdPersonInput>().enabled = false;
            }
            else
            {
                Time.timeScale = 1;
                this.GetComponent<vThirdPersonInput>().enabled = true;
                //Cursor.visible = false;
            }
        }
    }
}
