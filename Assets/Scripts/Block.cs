using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private GameManager gameManager;
    private bool isDestroying;
    private float destroyInterval = 1;
    private float destroyTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            isDestroying = true;
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= destroyInterval && isDestroying)
            {
                gameManager.GenerateBlock();
                Destroy(gameObject);
            }
        }
        else if(rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
        {
            isDestroying = false;
            destroyTimer = 0;
        }
    }
}
