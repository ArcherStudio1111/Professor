using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event Action blockFinishEvent;

    [SerializeField] private Rigidbody rb;

    private bool isDestroying;
    private float destroyInterval = 1;
    private float destroyTimer = 0;

    private void Update()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            isDestroying = true;
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyInterval && isDestroying)
            {
                blockFinishEvent?.Invoke();
            }
        }
        else if(rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
        {
            isDestroying = false;
            destroyTimer = 0;
        }
    }
}
