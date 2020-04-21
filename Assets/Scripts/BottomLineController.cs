using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomLineController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.GetComponent<PlayerController>();
            playerController.dead = true;
            GameManager.ShowGamePanel();
        }
    }
}