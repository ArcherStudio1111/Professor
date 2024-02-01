using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ExperimentObject : MonoBehaviour
{
    [Header("Status")]
    public bool isSpawnRedBlock;
    public bool isReportAbnormalData;
    public bool isCatchAbnormalData;

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
    [SerializeField] private GameObject blockRedPivot;
    [SerializeField] private GameObject blockBluePivot;

    [Header("Block Parameters")]
    public bool isOscillate;
    public float minBounce;
    public float maxBounce;
    public float minFriction;
    public float maxFriction;
    public float oscillateInterval;
    public float blockScale;

    public enum BlockPassStatus { Passed, Obstructed, OutBound }
    [HideInInspector] public BlockPassStatus blockPassStatus;

    private GameObject blockClone;
    private ExperimentManager experimentManager;

    private void Awake()
    {
        experimentManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<ExperimentManager>();
    }

    public void SpawnBlock()
    {
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
        if (isSpawnRedBlock)
        {
            blockClone = Instantiate(blockRedPivot, transform.position + originPosition, Quaternion.Euler(originRotation), transform);
        }
        else
        {
            blockClone = Instantiate(blockBluePivot, transform.position + originPosition, Quaternion.Euler(originRotation), transform);
        }
    }

    private void InitializeStatus()
    {
        blockPassStatus = BlockPassStatus.Obstructed;
        blockClone.transform.localScale = Vector3.one * blockScale;

        var blockScript =  blockClone.GetComponentInChildren<Block>();
        blockScript.blockFinishEvent += OnblockFinish;
        blockScript.isOscillate = isOscillate;
        blockScript.minBounce = minBounce;
        blockScript.maxBounce = maxBounce;
        blockScript.minFriction = minFriction;
        blockScript.maxFriction = maxFriction;
        blockScript.oscillateInterval = oscillateInterval;
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
        SendResultToManager(blockPassStatus);
        Destroy(blockClone);
        experimentManager.CountTestTime();
        SpawnBlock();
    }

    public void SendResultToManager(BlockPassStatus blockPassStatus)
    {
        switch (blockPassStatus)
        {
            case BlockPassStatus.Passed:
                experimentManager.passedTimes++;
                experimentManager.passedAndObstructedTimes++;
                experimentManager.CalculateEfficiency();
                if (isSpawnRedBlock)
                {
                    if (isReportAbnormalData)
                    {
                        OutPutAbnormalResult("Red");
                    }
                    if (isCatchAbnormalData)
                    {
                        Debug.Log("Catched abnormal data!");
                        Time.timeScale = 0;
                    }
                }
                break;
            case BlockPassStatus.Obstructed:
                experimentManager.obstructedTimes++;
                experimentManager.passedAndObstructedTimes++;
                experimentManager.CalculateEfficiency();
                if (!isSpawnRedBlock)
                {
                    if (isReportAbnormalData)
                    {
                        OutPutAbnormalResult("Blue");
                    }
                    if (isCatchAbnormalData)
                    {
                        Debug.Log("Catched abnormal data!");
                        Time.timeScale = 0;
                    }
                }
                break;
            case BlockPassStatus.OutBound: 
                experimentManager.outBoundTimes++;
                break;
        }
    }

    private void OutPutAbnormalResult(string blockColor)
    {
        //Get physics parameters
        var staticFriction = blockClone.GetComponentInChildren<MeshCollider>().material.staticFriction;
        var dynamicFriction = blockClone.GetComponentInChildren<MeshCollider>().material.dynamicFriction;
        var bounciness = blockClone.GetComponentInChildren<MeshCollider>().material.bounciness;

        //Output result
        var outputPath = Environment.CurrentDirectory + @"\Assets\ExperimentResult\AbnormalResult\" + SceneManager.GetActiveScene().name + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss-") + "AbnormalResult" + ".txt";
        using (StreamWriter testResult = new StreamWriter(outputPath))
        {
            testResult.WriteLine("Block Color: " + blockColor);
            testResult.WriteLine("origin Position: " + originPosition);
            testResult.WriteLine("origin Rotation: " + originRotation);
            testResult.WriteLine("origin Linear Velocity: " + originLinearVelocity);
            testResult.WriteLine("origin Angular Velocity: " + originAngularVelocity);
            testResult.WriteLine("static Friction: " + staticFriction);
            testResult.WriteLine("dynamic Friction: " + dynamicFriction);
            testResult.WriteLine("bounciness: " + bounciness);
        }
    }
}
