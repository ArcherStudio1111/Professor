using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExperimentObject : MonoBehaviour
{
    [Header("Status")]
    public bool isSpawnLeftBlock;
    public bool isAutoTest;

    [Header("Position")]
    public bool isRandomOriginPosition;
    public float randomRadius = 1.25f;
    public float minRandomY = 0.8f;
    public float maxRandomY = 1.7f;
    public Vector3 originPosition;

    [Header("Rotation")]
    public bool isRandonOriginRotation;
    public Vector3 originRotation;

    [Header("Linear Velocity")]
    public bool isRandomLinearVelocity;
    public Vector3 minRandomLinearVelocity;
    public Vector3 maxRandomLinearVelocity;
    public Vector3 originLinearVelocity;

    [Header("Angular Velocity")]
    public bool isRandomAngularVelocity;
    public Vector3 minRandomAngularVelocity;
    public Vector3 maxRandomAngularVelocity;
    public Vector3 originAngularVelocity;

    [Header("Blocks")]
    [SerializeField] private GameObject blockLeftPivot;
    [SerializeField] private GameObject blockRightPivot;

    public enum BlockPassStatus { Passed, Obstructed, OutBound }
    public BlockPassStatus blockPassStatus;

    private GameObject blockClone;
    private ExperimentManager experimentManager;

    private void Awake()
    {
        experimentManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<ExperimentManager>();
    }

    public void StartGame()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void SpawnBlock()
    {
        if (!isAutoTest)
        {
            Time.timeScale = 0;
        }
        SetRandomPositionRotation();
        if(blockClone != null)
        {
            Destroy(blockClone);
        }
        InstantiateBlock();
        InitializeStatus();
        SetRandomVelocities();
    }

    private void SetRandomPositionRotation()
    {
        if (isRandomOriginPosition)
        {
            var randCirclePosition = randomRadius * Random.insideUnitCircle;
            originPosition = new Vector3(randCirclePosition.x, Random.Range(minRandomY, maxRandomY), randCirclePosition.y);
        }
        if (isRandonOriginRotation)
        {
            originRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
    }

    private void InstantiateBlock()
    {
        if (isSpawnLeftBlock)
        {
            blockClone = Instantiate(blockLeftPivot, transform.position + originPosition, Quaternion.Euler(originRotation), transform);
        }
        else
        {
            blockClone = Instantiate(blockRightPivot, transform.position + originPosition, Quaternion.Euler(originRotation), transform);
        }
    }

    private void InitializeStatus()
    {
        blockClone.GetComponentInChildren<Block>().blockFinishEvent += OnblockFinish;
        blockPassStatus = BlockPassStatus.Obstructed;
    }

    private void SetRandomVelocities()
    {
        if (isRandomLinearVelocity)
        {
            originLinearVelocity = new Vector3(
                Random.Range(minRandomLinearVelocity.x, maxRandomLinearVelocity.x),
                Random.Range(minRandomLinearVelocity.y, maxRandomLinearVelocity.y),
                Random.Range(minRandomLinearVelocity.z, maxRandomLinearVelocity.z));
        }
        blockClone.GetComponentInChildren<Rigidbody>().velocity = originLinearVelocity;

        if (isRandomAngularVelocity)
        {
            originAngularVelocity = new Vector3(
                Random.Range(minRandomAngularVelocity.x, maxRandomAngularVelocity.x),
                Random.Range(minRandomAngularVelocity.y, maxRandomAngularVelocity.y),
                Random.Range(minRandomAngularVelocity.z, maxRandomAngularVelocity.z));
        }
        blockClone.GetComponentInChildren<Rigidbody>().angularVelocity = originAngularVelocity;
    }

    public void OnblockFinish()
    {
        var block = blockClone.GetComponentInChildren<Block>();
        block.blockFinishEvent -= OnblockFinish;
        OutPutResult(blockPassStatus);
        Destroy(blockClone);
        experimentManager.CountTestTime();
        SpawnBlock();
    }

    public void OutPutResult(BlockPassStatus blockPassStatus)
    {
        switch (blockPassStatus)
        {
            case BlockPassStatus.Passed:
                experimentManager.passedTimes++;
                experimentManager.passedAndObstructedTimes++;
                experimentManager.CalculateEfficiency();
                break;
            case BlockPassStatus.Obstructed:
                experimentManager.obstructedTimes++;
                experimentManager.passedAndObstructedTimes++;
                experimentManager.CalculateEfficiency();
                break;
            case BlockPassStatus.OutBound: 
                experimentManager.outBoundTimes++;
                break;
        }
    }
}
