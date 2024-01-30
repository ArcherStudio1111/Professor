using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    [Header("Test Status")]
    public float totalTestTimes;

    [Header("Test Result")]
    public float passedTimes;
    public float obstructedTimes;
    public float outBoundTimes;
    public float passedAndObstructedTimes;
    public float passedEfficiency;
    public float obstructedEfficiency;
    
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
                experimentObject.SpawnBlock();
            }
            Time.timeScale = 0;
        }
    }

    public void CalculateEfficiency()
    {
        passedEfficiency = (passedTimes - obstructedTimes) / passedAndObstructedTimes;
        obstructedEfficiency = (obstructedTimes - passedTimes) / passedAndObstructedTimes;
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
        var outputPath = Environment.CurrentDirectory + @"\TestResult\"
+ SceneManager.GetActiveScene().name
+ "-TestResult"
+ DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss-")
+ ".txt";
        using (StreamWriter testResult = new StreamWriter(outputPath))
        {
            testResult.WriteLine("passed Times: " + passedTimes);
            testResult.WriteLine("obstructed Times: " + obstructedTimes);
            testResult.WriteLine("outBound Times: " + outBoundTimes);
            testResult.WriteLine("passed And Obstructed Times: " + passedAndObstructedTimes);
            testResult.WriteLine("passed Efficiency: " + passedEfficiency);
            testResult.WriteLine("obstructed Efficiency: " + obstructedEfficiency);
        }

        Debug.Log("passed Times: " + passedTimes);
        Debug.Log("obstructed Times: " + obstructedTimes);
        Debug.Log("outBound Times: " + outBoundTimes);
        Debug.Log("passed And Obstructed Times: " + passedAndObstructedTimes);
        Debug.Log("passed Efficiency: " + passedEfficiency);
        Debug.Log("obstructed Efficiency: " + obstructedEfficiency);
        Debug.Log("Test Over!");
    }
}
