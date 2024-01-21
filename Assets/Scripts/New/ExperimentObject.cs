using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentObject : MonoBehaviour
{
    [Header("Status")]
    public bool isSpawnLeftBlock;
    public bool isStartingGame;
    public bool restartGame;
    public bool isRandomOriginPosition;
    public bool isRandonOriginRotation;
    public Vector3 originPosition;
    public Vector3 originRotation;

    [Header("Blocks")]
    [SerializeField] private GameObject blockLeftPivot;
    [SerializeField] private GameObject blockRightPivot;

    private GameObject blockClone;

    private float randCircleRadius = 1.25f;
    private float randMinPosY = 2f;
    private float randMaxPosY = 3f;

    private void Awake()
    {
        Time.timeScale = 0;
        SpawnBlock();
    }

    public void StartGame()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 0;

        //Reset position and rotation
        if (isRandomOriginPosition)
        {
            var randCirclePosition = randCircleRadius * Random.insideUnitCircle;
            originPosition = new Vector3(randCirclePosition.x, Random.Range(randMinPosY, randMaxPosY), randCirclePosition.y);
        }
        if (isRandonOriginRotation)
        {
            originRotation = new Vector3(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f));
        }

        Destroy(blockClone);
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        if (isSpawnLeftBlock)
        {
            blockClone = Instantiate(blockLeftPivot, originPosition, Quaternion.Euler(originRotation));
        }
        else
        {
            blockClone = Instantiate(blockRightPivot, originPosition, Quaternion.Euler(originRotation));
        }
    }
}
