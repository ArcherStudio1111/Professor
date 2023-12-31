using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedCollider : MonoBehaviour
{
    [SerializeField] private ExperimentManager experimentManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            //experimentManager.SpawnBlock();
            //Destroy(other.gameObject);
        }
    }
}

