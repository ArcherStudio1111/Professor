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
                var experimentObject = other.transform.parent.parent.parent.parent.GetComponent<ExperimentObject>();
                if (experimentObject.blockPassStatus != ExperimentObject.BlockPassStatus.Passed)
                {
                    experimentObject.blockPassStatus = ExperimentObject.BlockPassStatus.OutBound;
                }
                other.transform.parent.parent.GetComponent<Block>().InvokeBlockFinishEvent();
            }
            else if (judgeCombine.Equals(JudgeCombine.IsNotCombinedBlock))
            {
                var experimentObject = other.transform.parent.parent.GetComponent<ExperimentObject>();
                if (experimentObject.blockPassStatus != ExperimentObject.BlockPassStatus.Passed)
                {
                    experimentObject.blockPassStatus = ExperimentObject.BlockPassStatus.OutBound;
                }
                other.GetComponent<Block>().InvokeBlockFinishEvent();
            }
        }
    }
}

