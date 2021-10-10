using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates { InMenu,WaitingForStart, Active, Paused, Ended };

public class GameManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject GameField;
    public GameObject MainMenu;
    public GameObject HighScores;
    public GameObject Instructions;
    public GameStates gameState;
    [Space]
    public Button LStartButton;
    public Button RStartButton;

    public void StartGame(GameObject gamemode) {
        UI.SetActive(false);
        GameField.SetActive(true);
        gameState = GameStates.WaitingForStart;
        gamemode.SetActive(true);
        
        
        
    }

    public void EndGame()
    {
        GameField.SetActive(false);
        UI.SetActive(true);

        gameState = GameStates.InMenu;
        foreach (Transform child in GameObject.Find("GameModes").transform)
        {
            child.gameObject.SetActive(false);
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }

}
