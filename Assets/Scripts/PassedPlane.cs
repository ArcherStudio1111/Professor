using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedPlane : MonoBehaviour
{
    public enum JudgeCombine
    {
        IsNotCombinedBlock,
        IsCombinedBlock,
    }

    public JudgeCombine judgeCombine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            if (judgeCombine.Equals(JudgeCombine.IsCombinedBlock))
            {
                other.transform.parent.parent.GetComponent<Block>().InvokeBlockFinishEvent();
            }
            else if (judgeCombine.Equals(JudgeCombine.IsNotCombinedBlock))
            {
                other.GetComponent<Block>().InvokeBlockFinishEvent();
            }
        }
    }
}

