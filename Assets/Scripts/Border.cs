using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private ExperimentManager experimentManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            experimentManager.SpawnBlock();
            Destroy(collision.gameObject);
        }
    }
}
