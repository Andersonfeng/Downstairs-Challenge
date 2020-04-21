using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("平台生成位置")] public Transform generateLine;

    [Header("游戏面板")] public GameObject gamePanel;
    [Header("游戏分数")] public Text scoreText;
    [Header("玩家")] public GameObject player;

    private static GameManager _instance;
    public List<GameObject> platformList;
    public List<GameObject> trapList;
    public float offsetY = 3;

    private bool pause;
    private PlayerController _playerController;

    public float
        maxPlayformCount,
        timeDuration,
        randomTime,
        minRandomTime,
        maxRandomTime,
        generatePlatformCount;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        randomTime = Random.Range(minRandomTime, maxRandomTime);
        _playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        GeneratePlatform();
        GenerateTrap();

        if (Input.GetKeyDown(KeyCode.Escape))
            ShowGamePanel();
    }

    private void GenerateTrap()
    {
        //生成一定平台数量就生成一个陷阱
        if (generatePlatformCount >= maxPlayformCount)
        {
            var randomPosition = new Vector3(Random.Range(-offsetY, offsetY), generateLine.position.y,
                transform.position.y);
            Instantiate(trapList[Random.Range(0, trapList.Count)], randomPosition, Quaternion.identity);
            generatePlatformCount = 0;
        }
    }

    /**
     * 生成平台
     */
    private void GeneratePlatform()
    {
        timeDuration += Time.deltaTime;
        if (timeDuration > randomTime)
        {
            timeDuration = 0;
            randomTime = Random.Range(minRandomTime, maxRandomTime);
            var randomPosition = new Vector3(Random.Range(-offsetY, offsetY), generateLine.position.y,
                transform.position.y);
            Instantiate(platformList[Random.Range(0, platformList.Count)], randomPosition, Quaternion.identity);
            generatePlatformCount++;
        }
    }

    /**
     * 暂停游戏
     */
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }


    /**
     * 显示游戏面板
     */
    public static void ShowGamePanel()
    {
        _instance.scoreText.text = _instance._playerController.scorePoint.ToString(CultureInfo.InvariantCulture);
        _instance.gamePanel.SetActive(true);
        PauseGame();
    }

    /**
     * 继续游戏
     */
    public void ResumeGame()
    {
        Debug.Log("ResumeGame");
        Time.timeScale = 1;
        //如果玩家已dead 重新开始游戏
        if (_playerController.dead)
        {
            RestartGame();
        }
        _instance.gamePanel.SetActive(false);
    }

    /**
     * 重新开始游戏
     */
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * 退出游戏
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}