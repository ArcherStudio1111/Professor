using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersForTestingButton : MonoBehaviour
{
    public GameManager.Parameters parametersForTesting;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ChangeTestingParameter()
    {
        gameManager.testingParameter = parametersForTesting;
    }
}
