using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameObject start;
    [SerializeField]
    GameObject pause;
    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    GameObject pauseBtn;
    [SerializeField]
    Slider sensitivitySlider;

    [SerializeField]
    TextMeshProUGUI scoreBoardTxt;

    [SerializeField]
    Image muteBtn;
    [SerializeField]
    Sprite muteSprite;
    [SerializeField]
    Sprite unmuteSprite;
    [SerializeField]
    Sound sound;
    bool muted;

    int sensitivity;

    [SerializeField]
    TextMeshProUGUI highScoreText;
    [SerializeField]
    TextMeshProUGUI scoreTxt;

    public TextMeshProUGUI ScoreBoardTxt { get => scoreBoardTxt; }

    private void Awake()
    {
        Globals.paused = true; 
        pauseBtn.SetActive(false);
        start.SetActive(true);  
        //Start UI tutorial, touch to start game

        int mutedInt = PlayerPrefs.GetInt("Muted"); //load mute
        muted = mutedInt == 1;

        sensitivity = PlayerPrefs.GetInt("Sensitivity"); //load sensitivity ,1 move = screen width/touchsensitivity

        if (sensitivity == 0)
            sensitivity = 10; 

        if (muted)
            Mute();
    }

    public void StartGame()
    {
        Globals.paused = false;
        start.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void Pause()
    {
        Globals.paused = true;
        pause.SetActive(true);
        pauseBtn.SetActive(false);

        sensitivitySlider.value = sensitivity;
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

    public int GetSensitivity()
    {
        return sensitivity;
    }

    public void SetTouchSensitivity(System.Single sensitivity)
    {
        this.sensitivity = (int)sensitivity;
        PlayerPrefs.SetInt("Sensitivity", this.sensitivity);
    }
}
