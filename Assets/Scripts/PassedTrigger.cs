using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedTrigger : MonoBehaviour
{
    [SerializeField] private ExperimentObject experimentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            experimentObject.blockPassStatus = ExperimentObject.BlockPassStatus.Passed;
        }
    }
}
