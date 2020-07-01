using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
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
        numSave = this.gameObject.name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene"+numSave, sceneIndex);
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        PlayerPrefs.SetFloat("Xlocation" + numSave, playerLocation.x);
        PlayerPrefs.SetFloat("Ylocation" + numSave, playerLocation.y);
        PlayerPrefs.SetFloat("Zlocation" + numSave, playerLocation.z);
        Debug.Log(sceneIndex);
        Debug.Log(playerLocation);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        Debug.Log("LOAD");
        numSave = this.gameObject.name;
        Debug.Log("numSave : " + numSave);
        sceneToLoad = PlayerPrefs.GetInt("SavedScene" + numSave);
        SceneManager.LoadScene(sceneToLoad);
        playerLocation = new Vector3(PlayerPrefs.GetFloat("Xlocation"+numSave), PlayerPrefs.GetFloat("Ylocation"+numSave), PlayerPrefs.GetFloat("Zlocation" + numSave));
        hasSaved = true;
    }
    public void loadNewStats()
    {
        player.transform.position = playerLocation;
    }
}
