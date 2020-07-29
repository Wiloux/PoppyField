using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Save_system : MonoBehaviour
{
    private int sceneIndex;
    private int sceneToLoad;
    private string numSave;
    public static Vector3 playerLocation;
    public GameObject player;
    public static bool hasSaved;
    private float timerPersonInput = 0.2f;
    private float timer;
    public static string currentSave = "1";

    private void Start()
    {
        //Le timer permet de résoudre un bug avec le script vThirdPersonInput
        timer = timerPersonInput;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<vThirdPersonInput>().enabled = false;
        if (hasSaved)
        {
            loadNewStats();
        }
    }
    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            player.GetComponent<vThirdPersonInput>().enabled = true;
        }
    }
    public void SaveGame()
    {
        Debug.Log("SAVE");
        numSave = transform.parent.name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene"+numSave, sceneIndex+1);
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        PlayerPrefs.SetFloat("Xlocation" + numSave, playerLocation.x);
        PlayerPrefs.SetFloat("Ylocation" + numSave, playerLocation.y);
        PlayerPrefs.SetFloat("Zlocation" + numSave, playerLocation.z);
        DateTime date = DateTime.Now;
        PlayerPrefs.SetString("DateLastSave" + numSave, date.ToString());
        PlayerPrefs.SetInt("NbSave" + numSave, PlayerPrefs.GetInt("NbSave" + numSave)+1);
        PlayerPrefs.SetFloat("TotalPlaytime" + numSave, PlayerPrefs.GetFloat("TotalPlaytime" + numSave) + Time.timeSinceLevelLoad);
        Debug.Log(sceneIndex);
        Debug.Log(playerLocation);
        transform.root.Find("PanelSaveLoad").gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        Debug.Log("LOAD");
        numSave = gameObject.name;
        Debug.Log("numSave : " + numSave);
        sceneToLoad = PlayerPrefs.GetInt("SavedScene" + numSave)-1;
        Debug.Log(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
        playerLocation = new Vector3(PlayerPrefs.GetFloat("Xlocation"+numSave), PlayerPrefs.GetFloat("Ylocation"+numSave), PlayerPrefs.GetFloat("Zlocation" + numSave));
        hasSaved = true;
        currentSave = numSave;
    }

    public void SaveLoadGame()
    {
        numSave = gameObject.name;
        if (PlayerPrefs.GetInt("SavedScene" + numSave) == 0)
        {
            SaveGame();
        }
        else
        {
            LoadGame();
        }
    }

    //public void AutoSaveGame()
    //{
    //    Debug.Log("AUTOSAVE");
    //    sceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    PlayerPrefs.SetInt("SavedScene" + currentSave, sceneIndex + 1);
    //    playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
    //    PlayerPrefs.SetFloat("Xlocation" + currentSave, playerLocation.x);
    //    PlayerPrefs.SetFloat("Ylocation" + currentSave, playerLocation.y);
    //    PlayerPrefs.SetFloat("Zlocation" + currentSave, playerLocation.z);
    //    Debug.Log(sceneIndex);
    //    Debug.Log(playerLocation);
    //}

    //public void DeleteSave()
    //{
    //    Debug.Log("DELETE");
    //    numSave = transform.parent.name;
    //    PlayerPrefs.DeleteKey("SavedScene" + numSave);
    //    PlayerPrefs.DeleteKey("Xlocation" + numSave);
    //    PlayerPrefs.DeleteKey("Ylocation" + numSave);
    //    PlayerPrefs.DeleteKey("Zlocation" + numSave);
    //}

    public void loadNewStats()
    {
        player.transform.position = playerLocation;
    }
}
