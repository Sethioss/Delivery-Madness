using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public UIManager _UIManager;
    [HideInInspector]
    public CameraController cameraController;
    [HideInInspector]
    public LevelManager levelManager;
    [HideInInspector]
    public GameObject level;
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;

    private static GameManager Instance;
    public static GameManager instance
    {
        get
        {
            return Instance;
        }
    }


    public int score = 0;
    public int overallScore = 0;

    public bool gameStarted = false;
    public bool gameOver = false;
    public bool gameWon = false;
    public bool bonus = false;

    public float lostCooldown = 2f;
    private bool gameOverScreenOn = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        _UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        level = GameObject.FindGameObjectWithTag("Level");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (gameWon)
        {
            _UIManager.SetTriggerAnim("levelFinished");
        }

        if (gameOver && lostCooldown > 0)
        {
            lostCooldown -= Time.deltaTime;
        }

        if (lostCooldown <= 0 && !gameOverScreenOn)
        {
            gameOverScreenOn = true;
            _UIManager.SetTriggerAnim("gameOver");
        }

        _UIManager.SetUIProgress();
    }

    public void FindObjects()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        if (_UIManager == null)
        {
            _UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }

        if (cameraController == null)
        {
            cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        }

        if (levelManager == null)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }

        if (level == null)
        {
            level = GameObject.FindGameObjectWithTag("Level");
        }

        if (audioManager == null)
        {
            audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
    }

    public void RestartLevel()
    {
        ResetGameManager();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopGame(bool won = false)
    {
        gameOver = true;
        gameStarted = false;

        if (won)
        {
            _UIManager.SetTriggerAnim("levelFinished");
        }

        else
        {
            audioManager.PlaySound("bicycleFalling");
            playerController.RagdollPlayer();
            levelManager.levelSpeed = 0;
        }
    }

    public void ChangeScore(int addition)
    {
        score += addition;
        overallScore += addition;
        if (score < 0)
        {
            score = 0;
        }
        _UIManager.UpdateScoreUI();
    }

    public void LoadNextScene()
    {
        ResetGameManager();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int id)
    {
        ResetGameManager();
        SceneManager.LoadScene(id);
    }

    public void LoadLevel(string name)
    {
        ResetGameManager();
        SceneManager.LoadScene(name);
    }

    private void ResetGameManager()
    {
        gameStarted = false;
        gameWon = false;
        bonus = false;
        gameOver = false;
        gameOverScreenOn = false;
        lostCooldown = 2f;
        score = 0;

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        FindObjects();
    }
}
