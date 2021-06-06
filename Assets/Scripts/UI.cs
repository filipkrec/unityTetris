using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject pause;
    public GameObject gameOver;
    public GameObject pauseBtn;

    public GameObject muteBtn;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTxt;

    public void Pause()
    {
        Globals.paused = true;
        pause.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void UnPause()
    {
        Globals.paused = false;
        pause.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose(int score)
    {
        Globals.paused = true;
        gameOver.SetActive(true);
        pauseBtn.SetActive(false);
        int highScore = 0; //TODO
        if(score > highScore)
        {
            highScoreText.text = "New high score:";
            scoreTxt.text = score.ToString();
            //TODO save score
        }
        else
        {
            highScoreText.text = "High score:";
            scoreTxt.text = highScore.ToString();
        }
    }

    public void ToggleMute()
    {

    }
}
