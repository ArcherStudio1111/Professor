using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedPlane : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            gameManager.AddPassedBlock();
            gameManager.SpawnBlock();
            Destroy(collision.gameObject);
        }
    }
}
