using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject start;
    public GameObject pause;
    public GameObject gameOver;
    public GameObject pauseBtn;
    bool muted;

    public Image muteBtn;
    public Sprite muteSprite;
    public Sprite unmuteSprite;
    public Sound sound;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTxt;

    private void Awake()
    {
        Globals.paused = true;
        pauseBtn.SetActive(false);
        start.SetActive(true);

        int mutedInt = PlayerPrefs.GetInt("Muted");
        muted = mutedInt == 1;

        if (muted)
            Mute();
    }

    public void StartGame()
    {
        start.SetActive(false);
        pauseBtn.SetActive(true);
    }

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
        int highScore = SaveSystem.Load();
        if(score > highScore)
        {
            highScoreText.text = "New high score:";
            scoreTxt.text = score.ToString();
            SaveSystem.Save(score);
        }
        else
        {
            highScoreText.text = "High score:";
            scoreTxt.text = highScore.ToString();
        }
    }

    public void ToggleMute()
    {
        if (muted)
        {
            Unmute();
        }
        else
        {
            Mute();
        }
    }

    void Mute()
    {
        muted = true;
        PlayerPrefs.SetInt("Muted", 1);
        muteBtn.sprite = unmuteSprite;
        sound.Mute();
    }

    void Unmute()
    {
        muted = false;
        PlayerPrefs.SetInt("Muted", 0);
        muteBtn.sprite = muteSprite;
        sound.Unmute();
    }
}
