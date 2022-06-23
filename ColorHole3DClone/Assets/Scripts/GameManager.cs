using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private GameObject gameOverPanel;
    private GameObject endLevelPanel;
    private GameObject startPanel;
    private GameObject gamePanel;
    private GameObject settingsPanel;
    public bool vibrationIsActive;
    private int levelNumber;

    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        startPanel = GameObject.Find("StartPanel");
        endLevelPanel = GameObject.Find("EndLevelPanel");
        gamePanel = GameObject.Find("GamePanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        LoadData();//loads saved data from playerPrefs
        startPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
        endLevelPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
    }

    public void PlayGame()//if player touch play button, active panels change
    {
        startPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
    }

    public void ReplayLevel()//if player eats nonEatableObjects, level restarts and active panels change
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        LoadSameScene();
    }

    public void NextLevel()//if player finishes level, level and active panels changes
    {
        gamePanel.SetActive(false);
        endLevelPanel.SetActive(true);
        LoadNextScene();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);//waits a little bit to show endLevelPanel to player
        levelNumber = SceneManager.GetActiveScene().buildIndex + 1;//loads next level
        if (levelNumber == 2)//resets level no for creating infinite loop of levels
        {
            levelNumber = 0;
        }
        SaveData();//saves data to playerPrefs
        SceneManager.LoadScene(levelNumber);
    }

    IEnumerator LoadSameScene()
    {
        yield return new WaitForSeconds(2f);//waits a little bit to show gameOverPanel to player
        levelNumber = SceneManager.GetActiveScene().buildIndex;
        SaveData();
        SceneManager.LoadScene(levelNumber);
    }

    public void QuitGame()
    {
        SaveData();
        Application.Quit();
    }

    public void SettingsButton()//if player touches settingButton, settingPanels is activated
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void VibrationButton()
    {
        if (settingsPanel.gameObject.GetComponentInChildren<Scrollbar>().value>0.5f)//changes vibration according to button
        {
            vibrationIsActive = true;
        }
        else
        {
            vibrationIsActive = false;
        }
        SaveData();
    }

    public void CloseButton()//closes settingsPanel
    {
        SaveData();
        settingsPanel.gameObject.SetActive(false);
    }

    private void SaveData()//saves variables
    {
        if (vibrationIsActive)
        {
            PlayerPrefs.SetInt("VibrationIsActive", 1);
        }
        else
        {
            PlayerPrefs.SetInt("VibrationIsActive", 0);
        }
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
    }

    private void LoadData()//loads variables
    {
        if (PlayerPrefs.GetInt("VibrationIsActive") == 1)
        {
            vibrationIsActive = true;
            settingsPanel.gameObject.GetComponentInChildren<Scrollbar>().value = 1;
        }
        else
        {
            vibrationIsActive = false;
            settingsPanel.gameObject.GetComponentInChildren<Scrollbar>().value = 0;
        }
        levelNumber = PlayerPrefs.GetInt("LevelNumber");
    }

}
