using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PreIntroduction : MonoBehaviour
{
    public static bool textFinished = false;
    public GameObject textPanel;
    public TextAsset textFile;
    public Text textLabel;
    public float textSpeed = 0.1f;
    void Start()
    {
        textLabel.text = "";
        textPanel.SetActive(false);
        if (!textFinished)
        {
            textPanel.SetActive(true);
        }
    }
    private void Update()
    {
        if (!textFinished)
        {
            StartCoroutine(SetTextUI());
            textFinished = true;
        }
    }
    private IEnumerator SetTextUI()
    {
        for (int i = 0;i < textFile.text.Length;i++)
        {
            textLabel.text += textFile.text[i];
            yield return new WaitForSeconds(textSpeed);
        }
        textFinished = true;
    }
    public void Skip()
    {
        textFinished = true;
        textPanel.SetActive(false);
    }
}
