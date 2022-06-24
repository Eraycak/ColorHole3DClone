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
    private Slider levelSlider;
    public bool vibrationIsActive;
    private int levelNumber;
    private int goldNumber = 0;// goldNumber which player has //it is assigned 0 for first play time
    private int levelNumberCurrent = 0;//current level number which player is //it is assigned 0 for first play time
    private float eatableCount = 0;//eatableObjects count in the first area
    private float eatableCount2 = 0;//eatableObjects count in the second area
    private float keepEatableCount = 0;//eatableObjects count in the first area when game is started
    private float keepEatableCount2 = 0;//eatableObjects count in the second area when game is started
    private TextMeshProUGUI goldNumberText;
    private TextMeshProUGUI levelNumberCurrentText;
    private TextMeshProUGUI levelNumberNextText;

    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
        startPanel = GameObject.Find("StartPanel");
        endLevelPanel = GameObject.Find("EndLevelPanel");
        gamePanel = GameObject.Find("GamePanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        goldNumberText = GameObject.Find("GoldNumberText").GetComponent<TextMeshProUGUI>();
        levelNumberCurrentText = GameObject.Find("LevelNumberCurrentText").GetComponent<TextMeshProUGUI>();
        levelNumberNextText = GameObject.Find("LevelNumberNextText").GetComponent<TextMeshProUGUI>();
        eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
        eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
        keepEatableCount = eatableCount;
        keepEatableCount2 = eatableCount2;
        LoadData();//loads saved data from playerPrefs
        startPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
        endLevelPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        goldNumberText.text = "" + goldNumber;
        levelNumberCurrentText.text = "" + levelNumberCurrent;
        levelNumberNextText.text = "" + (levelNumberCurrent + 1);
        levelSlider = (Slider)GameObject.FindObjectsOfType(typeof(Slider))[0];
        levelSlider.value = 0;
    }

    private void Update()
    {
        if (gamePanel.activeInHierarchy)
        {
            if (eatableCount != 0)
            {
                levelSlider.value = 0.5f - ((eatableCount / keepEatableCount) / 2f);
                eatableCount = GameObject.FindGameObjectsWithTag("EatableObjects").Length;
            }
            else if (eatableCount2 != 0 && eatableCount == 0)
            {
                levelSlider.value = 1f - ((eatableCount2 / keepEatableCount2) / 2f);
                eatableCount2 = GameObject.FindGameObjectsWithTag("EatableObjects2").Length;
            }
        }
    }

    public void PlayGame()//if player touch play button, active panels change
    {
        startPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);
        levelNumberCurrentText = GameObject.Find("LevelNumberCurrentText").GetComponent<TextMeshProUGUI>();
        levelNumberNextText = GameObject.Find("LevelNumberNextText").GetComponent<TextMeshProUGUI>();
        levelNumberCurrentText.text = "" + levelNumberCurrent;
        levelNumberNextText.text = "" + (levelNumberCurrent + 1);
        levelSlider = (Slider)GameObject.FindObjectsOfType(typeof(Slider))[0];
    }

    public void ReplayLevel()//if player eats nonEatableObjects, level restarts and active panels change
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        StartCoroutine(LoadSameScene());
    }

    public void NextLevel()//if player finishes level, level and active panels changes
    {
        gamePanel.SetActive(false);
        endLevelPanel.SetActive(true);
        goldNumber += 100;
        levelNumberCurrent++;
        StartCoroutine(LoadNextScene());
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
        PlayerPrefs.SetInt("GoldNumber", goldNumber);
        PlayerPrefs.SetInt("LevelNumberCurrent", levelNumberCurrent);
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
        goldNumber = PlayerPrefs.GetInt("GoldNumber");
        levelNumberCurrent = PlayerPrefs.GetInt("LevelNumberCurrent");
    }

}
