using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            other.GetComponent<Block>().InvokeBlockFinishEvent();
        }
    }
}

