using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Vector3[] generatePos;

    [SerializeField] private GameObject block;
    [SerializeField] private TextMeshProUGUI totalBlockText;
    [SerializeField] private TextMeshProUGUI passedBlockText;

    private int passedBlockNum = 0;
    private int totalBlockNum = 0;
    private int currentGeneratePos = 0;

    public void StartGame()
    {
        GenerateBlock();
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

    public void GenerateBlock()
    {
        Instantiate(block, generatePos[currentGeneratePos], Quaternion.identity);
        totalBlockNum++;
        totalBlockText.text = "Total Blocks: " + totalBlockNum.ToString();
        currentGeneratePos++;
        if (currentGeneratePos >= generatePos.Length)
        {
            currentGeneratePos = 0;
        }
    }
}
