using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    [Header("Parameters")]
    public float staticFriction = 0.05f;
    public float dynamicFriction = 0.05f;
    public float bounciness = 0.3f;

    [Header("Test Objects")]
    [SerializeField] private List<GameObject> blocksUp = new List<GameObject>();
    [SerializeField] private List<GameObject> blocksDown = new List<GameObject>();
    [SerializeField] private GameObject upFunnel;
    [SerializeField] private GameObject downFunnel;
    [SerializeField] private GameObject upObjects;
    [SerializeField] private GameObject downObjects;

    private bool isUpDirection = true;
    private int currentBlock = 0;
    private GameManager gameManager;

    private void Awake()
    {
        SetParameters();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStart += SpawnBlock;
    }

    private void SetParameters()
    {
        SetBouciness();
        SetFriction();
    }

    private void SetBouciness()
    {
        //Check parameter
        if (bounciness < 0)
        {
            //Debug.LogWarning("bounciness is too large!");
            bounciness = 0;
        }
        else if (bounciness > 1)
        {
            bounciness = 1;
        }

        upFunnel.GetComponent<MeshCollider>().material.bounciness = bounciness;
        downFunnel.GetComponent<MeshCollider>().material.bounciness = bounciness;
    }

    private void SetFriction()
    {
        //Check parameters
        if (staticFriction < 0)
        {
            staticFriction = 0;
        }
        if (dynamicFriction < 0)
        {
            dynamicFriction = 0;
        }

        upFunnel.GetComponent<MeshCollider>().material.staticFriction = staticFriction;
        upFunnel.GetComponent<MeshCollider>().material.dynamicFriction = dynamicFriction;
        downFunnel.GetComponent<MeshCollider>().material.staticFriction = staticFriction;
        downFunnel.GetComponent<MeshCollider>().material.dynamicFriction = dynamicFriction;
    }

    public void SpawnBlock()
    {
        if (isUpDirection && currentBlock >= blocksUp.Count)
        {
            currentBlock = 0;
            isUpDirection = false;
            upObjects.SetActive(false);
            downObjects.SetActive(true);
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
