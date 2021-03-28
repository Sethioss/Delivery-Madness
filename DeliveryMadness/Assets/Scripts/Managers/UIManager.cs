using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;

    [Header("Animated UI Elements")]
    public GameObject gameCanvas;
    public GameObject gameOverUI;
    public GameObject levelFinishedUI;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI feedbackText;
    public Image levelProgressFill;
    public GameObject prompt;
    public GameObject overlayCam;

    [Header("Countdown manager")]
    public TextMeshProUGUI[] texts = new TextMeshProUGUI[3];
    public int[] numbers = new int[3];
    public float delay;

    private float levelLength;
    private Transform finishLine;
    private float playerPosZ;
    private GameManager gameManager;

    /*private void Awake()
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

        DontDestroyOnLoad(gameCanvas);
        ResetUI();

    }*/

    private void Start()
    {
        finishLine = GameObject.FindGameObjectWithTag("Goal").transform;
        gameManager = GameManager.instance;
        gameManager.FindObjects();
        playerPosZ = gameManager.player.transform.position.z;
        levelLength = GetAbsolute(playerPosZ) + finishLine.position.z;

        ResetUI();
        UpdateScoreUI();

    }

    public void ActivateCanva(GameObject canvaToActivate, bool deactivateOthers = false)
    {
        canvaToActivate.SetActive(true);

        if (deactivateOthers)
        {

        }
    }

    public float GetAbsolute(float number)
    {
        if(number >=0)
        {
            return number;
        }
        else
        {
            return number * -1;
        }
    }

    public void RestartLevel()
    {
        gameManager.RestartLevel();
    }

    public void LoadNextLevel()
    {
        gameManager.LoadNextScene();
    }

    public void LoadLevel(int id)
    {
        gameManager.LoadLevel(id);
    }

    public void LoadLevel(string name)
    {
        gameManager.LoadLevel(name);
    }

    public void DeActivateCanva(GameObject canvaToDeActivate)
    {
        canvaToDeActivate.SetActive(false);
    }

    public void UpdateScoreUI()
    {
        scoreUI.text = gameManager.score.ToString();
    }

    /*public void AnimateBox(Transform boxToAnimate)
    {
        if(gameManager.gameStarted)
        {
            boxToAnimate.Rotate(new Vector3(boxToAnimate.rotation.x, 50, boxToAnimate.rotation.z));
        }
    }*/

    public void SetUIProgress()
    {
        float number = GetAbsolute(gameManager.player.transform.position.z);
        float absolute;
        float finishLinePos = finishLine.position.z;

        if (number >= 0)
        {
            absolute = number;
        }

        else
        {
            absolute = number * -1;
        }

        levelProgressFill.fillAmount = (levelLength - (absolute + finishLinePos)) / levelLength;
    }

    public void ResetUI()
    {
        DeActivateCanva(gameOverUI);
        DeActivateCanva(levelFinishedUI);
    }

    public void SetTriggerAnim(string trigger, int score = 0)
    {
        if (trigger == "playFeedback")
        {
            string[] feedBacks = { "Nice!", "Great!", "Awesome!", "Amazing!", "Crashing!" };
            int index = Random.Range(0, feedBacks.Length);

            feedbackText.text = feedBacks[index] + "\n + " + score;
            GetComponent<Animator>().SetTrigger("playFeedback");
        }

        else
        {
            GetComponent<Animator>().SetTrigger(trigger);
        }
    }

    public void StartCountdown(int id)
    {
        StartCoroutine(IncrementText(texts[id], numbers[id]));
    }

    public void SetWindowTxt(int brokenWindows)
    {
        numbers[0] = brokenWindows;
    }

    public void SetMailboxTxt(int mailbox)
    {
        numbers[1] = mailbox;
    }

    public void SetHumanTxt(int human)
    {
        numbers[2] = human;
    }

    public IEnumerator IncrementText(TextMeshProUGUI text, int number)
    {
        int num = 0;
        while (num <= number)
        {
            text.text = "x " + num.ToString();
            num++;
            gameManager.audioManager.PlaySound("scoreSound");
            yield return new WaitForSeconds(delay);
        }
    }
}
