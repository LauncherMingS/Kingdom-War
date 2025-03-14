using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script
{
    public class DialogSystem2 : MonoBehaviour
    {
        [Header("文檔")]
        public TextAsset currentFile;       //臺詞來源文檔
        public string route;                //檔案路徑

        [Header("對話")]
        public IEnumerator coroutine;
        public List<string> log;            //對話，以換行為一段
        public int index;                   //偶數段為角色名稱，奇數段為臺詞
        public float scrollSpeed = 0.2f;    //顯示一個字等待的時間
        public bool isFinishedTyping;       //是否顯示完一段話
        public bool isPausedPassage;        //是否暫停對話
        public bool isSkipPassage;          //是否跳過這段話
        public bool isFinishedDialogue;     //是否完成整個對話

        [Header("UI")]
        public GameObject dialogPanel;
        public Text characterName;
        public Text passage;

        private void Awake()
        {
            currentFile = null;
            currentFile = Resources.Load<TextAsset>("Dialog/Level1_1");
            LoadText();
            coroutine = ShowOnePassage();
            StartCoroutine(coroutine);
        }
        private void LoadText()
        {
            string[] loadArr = currentFile.text.Split('\n');
            log = new List<string>();
            foreach (string text in loadArr)
            {
                log.Add(text);
            }
        }
        public void ProgressPassage()   //
        {
            if (isFinishedDialogue)
            {
                dialogPanel.SetActive(false);
                return;
            }
            else if (isFinishedTyping)
            {
                StartCoroutine(coroutine);
            }
            else if (isPausedPassage)
            {
                isPausedPassage = false;
            }
        }
        private IEnumerator ShowOnePassage()    //顯示角色的一段對話
        {
            isFinishedTyping = false;
            characterName.text = passage.text = "";
            characterName.text = log[index];
            ++index;
            for (int letter = 0; !isSkipPassage && letter < log[index].Length; ++letter)
            {
                if (isPausedPassage)
                {
                    yield return new WaitUntil(() => { return !isPausedPassage; });
                }
                else
                {
                    yield return new WaitForSeconds(scrollSpeed);
                }
                passage.text = passage.text + log[index][letter];
            }
            if (isSkipPassage)
            {
                passage.text = log[index];
            }
            ++index;
            isFinishedTyping = true;
            isSkipPassage = false;

            //判斷是否有剩餘的話尚未進行完
            if (index >= log.Count)
            {
                coroutine = null;
                isFinishedDialogue = true;
            }
            else
            {
                coroutine = ShowOnePassage();
            }
        }
        public void SkipPassage()
        {
            if (!isFinishedTyping)
            {
                isSkipPassage = true;
                isPausedPassage = false;
            }
        }
        public void PausePassage()
        {
            if (!isPausedPassage && !isFinishedTyping)
            {
                isPausedPassage = true;
            }
        }
    }
}