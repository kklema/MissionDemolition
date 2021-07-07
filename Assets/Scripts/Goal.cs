using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color color = mat.color;
            color.a = 1;
            mat.color = color;
        }
    }
}