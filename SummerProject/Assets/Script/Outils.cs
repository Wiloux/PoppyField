using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Outils : MonoBehaviour
{
    public static void SmoothRigWeight(bool On, TwoBoneIKConstraint rig)
    {
        if (On)
        {
            if (rig.weight < 1)
            {
                rig.weight += 0.05f;
            }
        }
        else
        {
            if (rig.weight > 0)
            {
                rig.weight -= 0.05f;
            }
        }
    }
}
