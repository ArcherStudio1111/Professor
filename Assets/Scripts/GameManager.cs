using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        SpawnExperimentManagers();
        SetParameters();
    }

    private void SpawnExperimentManagers()
    {

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
