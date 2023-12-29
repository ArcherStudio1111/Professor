using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public float staticFriction = 0.05f;
    public float dynamicFriction = 0.05f;
    public float bounciness = 0.3f;

    [SerializeField] private List<GameObject> blocksUp = new List<GameObject>();
    [SerializeField] private List<GameObject> blocksDown = new List<GameObject>();
    [SerializeField] private GameObject upFunnel;
    [SerializeField] private GameObject downFunnel;

    private bool isUpDirection = true;
    private int currentBlock = 0;

    public void SetParameters(float staticParameter, float dynamicParameter, float bounceParameter)
    {
        staticFriction = staticParameter;
        dynamicFriction = dynamicParameter;
        bounciness = bounceParameter;
        if (staticFriction < 0)
        {
            staticFriction = 0;
        }
        if (dynamicFriction < 0)
        {
            dynamicFriction = 0;
        }
        if(bounciness < 0)
        {
            bounciness = 0;
        }
        else if (bounciness > 1) 
        { 
            bounciness = 1;
        }
    }

    public void SpawnBlock()
    {
        if (isUpDirection && currentBlock >= blocksUp.Count)
        {
            currentBlock = 0;
            isUpDirection = false;
            upFunnel.SetActive(false);
            downFunnel.SetActive(true);
        }

        if (isUpDirection)
        {
            blocksUp[currentBlock].SetActive(true);
            currentBlock++;
        }
        else
        {
            if (currentBlock >= blocksDown.Count)
            {
                OutputResult();
            }
            else
            {
                blocksDown[currentBlock].SetActive(true);
                currentBlock++;
            }
        }
    }

    private void OutputResult()
    {
        Debug.Log("Result");
    }
}
