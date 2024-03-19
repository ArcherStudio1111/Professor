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
    public bool isSingleMode;
    public bool isCatchAbnormalObject;
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
    public float angularDrag = 0.05f;
    public float linearDrag;

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
        if(totalTestTimes > 0)
        {
            SpawnExperimentObjects();
            if (experimentObjects.Count > 0)
            {
                foreach (var experimentObject in experimentObjects)
                {
                    experimentObject.SpawnBlock();
                }
                Time.timeScale = 0;
            }
        }
    }

    private void SpawnExperimentObjects()
    {
        foreach (var experimentObject in experimentObjects)
        {
            Destroy(experimentObject.gameObject);
        }
        experimentObjects.Clear();
        ResetResult();

        for (int i = 0; i < experimentObjectNum; i++)
        {
            var experimentObjectClone = Instantiate(experimentObjectForTest, 
                new Vector3(i % experimentObjectRowNum, 0, i / experimentObjectRowNum) * experimentObjectInterval,
                Quaternion.identity, transform);
            experimentObjects.Add(experimentObjectClone);

            //Only spawn one object
            if (isSingleMode)
            {
                return;
            }
        }
    }

    private void ResetResult()
    {
        passedTimes = 0;
        obstructedTimes = 0;
        passedAndObstructedTimes = 0;
        outBoundTimes = 0;
        overallYield = 0;
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
        var outputPath = Environment.CurrentDirectory + @"\Assets\ExperimentResult\TestResult\" + SceneManager.GetActiveScene().name + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + "TestResult" + ".txt";
        using (StreamWriter testResult = new StreamWriter(outputPath))
        {
            testResult.WriteLine("Scene Name: " + SceneManager.GetActiveScene().name);
            testResult.WriteLine("Block Color: " + (isSpawnRedBlock ? "Red" : "Blue"));
            testResult.WriteLine("passed Times: " + passedTimes);
            testResult.WriteLine("obstructed Times: " + obstructedTimes);
            testResult.WriteLine("outBound Times: " + outBoundTimes);
            testResult.WriteLine("passed And Obstructed Times: " + passedAndObstructedTimes);
            if (!isSpawnRedBlock)
            {
                testResult.WriteLine("Overall Yield(blue): " + overallYield);
            }
        }
        Debug.Log("Test Over!");
    }
}
