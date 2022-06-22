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

    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        startPanel = GameObject.Find("StartPanel");
        endLevelPanel = GameObject.Find("EndLevelPanel");
        gamePanel = GameObject.Find("GamePanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        LoadData();
        startPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
        endLevelPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        startPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
    }

    public void ReplayLevel()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        LoadSameScene();
    }

    public void NextLevel()
    {
        gamePanel.SetActive(false);
        endLevelPanel.SetActive(true);
        LoadNextScene();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level == 2)
        {
            level = 0;
        }
        SaveData();
        SceneManager.LoadScene(level);
    }

    IEnumerator LoadSameScene()
    {
        yield return new WaitForSeconds(2f);
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level == 2)
        {
            level = 0;
        }
        SaveData();
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        SaveData();
        Application.Quit();
    }

    public void SettingsButton()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void VibrationButton()
    {
        if (settingsPanel.gameObject.GetComponentInChildren<Scrollbar>().value>0.5f)
        {
            vibrationIsActive = true;
        }
        else
        {
            vibrationIsActive = false;
        }
        SaveData();
    }

    public void CloseButton()
    {
        SaveData();
        settingsPanel.gameObject.SetActive(false);
    }

    private void SaveData()
    {
        if (vibrationIsActive)
        {
            PlayerPrefs.SetInt("VibrationIsActive", 1);
        }
        else
        {
            PlayerPrefs.SetInt("VibrationIsActive", 0);
        }
    }

    private void LoadData()
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
    }

}
