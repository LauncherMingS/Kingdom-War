using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject dialogPannel;             //對話面板
    public Text character;                      //角色顯示文字
    public Text textLabel;                      //對話框顯示文字
    public Image leftImage;                     
    public Animation anim;                      

    public GameObject victoryMenu, defeatMenu;
    public string defeatedGroup;

    public GameObject debugImage;
    [Header("文本文件")]
    public TextAsset currentFile;//要讀F取的文檔
    public int index;//行數
    public float textSpeed = 0.1f;//顯示
    public bool isFinishedDialogue;
    public bool textFinished;//一段句子是否顯示完畢，非一次的整個對話
    public bool isCancelTyping;


    public TextAsset startLog;
    public TextAsset endLog;
    public TextAsset startLog2;
    public int c;//讓startLog2只執行一次的小手段
    public TextAsset episodeLog;

    [Header("立繪")]
    public Sprite prince, infantryLeader, infantry, priest, commander, archer, villager, businessman;


    public List<string> textList = new List<string>();//將文檔以回車切分
    //人物 --> 一行
    //人物講的話...... --> 一行

    void Awake()
    {
        currentFile = null;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level_1":
                startLog = Resources.Load<TextAsset>("Dialog/Level1_1");
                endLog = Resources.Load<TextAsset>("Dialog/Level1_2");
                break;
            case "Level_2":
                startLog = Resources.Load<TextAsset>("Dialog/Level2_1");
                endLog = Resources.Load<TextAsset>("Dialog/Level2_2");
                break;
            case "Level_3":
                startLog = Resources.Load<TextAsset>("Dialog/Level3_1");
                endLog = Resources.Load<TextAsset>("Dialog/Level3_4");
                startLog2 = Resources.Load<TextAsset>("Dialog/Level3_2");
                episodeLog = Resources.Load<TextAsset>("Dialog/Level3_3");
                break;
            default:
                Debug.Log("Error on dialogSystem");
                break;
        }
        GetTextFromFile(startLog);
        textSpeed = 0.1f;
        anim = leftImage.GetComponent<Animation>();
        c = 0;
        textFinished = true;
        dialogPannel.SetActive(true);
        leftImage.gameObject.SetActive(true);
        StartCoroutine(SetTextUI());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && index == textList.Count)//到最後一行
        {
            if (isFinishedDialogue)
            {
                FinishDialogue();
                return;
            }
            else
            {
                isFinishedDialogue = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (textFinished && !isCancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !isCancelTyping)
            {
                isCancelTyping = true;
            }
        }
    }
    public void DialogEvent(string section)
    {
        switch (section)
        {
            case "start":
                GetTextFromFile(startLog);
                break;
            case "end":
                GetTextFromFile(endLog);
                break;
            case "start2":
                GetTextFromFile(startLog);
                break;
            case "episode":
                GetTextFromFile(episodeLog);
                break;
            default:
                Debug.Log("Error");
                break;
        }
        dialogPannel.SetActive(true);
        leftImage.gameObject.SetActive(true);
        StartCoroutine(SetTextUI());
    }
    public void DialogEvent(string section, string group)
    {
        defeatedGroup = group;
        DialogEvent(section);
    }
    public void GetTextFromFile(TextAsset file)
    {
        currentFile = file;
        textList.Clear();
        index = 0;

        var lineDate = currentFile.text.Split('\n');

        foreach (var line in lineDate)
        {
            string splitline = line.Substring(0, line.Length - 1);//記得將回車清除再存入List
            textList.Add(splitline);
        }

    }
    private IEnumerator SetTextUI()
    {
        textFinished = false;
        character.text = "";
        textLabel.text = "";

        character.text = textList[index];
        switch (textList[index])
        {
            case "王子":
                try
                {
                    if (!leftImage.sprite.Equals(prince))
                    {
                        leftImage.sprite = prince;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = prince;
                    anim.Play();
                }
                index++;
                break;
            case "步兵團長":
                try
                {
                    if (!leftImage.sprite.Equals(infantryLeader))
                    {
                        leftImage.sprite = infantryLeader;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = infantryLeader;
                    anim.Play();
                }
                index++;
                break;
            case "步兵":
                try
                {
                    if (!leftImage.sprite.Equals(infantry))
                    {
                        leftImage.sprite = infantry;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = infantry;
                    anim.Play();
                }
                index++;
                break;
            case "祭司":
                try
                {
                    if (!leftImage.sprite.Equals(priest))
                    {
                        leftImage.sprite = priest;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = priest;
                    anim.Play();
                }
                index++;
                break;
            case "魔王":
                try
                {
                    if (!leftImage.sprite.Equals(commander))
                    {
                        leftImage.sprite = commander;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = commander;
                    anim.Play();
                }
                index++;
                break;
            case "弓箭手":
                try
                {
                    if (!leftImage.sprite.Equals(archer))
                    {
                        leftImage.sprite = archer;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = archer;
                    anim.Play();
                }
                index++;
                break;
            case "村民":
                try
                {
                    if (!leftImage.sprite.Equals(villager))
                    {
                        leftImage.sprite = villager;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = villager;
                    anim.Play();
                }
                index++;
                break;
            case "神秘商人":
                try
                {
                    if (!leftImage.sprite.Equals(businessman))
                    {
                        leftImage.sprite = businessman;
                        anim.Play();
                    }
                }
                catch (NullReferenceException)
                {
                    leftImage.sprite = businessman;
                    anim.Play();
                }
                index++;
                break;
            default:
                //debugImage.gameObject.SetActive(true);
                Debug.Log("error " + index);
                break;
        }

        int letter = 0;
        while (!isCancelTyping && letter < textList[index].Length)
        {
            textLabel.text += textList[index][letter++];
            yield return new WaitForSeconds(textSpeed);
        }
        if (isCancelTyping)
        {
            textLabel.text = textList[index];
        }
        else
        {
            isFinishedDialogue = true;
        }

        isCancelTyping = false;
        textFinished = true;
        index++;
    }
    public void FinishDialogue()
    {
        dialogPannel.SetActive(false);
        leftImage.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name.Equals("Level_3") && c == 0)
        {
            FindObjectOfType<CustomCamera>().moveToCenter = true;
            DialogEvent("start2");
            c++;
        }
        else if (currentFile.Equals(startLog))
        {
            FindObjectOfType<AiController>().enabled = true;
        }
        else if (currentFile.Equals(endLog))
        {
            switch (defeatedGroup)
            {
                case "NLI":
                    victoryMenu.SetActive(true);
                    Time.timeScale = 0;
                    break;
                default:
                    Debug.Log("No defeated group");
                    break;
            }
        }
        textList.Clear();
        index = 0;
    }
}