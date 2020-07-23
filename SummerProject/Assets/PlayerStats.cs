using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float Health = 150f;
    public float MaxHealth = 150f;
    public float StruggleMax = 1f;
    public GameObject HealthUI;
    private void Start()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        HealthUI.GetComponent<Image>().fillAmount = Health / MaxHealth;
    }
}
