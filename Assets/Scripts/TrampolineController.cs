using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : PlatformController
{
    private Animator _animator;
    private static readonly int Up = Animator.StringToHash("popUp");
    private PlayerController _playerController;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PopUp();
            _playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PopUp();
            _playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }

    /**
     * 弹起动画
     */
    private void PopUp()
    {
        _animator.SetBool(Up,true);
    }

    /*
     * 弹起玩家
     */
    private void Trigger()
    {
        _playerController.TrampolineTrigger(gameObject);
    }

    /**
     * 恢复
     */
    private void RecoverIdle()
    {
        _animator.SetBool(Up,false);
    }


}
