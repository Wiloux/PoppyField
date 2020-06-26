using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handPlacement : MonoBehaviour
{
    public Transform HandAim;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (HandAim != null)
        {
            transform.position = new Vector3(HandAim.transform.position.x, HandAim.transform.position.y, HandAim.transform.position.z);
            transform.rotation = Quaternion.Euler(HandAim.transform.rotation.x, HandAim.transform.rotation.y, HandAim.transform.rotation.z);
        }
    }
}
