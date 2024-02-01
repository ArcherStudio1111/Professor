using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    [Header("Test Status")]
    public bool isSpawnRedBlock;
    public bool isReportAbnormalData;
    public int totalTestTimes;
    public int experimentObjectNum = 1;

    [Header("Test Result")]
    public float passedTimes;
    public float obstructedTimes;
    public float outBoundTimes;
    public float passedAndObstructedTimes;
    public float overallYield;

    [Header("Position")]
    public bool isRandomOriginPosition = true;
    public float randomRadius = 0.9f;
    public float minRandomY = 1.5f;
    public float maxRandomY = 4f;
    public Vector3 originPosition;

    [Header("Rotation")]
    public bool isRandonOriginRotation = true;
    public Vector3 originRotation;

    [Header("Linear Velocity")]
    public bool isRandomLinearVelocity = true;
    public Vector3 minRandomLinearVelocity = new Vector3(-2, -2, -2);
    public Vector3 maxRandomLinearVelocity = new Vector3(2, 0, 2);
    public Vector3 originLinearVelocity;

    [Header("Angular Velocity")]
    public bool isRandomAngularVelocity = true;
    public Vector3 minRandomAngularVelocity = Vector3.zero;
    public Vector3 maxRandomAngularVelocity = new Vector3(300, 300, 300);
    public Vector3 originAngularVelocity;

    [Header("Block Parameters")]
    public bool isOscillate;
    public float minBounce = 0.502f;
    public float maxBounce = 0.498f;
    public float minFriction = 0.001f;
    public float maxFriction = 0.001f;
    public float oscillateInterval = 0.2f;
    public float blockScale = 0.95f;

    [Space(20)]
    public ExperimentObject experimentObjectForTest;

    private List<ExperimentObject> experimentObjects = new List<ExperimentObject>();
    private float experimentObjectInterval = 5f;
    private int experimentObjectRowNum = 5;

    public void StartGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void SpawnBlocks()
    {
        if(totalTestTimes > 0 && FindFirstObjectByType<ExperimentObject>() == null)
        {
            SpawnExperimentObjects();
            if (experimentObjects.Count > 0)
            {
                foreach (var experimentObject in experimentObjects)
                {
                    InitializeParameters(experimentObject);
                    experimentObject.SpawnBlock();
                }
                Time.timeScale = 0;
            }
        }
    }

    private void SpawnExperimentObjects()
    {
        for (int i = 0; i < experimentObjectNum; i++)
        {
            var experimentObjectClone = Instantiate(experimentObjectForTest, 
                new Vector3(i % experimentObjectRowNum, 0, i / experimentObjectRowNum) * experimentObjectInterval,
                Quaternion.identity, transform);
            experimentObjects.Add(experimentObjectClone);
        }
    }

    private void InitializeParameters(ExperimentObject experimentObject)
    {
        //Status
        experimentObject.isSpawnRedBlock = isSpawnRedBlock;
        experimentObject.isReportAbnormalData = isReportAbnormalData;

        //Position
        experimentObject.isRandomOriginPosition = isRandomOriginPosition;
        experimentObject.randomRadius = randomRadius;
        experimentObject.minRandomY = minRandomY;
        experimentObject.maxRandomY = maxRandomY;
        experimentObject.originPosition = originPosition;

        //Rotation
        experimentObject.isRandonOriginRotation = isRandonOriginRotation;
        experimentObject.originRotation = originRotation;

        //Linear Velocity
        experimentObject.isRandomLinearVelocity = isRandomLinearVelocity;
        experimentObject.minRandomLinearVelocity = minRandomLinearVelocity;
        experimentObject.maxRandomLinearVelocity = maxRandomLinearVelocity;
        experimentObject.originLinearVelocity = originLinearVelocity;

        //Angular Velocity
        experimentObject.isRandomAngularVelocity = isRandomAngularVelocity;
        experimentObject.minRandomAngularVelocity = minRandomAngularVelocity;
        experimentObject.maxRandomAngularVelocity = maxRandomAngularVelocity;
        experimentObject.originAngularVelocity = originAngularVelocity;

        //Block Parameters
        experimentObject.isOscillate = isOscillate;
        experimentObject.minBounce = minBounce;
        experimentObject.maxBounce = maxBounce;
        experimentObject.minFriction = minFriction;
        experimentObject.maxFriction = maxFriction;
        experimentObject.oscillateInterval = oscillateInterval;
        experimentObject.blockScale = blockScale;
    }

    public void CalculateEfficiency()
    {
        if (!isSpawnRedBlock)
        {
            overallYield = passedTimes / passedAndObstructedTimes;
        }
    }

    public void CountTestTime()
    {
        totalTestTimes--;
        if(totalTestTimes <= 0)
        {
            Time.timeScale = 0;
            OutPutResult();
        }
    }

    private void OutPutResult()
    {
        var outputPath = Environment.CurrentDirectory + @"\TestResult\" + SceneManager.GetActiveScene().name + "-TestResult" + DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss-") + ".txt";
        using (StreamWriter testResult = new StreamWriter(outputPath))
        {
            testResult.WriteLine("passed Times: " + passedTimes);
            testResult.WriteLine("obstructed Times: " + obstructedTimes);
            testResult.WriteLine("outBound Times: " + outBoundTimes);
            testResult.WriteLine("passed And Obstructed Times: " + passedAndObstructedTimes);
            if (!isSpawnRedBlock)
            {
                testResult.WriteLine("Overall Yield(blue): " + overallYield);
                testResult.WriteLine("Block Color: Blue");
            }
            else
            {
                testResult.WriteLine("Block Color: Red");
            }
        }
        Debug.Log("Test Over!");
    }
}
