using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePannel;
    public Text textLable;

    [Header("文件文本")]
    public TextAsset currentFile;
    public List<string> logs = new List<string>();
    public int index;
    public float textSpeed;
    public bool isFinishedDialogue;
    public bool isFinishedText;
    public bool isCancelTyping;

    public TextAsset startFile;
    public TextAsset endFile;

    void Start()
    {
        dialoguePannel.SetActive(true);

        currentFile = null;
        startFile = Resources.Load<TextAsset>("Dialog/Level1_1");
        endFile = Resources.Load<TextAsset>("Dialog/Level1_2");
        textSpeed = 0.1f;
        isFinishedText = false;

        ExtractLogs(startFile);
        StartCoroutine(SetLogUI());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && index == logs.Count)
        {
            if (isFinishedDialogue)
            {
                dialoguePannel.SetActive(false);
                index = 0;
                return;
            }
            else
            {
                isFinishedDialogue = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (isFinishedText && !isCancelTyping)
            {
                StartCoroutine(SetLogUI());
            }
            else if (!isFinishedText && !isCancelTyping)
            {
                isCancelTyping = true;
            }
        }
    }
    public void ExtractLogs(TextAsset file)
    {
        currentFile = file;
        logs.Clear();
        index = 0;

        string[] lineDate = file.text.Split('\n');
        foreach (string line in lineDate)
        {
            logs.Add(line);
        }
    }
    private IEnumerator SetLogUI()
    {
        isFinishedText= false;
        textLable.text = "";

        int letter = 0;
        while (!isCancelTyping && letter < logs[index].Length)
        {
            textLable.text += logs[index][letter++];
            yield return new WaitForSeconds(textSpeed);
        }
        if (isCancelTyping)
        {
            textLable.text = logs[index];
        }
        else
        {
            isFinishedDialogue = true;
        }

        isCancelTyping = false;
        isFinishedText = true;
        index++;
    }
    public void SkipLog()
    {
        
    }
}
