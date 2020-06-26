using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Functionnalities : MonoBehaviour
{

    #region Singleton
    public static Functionnalities _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("More than one inventory");
            return;
        }
        _instance = this;
    }
    #endregion

    public int numberSlots;

    public UISlots Ui;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("done");
            Ui.CalculSizeInventory();
        }
    }
}
