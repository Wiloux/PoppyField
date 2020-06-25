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
    public float range;
    public float recoveryTime;



    public AudioClip shootSound;
    public float shootVolume = 0.5f;
    public AudioClip reloadSound;
    public float reloadVolume = 0.5f;

    public Sprite bulletSprite;
    public Sprite crossHairSprite;

}
