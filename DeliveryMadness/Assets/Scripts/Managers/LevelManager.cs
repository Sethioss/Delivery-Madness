using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public float originalSpeed;

    [Header("Level rotation manager")]
    public float levelSpeed;
    public Transform level;
    public float speed;

    public int destroyedWindows;
    public int mailbox;
    public int human;

    private float delay = 0;
    private float speedToChangeTo;
    private bool changeLevelSpeed;
    private Vector3 levelMovement;
    private GameManager gameManager;

    private void Start()
    {
        originalSpeed = levelSpeed;
        delay = 0;
        levelMovement = new Vector3(0, 0, -levelSpeed);
        gameManager = GameManager.instance;
        gameManager.FindObjects();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted && !gameManager.gameOver)
        {
            MoveLevel();
        }

        if (changeLevelSpeed)
        {
            ChangeLevelSpeed(speedToChangeTo);
        }
    }

    public void MoveLevel()
    {
        level.transform.Translate(levelMovement * Time.deltaTime);
    }

    public void ChangeLevelSpeed(float desiredSpeed)
    {
        float targetSpeed = originalSpeed;
        speedToChangeTo = desiredSpeed;

        if (delay <= speed)
        {
            changeLevelSpeed = true;

            float ratio = delay / speed;

            levelSpeed = Mathf.Lerp(originalSpeed, desiredSpeed, ratio);
            delay += Time.deltaTime;
            levelMovement = new Vector3(0, 0, -levelSpeed);
        }

        else
        {
            changeLevelSpeed = false;
            delay = 0;
        }
    }
}
