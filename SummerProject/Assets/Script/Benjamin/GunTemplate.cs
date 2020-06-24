using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunTemplate : ScriptableObject
{
    public string gunName;

    public int gunDamage;
    public int nbAmmo;
    public int maxAmmo;

    public float reloadTime;

    public AudioClip shootSound;

    public Sprite bulletSprite;

}
