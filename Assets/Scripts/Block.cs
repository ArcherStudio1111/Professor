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

    [SerializeField] private Rigidbody rb;
    [SerializeField] private MeshCollider meshCollider;

    private bool isDestroying;
    private float destroyInterval = 1;
    private float destroyTimer = 0;
    private float oscillateTimer = 0;
    private bool isOscillateAdd;

    private void Update()
    {
        JudgeStatic();
        OscillateParameters();
    }

    private void JudgeStatic()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            isDestroying = true;
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyInterval && isDestroying)
            {
                InvokeBlockFinishEvent();
            }
        }
        else if (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
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
