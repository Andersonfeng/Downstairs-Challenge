using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.SpikedBallTrigger(gameObject);
        }
    }
}