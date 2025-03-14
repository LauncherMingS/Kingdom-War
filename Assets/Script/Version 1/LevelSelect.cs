using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip click;

    public int index;
    public GameObject teachPanel;
    public GameObject next;
    public GameObject previous;

    public GameObject teach_1;
    public GameObject teach_2;
    private void Start()
    {
        index = 0;
    }
    public void LoadScene(string sceneName)
    {
        audioSource.clip = click;
        audioSource.Play();
        SceneManager.LoadScene(sceneName);
    }
    public static void ExitGame()
    {
        Application.Quit();
    }
    public void SetTeachPanel(int i)
    {
        index += i;
        switch (index) {
            case 0:
                previous.SetActive(false);

                teach_1.SetActive(true);
                break;
            case 1:
                previous.SetActive(true);
                next.SetActive(true);

                teach_1.SetActive(false);
                teach_2.SetActive(true);
                break;
            case 2:
                next.SetActive(false);

                teach_1.SetActive(false);
                teach_2.SetActive(false);
                break;
            default:
                Debug.Log("Error SetTeachPanel");
                break;
        }
    }
}
