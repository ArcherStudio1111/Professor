using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    [Header("Test Status")]
    public bool isSpawnLeftBlock;
    public float totalTestTimes;

    [Header("Position")]
    public bool isRandomOriginPosition = true;
    public float randomRadius = 0.9f;
    public float minRandomY = 1.5f;
    public float maxRandomY = 4f;

    [Header("Rotation")]
    public bool isRandonOriginRotation = true;

    [Header("Linear Velocity")]
    public bool isRandomLinearVelocity = true;
    public Vector3 minRandomLinearVelocity = new Vector3(-2, -2, -2);
    public Vector3 maxRandomLinearVelocity = new Vector3(2, 0, 2);

    [Header("Angular Velocity")]
    public bool isRandomAngularVelocity = true;
    public Vector3 minRandomAngularVelocity = Vector3.zero;
    public Vector3 maxRandomAngularVelocity = new Vector3(300, 300, 300);

    [Header("Test Result")]
    public float passedTimes;
    public float obstructedTimes;
    public float outBoundTimes;
    public float passedAndObstructedTimes;
    public float overallYield;
    
    [Space(20)]
    [SerializeField] private List<ExperimentObject> experimentObjects = new List<ExperimentObject>();

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
            foreach (var experimentObject in experimentObjects)
            {
                InitializeParameters(experimentObject);
                experimentObject.SpawnBlock();
            }
            Time.timeScale = 0;
        }
    }

    private void InitializeParameters(ExperimentObject experimentObject)
    {
        //Status
        experimentObject.isSpawnLeftBlock = isSpawnLeftBlock;

        //Position
        experimentObject.isRandomOriginPosition = isRandomOriginPosition;
        experimentObject.randomRadius = randomRadius;
        experimentObject.minRandomY = minRandomY;
        experimentObject.maxRandomY = maxRandomY;

        //Rotation
        experimentObject.isRandonOriginRotation = isRandonOriginRotation;

        //Linear Velocity
        experimentObject.isRandomLinearVelocity = isRandomLinearVelocity;
        experimentObject.minRandomLinearVelocity = minRandomLinearVelocity;
        experimentObject.maxRandomLinearVelocity = maxRandomLinearVelocity;

        //Angular Velocity
        experimentObject.isRandomAngularVelocity = isRandomAngularVelocity;
        experimentObject.minRandomAngularVelocity = minRandomAngularVelocity;
        experimentObject.maxRandomAngularVelocity = maxRandomAngularVelocity;
    }

    public void CalculateEfficiency()
    {
        if (!isSpawnLeftBlock)
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
            if (!isSpawnLeftBlock)
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
