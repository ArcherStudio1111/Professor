using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject : MonoBehaviour
{
    [Header("Status")]
    public bool isStartingGame;
    public bool restartGame;
    public bool isRandomOriginPosition;
    public Vector3 originPosition;

    [Header("Blocks")]
    [SerializeField] private Transform leftBlock;
    [SerializeField] private Transform rightBlock;

    private float randCircleRadius = 2;
    private float randMinPosZ = 1.46f;
    private float randMaxPosZ = 4.07f;
    private Vector2 randCircleOffset = new Vector2(54.771f, 3.4105f);

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        StartGame();
        RestartGame();
    }

    private void StartGame()
    {
        if (isStartingGame)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    private void RestartGame()
    {
        if (restartGame)
        {
            restartGame = false;
            isStartingGame = false;
            if (isRandomOriginPosition)
            {
                var randCirclePosition = randCircleRadius * Random.insideUnitCircle + randCircleOffset;
                originPosition = new Vector3(randCirclePosition.x, randCirclePosition.y, Random.Range(randMinPosZ, randMaxPosZ));
            }
            leftBlock.localPosition = originPosition;
            rightBlock.localPosition = originPosition;
        }
    }
}
