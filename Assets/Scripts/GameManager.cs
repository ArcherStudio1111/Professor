using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Vector3[] spawnPos;

    [SerializeField] private GameObject block;
    [SerializeField] private TextMeshProUGUI totalBlockText;
    [SerializeField] private TextMeshProUGUI passedBlockText;

    private int passedBlockNum = 0;
    private int totalBlockNum = 0;
    private int currentSpawnPos = 0;

    public void StartGame()
    {
        SpawnBlock();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void AddPassedBlock()
    {
        passedBlockNum++;
        passedBlockText.text = "Passed Blocks: " + passedBlockNum.ToString();
    }

    public void SpawnBlock()
    {
        Instantiate(block, spawnPos[currentSpawnPos], Quaternion.identity);
        totalBlockNum++;
        totalBlockText.text = "Total Blocks: " + totalBlockNum.ToString();
        currentSpawnPos++;
        if (currentSpawnPos >= spawnPos.Length)
        {
            currentSpawnPos = 0;
            ReportParameters();
            ChangeParameters();
        }
    }

    private void ReportParameters()
    {

    }

    private void ChangeParameters()
    {

    }
}
