using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject dialogPannel;             //��ܭ��O
    public Text character;                      //������ܤ�r
    public Text textLabel;                      //��ܮ���ܤ�r
    public Image leftImage;                     
    public Animation anim;                      

    public GameObject victoryMenu, defeatMenu;
    public string defeatedGroup;

    public GameObject debugImage;
    [Header("�奻���")]
    public TextAsset currentFile;//�nŪF��������
    public int index;//���
    public float textSpeed = 0.1f;//���
    public bool isFinishedDialogue;
    public bool textFinished;//�@�q�y�l�O�_��ܧ����A�D�@������ӹ��
    public bool isCancelTyping;


    public TextAsset startLog;
    public TextAsset endLog;
    public TextAsset startLog2;
    public int c;//��startLog2�u����@�����p��q
    public TextAsset episodeLog;

    [Header("��ø")]
    public Sprite prince, infantryLeader, infantry, priest, commander, archer, villager, businessman;


    public List<string> textList = new List<string>();//�N���ɥH�^������
    //�H�� --> �@��
    //�H��������...... --> �@��

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
        if (Input.GetMouseButtonDown(0) && index == textList.Count)//��̫�@��
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
            string splitline = line.Substring(0, line.Length - 1);//�O�o�N�^���M���A�s�JList
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
            case "���l":
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
            case "�B�L�Ϊ�":
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
            case "�B�L":
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
            case "���q":
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
            case "�]��":
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
            case "�}�b��":
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
            case "����":
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
            case "�����ӤH":
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