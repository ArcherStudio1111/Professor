using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public event Action OnGameStart;

    [SerializeField] private ExperimentManager experimentManager;

    [Header("Parameters Text")]
    [SerializeField] private TMP_InputField bouncinessText;
    [SerializeField] private TMP_InputField staticFrictionText;
    [SerializeField] private TMP_InputField dynamicFrictionText;
    [SerializeField] private TMP_InputField testNumText;
    [SerializeField] private TMP_InputField precisionAccuracyText;
    public enum Parameters { Bounciness, StaticFriction, DynamicFriction }
    public Parameters testingParameter = Parameters.Bounciness;

    private float bounciness;
    private float staticFriction;
    private float dynamicFriction;
    private int testNum;
    private float precision;
    private float managerInterval;
    private int maxRowManagers;

    public void StartGame()
    {
        ReadParameters();
        SpawnExperimentManagers();
        SetParameters();
        OnGameStart.Invoke();
    }

    private void ReadParameters()
    {
        if (!float.TryParse(bouncinessText.text, out bounciness))
        {
            bounciness = 0.3f;
        }

        if (!float.TryParse(staticFrictionText.text,out staticFriction))
        {
            staticFriction = 0.05f;
        }

        if (!float.TryParse(dynamicFrictionText.text, out dynamicFriction))
        {
            dynamicFriction = 0.05f;
        }

        if (!int.TryParse(testNumText.text, out testNum)) 
        { 
            testNum = 1;
        }

        if (!float.TryParse(precisionAccuracyText.text, out precision))
        {
            precision = 0;
        }

    }

    private void SpawnExperimentManagers()
    {
        for (int i = 0; i < testNum; i++)
        {
            var rowRemainder = i % maxRowManagers;
            Vector3 Xoffset = Vector3.back * managerInterval * rowRemainder;
            Vector3 Yoffset = Vector3.left * managerInterval * (i / maxRowManagers);
            var experimentManagerClone = Instantiate(experimentManager, transform.position + Xoffset + Yoffset, Quaternion.identity);

            switch (testingParameter)
            {
                case Parameters.Bounciness:
                    experimentManagerClone.bounciness = bounciness + i * precision;
                    break;
                case Parameters.StaticFriction:
                    experimentManagerClone.staticFriction = staticFriction + i * precision;
                    break;
                case Parameters.DynamicFriction:
                    experimentManagerClone.dynamicFriction = dynamicFriction + i * precision;
                    break;
            }
        }
    }

    private void SetParameters()
    {

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
