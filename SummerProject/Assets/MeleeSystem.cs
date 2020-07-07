using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public float Dmg;
    public bool isAttacking;
    public GameObject Target;
    public GunBehaviour Behaviour;

    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ennemi" && isAttacking)
        {
           other.gameObject.GetComponent<EnnemyBehaviour>().TakeDamage(Dmg);
        }
    }
}
