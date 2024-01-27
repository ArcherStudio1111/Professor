using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private ExperimentObject experimentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            experimentObject.blockPassStatus = ExperimentObject.BlockPassStatus.OutBound;
            experimentObject.OnblockFinish();
        }
    }
}
