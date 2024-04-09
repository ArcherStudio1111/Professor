using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isOscillate;
    public float minBounce;
    public float maxBounce;
    public float minFriction;
    public float maxFriction;
    public float oscillateInterval;

    public event Action blockFinishEvent;

    public Rigidbody rb;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private MeshCollider[] meshColliders;
    
    public enum JudgeCombine
     {
         IsNotCombinedBlock,
         IsCombinedBlock,
     }

    public JudgeCombine judgeCombine;
    
    private bool isDestroying;
    private float destroyInterval = 5;
    private float destroyTimer = 0;
    private float oscillateTimer = 0;
    private bool isOscillateAdd;
    private float previousHeight;
    private float currentHeight;

    private void Start()
    {
        StartCoroutine(CalcutePreviousHeight());
    }

    private void Update()
    {
        JudgeStatic();
        OscillateParameters();
    }

    private IEnumerator CalcutePreviousHeight()
    {
        previousHeight = transform.position.y;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CalcutePreviousHeight());
    }

    private void JudgeStatic()
    {
        currentHeight = transform.position.y;
        if (Mathf.Abs(currentHeight - previousHeight) < 0.1f)
        {
            isDestroying = true;
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyInterval && isDestroying)
            {
                InvokeBlockFinishEvent();
            }
        }
        else if (Mathf.Abs(currentHeight - previousHeight) >= 0.1f)
        {
            isDestroying = false;
            destroyTimer = 0;
        }
    }

    public void InvokeBlockFinishEvent()
    {
        blockFinishEvent?.Invoke();
    }

    private void OscillateParameters()
    {
        if(isOscillate) 
        {
            var bounceRange = maxBounce - minBounce;
            var frictionRange = maxFriction - minFriction;
            oscillateTimer += Time.deltaTime;
            var changePercent = oscillateTimer / oscillateInterval;

            if (oscillateTimer >= oscillateInterval)
            {
                isOscillateAdd = !isOscillateAdd;
                oscillateTimer = 0;
            }

            if (judgeCombine.Equals(JudgeCombine.IsCombinedBlock))
            {
                if (isOscillateAdd)
                {
                    foreach (var partMeshCollider in meshColliders)
                    {
                        partMeshCollider.material.staticFriction = minFriction + changePercent * frictionRange;
                        partMeshCollider.material.dynamicFriction = minFriction + changePercent * frictionRange;
                        partMeshCollider.material.bounciness = minBounce + changePercent * bounceRange;
                    }
                }
                else
                {
                    foreach (var partMeshCollider in meshColliders)
                    {
                        partMeshCollider.material.staticFriction = maxFriction - changePercent * frictionRange;
                        partMeshCollider.material.dynamicFriction = maxFriction - changePercent * frictionRange;
                        partMeshCollider.material.bounciness = maxBounce - changePercent * bounceRange;
                    }
                }
            }
            else if(judgeCombine.Equals(JudgeCombine.IsNotCombinedBlock))
            {
                if (isOscillateAdd)
                {
                    meshCollider.material.staticFriction = minFriction + changePercent * frictionRange;
                    meshCollider.material.dynamicFriction = minFriction + changePercent * frictionRange;
                    meshCollider.material.bounciness = minBounce + changePercent * bounceRange;

                }
                else
                {
                    meshCollider.material.staticFriction = maxFriction - changePercent * frictionRange;
                    meshCollider.material.dynamicFriction = maxFriction - changePercent * frictionRange;
                    meshCollider.material.bounciness = maxBounce - changePercent * bounceRange;
                }
            }
        }
    }
}
