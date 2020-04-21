using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    [Header("背景音乐")] public AudioClip backgroundClip;
    private AudioSource _backgroundSouce;

    [Header("跳跃音效")] public AudioClip jumpClip;
    private AudioSource _jumpSouce;

    [Header("弹簧床音效")] public AudioClip trampolineClip;
    private AudioSource _trampolineSouce;

    [Header("受击音效")] public AudioClip hitClip;
    private AudioSource _hitSouce;

    [Header("落地音效")] public AudioClip onGroundClip;
    private AudioSource _onGroundSouce;

    [Header("风扇音效")] public AudioClip onFanClip;
    private AudioSource _onFanSouce;

    [Header("坠落平台音效")] public AudioClip fallingplatformClip;
    private AudioSource _fallingplatformSouce;

    private void Awake()
    {
        Debug.Log("_instance" + _instance);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _backgroundSouce = gameObject.AddComponent<AudioSource>();
        _backgroundSouce.clip = backgroundClip;

        _jumpSouce = gameObject.AddComponent<AudioSource>();
        _jumpSouce.clip = jumpClip;

        _trampolineSouce = gameObject.AddComponent<AudioSource>();
        _trampolineSouce.clip = trampolineClip;

        _hitSouce = gameObject.AddComponent<AudioSource>();
        _hitSouce.clip = hitClip;

        _onGroundSouce = gameObject.AddComponent<AudioSource>();
        _onGroundSouce.clip = onGroundClip;

        _onFanSouce = gameObject.AddComponent<AudioSource>();
        _onFanSouce.clip = onFanClip;

        _fallingplatformSouce = gameObject.AddComponent<AudioSource>();
        _fallingplatformSouce.clip = fallingplatformClip;

        PlayBackgroundSound();
        DontDestroyOnLoad(this);
    }

    /**
     * 切换背景音乐 播放/暂停
     */
    public void ToggleBackgroundSound()
    {
        if (_instance._backgroundSouce.isPlaying)
        {
            _instance._backgroundSouce.Pause();
        }
        else
        {
            PlayBackgroundSound();
        }
    }

    /**
     * 坠落平台音效
     */
    public static void PlayBackgroundSound()
    {
        _instance._backgroundSouce.loop = true;
        _instance._backgroundSouce.Play();
    }

    /**
     * 坠落平台音效
     */
    public static void PlayFallingplatformSound()
    {
        _instance._fallingplatformSouce.Play();
    }

    /**
     * 风扇音效
     */
    public static void PlayOnFanSound()
    {
        _instance._onFanSouce.Play();
    }

    /**
     * 落地音效
     */
    public static void PlayOnGroundSound()
    {
        _instance._onGroundSouce.Play();
    }

    /**
     * 跳音效
     */
    public static void PlayJumpSound()
    {
        _instance._jumpSouce.Play();
    }

    /*
     * 弹跳音效
     */
    public static void PlayTrampolineSound()
    {
        _instance._trampolineSouce.Play();
    }

    /**
     * 受击音效
     */
    public static void PlayHitSound()
    {
        _instance._hitSouce.Play();
    }
}