using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public float currentPassTime;
    public Text Text_currentPassTime_v, Text_currentPassTime_d;
    public int score;
    public Text Text_score_v, Text_score_d;

    public int generatedMoney;
    public float generatedMoneyFrequence;
    public float generatedMoneyCD;
    public Controller SYWS_Controller;
    public Controller NLI_Controller;

    public GameObject SettingBoard;
    public bool isActive;

    public DialogSystem dialogSystem;
    private void Start()
    {
        //SYWS_Controller = GameObject.Find("我方兵種控制器").GetComponent<我方士兵控制器>();
        Text_currentPassTime_v.text = Text_currentPassTime_d.text = "";
        currentPassTime = 0;
        Text_score_v.text = Text_score_d.text = "";
        score = 0;

        generatedMoney = 25;
        generatedMoneyFrequence = 10;
        generatedMoneyCD = generatedMoneyFrequence;

        isActive = false;

        dialogSystem = gameObject.GetComponent<DialogSystem>();
    }
    private void Update()
    {
        currentPassTime += Time.deltaTime;
        Text_currentPassTime_v.text = Text_currentPassTime_d.text = ((int)(currentPassTime / 60)).ToString() + ":" + ((int)(currentPassTime % 60)).ToString();
        Text_score_v.text = Text_score_d.text = score.ToString();

        generatedMoneyCD -= Time.deltaTime;
        if (generatedMoneyCD <= 0)
        {
            SYWS_Controller.AutoGenerateMoney(generatedMoney);
            NLI_Controller.AutoGenerateMoney(generatedMoney);

            generatedMoneyCD = generatedMoneyFrequence;
        }
    }
    public void Setting()
    {
        isActive = !isActive;
        if (isActive)
        {
            SettingBoard.SetActive(true);
            Time.timeScale = 0;
            //enemyPannel.SetActive(true);
            //enemyController.GetComponent<AiController>().enabled = false;
        }
        else
        {
            SettingBoard.SetActive(false);
            Time.timeScale = 1;
            //enemyPannel.SetActive(false);
            //enemyController.GetComponent<AiController>().enabled = true;
        }
    }
    public void LoadInScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
    public void CommanderDebut()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level_3"))
        {
            dialogSystem.DialogEvent("episode");
            FindObjectOfType<CustomCamera>().moveTo_NLI = true;
        }
    }
}