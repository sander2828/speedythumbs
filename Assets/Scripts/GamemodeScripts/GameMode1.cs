using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Linq;

enum CurrentTaskType {Pressone,Presstwo,none};

public class GameMode1 : MonoBehaviour
{
    // Start is called before the first frame update

    private GameManager gameManager;
    private GameObject[] taskButtons;

    private Dictionary<string, GameObject> currenttask = new Dictionary<string, GameObject>();
    private CurrentTaskType currentTaskType = CurrentTaskType.none; 

    

    public float timelimit = 120;

    private TextMeshProUGUI timertext,scoretext,highscoretext,endscoretext,endhighscoretext,newhighscoretext;
    private Transform gameend;
    private int highscore;
    
    private float timeremaining;
    private int score = 0;

    public Color TaskButtonDefaultColor;

    private void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
        taskButtons = GameObject.FindGameObjectsWithTag("TaskButton");
        Transform canvas = gameManager.GameField.transform.Find("Canvas").transform;
        Transform toparea = canvas.Find("TopArea").transform;
         gameend = canvas.Find("GameEnd");
        timertext = toparea.Find("TimerText").GetComponent<TextMeshProUGUI>();
        scoretext = toparea.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        highscoretext = toparea.Find("HighscoreText").GetComponent<TextMeshProUGUI>();

        endscoretext = gameend.Find("EndScore").GetComponent < TextMeshProUGUI>();
        endhighscoretext = gameend.Find("EndHighscore").GetComponent<TextMeshProUGUI>();
        newhighscoretext = gameend.Find("NewHighscore").GetComponent<TextMeshProUGUI>();

        foreach (GameObject child in taskButtons)
        {
            child.GetComponent<Image>().color = TaskButtonDefaultColor;
            
        }

        currenttask.Clear();
        currentTaskType = CurrentTaskType.none;


        score = 0;
        timeremaining = 0;
        timeremaining = timelimit;
        UpdateTimer();
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        highscoretext.text = "Highscore:"+highscore.ToString();
        UpdateScore();
        gameend.gameObject.SetActive(false);
        newhighscoretext.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.gameState)
        {
            case GameStates.WaitingForStart:
                LookStartCondition();
                break;
            case GameStates.Active:
                ManageGame();
                timeremaining -= Time.deltaTime;
                UpdateTimer();
                if (timeremaining <= 0)
                {
                    EndGame();
                }
                break;
            case GameStates.Paused:
                break;
            default:
                break;
        }
        
       

    }

    void LookStartCondition()
    {
        if (gameManager.LStartButton.GetComponent<checkHeldDown>().isHeldDown == true && gameManager.RStartButton.GetComponent<checkHeldDown>().isHeldDown == true)
        {
            gameManager.gameState = GameStates.Active;
        }

    }

    void ManageGame()
    {
        if (currentTaskType == CurrentTaskType.none)
        {
            assignNewTask();
        }
        else {
            CheckTaskCompletion();
        }

    }


    void EndGame()
    {
        gameManager.gameState = GameStates.Ended;

        if (score > highscore) {
            highscore = score;
            
            newhighscoretext.gameObject.SetActive(true);
            PlayerPrefs.SetInt("highscore",highscore);
        }

        endscoretext.text = score.ToString();
        endhighscoretext.text = "Highscore:" +highscore.ToString();
        gameend.gameObject.SetActive(true);
        GameObject.Find("GameModes").transform.Find("Gamemode1").gameObject.SetActive(false);
    }

    void assignNewTask()
    {
        int index = Random.Range(0, taskButtons.Length);
        Image taskbutton = taskButtons[index].GetComponent<Image>();

        int index2 = Random.Range(0, 100);

        if (index2 < 50)
        {
            taskbutton.color = gameManager.LStartButton.GetComponent<Image>().color;
            currentTaskType = CurrentTaskType.Pressone;
            currenttask.Add("Taskbutton", taskButtons[index]);
            currenttask.Add("CantBePressed",gameManager.LStartButton.gameObject);
            currenttask.Add("MustBePressed",gameManager.RStartButton.gameObject);
        }
        else { taskbutton.color = gameManager.RStartButton.GetComponent<Image>().color;
            currentTaskType = CurrentTaskType.Pressone;
            currenttask.Add("Taskbutton", taskButtons[index]);
            currenttask.Add("CantBePressed", gameManager.RStartButton.gameObject);
            currenttask.Add("MustBePressed", gameManager.LStartButton.gameObject);
        }
    }


    private void CheckTaskCompletion()
    {
        if (currenttask["Taskbutton"].GetComponent<checkHeldDown>().isHeldDown &&
           !currenttask["CantBePressed"].GetComponent<checkHeldDown>().isHeldDown &&
           currenttask["MustBePressed"].GetComponent<checkHeldDown>().isHeldDown
            ) {
            currentTaskType = CurrentTaskType.none;
            currenttask["Taskbutton"].GetComponent<Image>().color = TaskButtonDefaultColor;
            UpdateScore(100);
            currenttask.Clear();
        }
    }


    void UpdateTimer()
    {
        var ts = TimeSpan.FromSeconds(Mathf.CeilToInt(timeremaining));
        timertext.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);



    }

    void UpdateScore(int amount = 0)
    {
        score += amount;
        scoretext.text = "Score:"+score.ToString();
    }

}
