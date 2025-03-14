using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    public float hp = 750;
    public float maxhp = 750;
    public float percentage;
    public float time = 0;
    public bool countStart = false;

    public SpriteRenderer spr;
    public Sprite spr1, spr2, spr3, spr4, spr5, spr6, spr7;

    public int c;//讓出場對話只執行一次的小手段

    public GameObject defeatMenu;
    //public List<GameObject> atkUnit = new List<GameObject>();
    //public GameObject hp_bar;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        maxhp = hp = 150;
}
    private void Update()
    {
        if (countStart)
        {
            time += Time.deltaTime;
            if (time > 5)
            {
                if (gameObject.layer.Equals(LayerMask.NameToLayer("NLI")))
                {
                    GameObject.Find("GameState").GetComponent<DialogSystem>().DialogEvent("end", "NLI");
                }
                else if (gameObject.layer.Equals(LayerMask.NameToLayer("SYWS")))
                {
                    defeatMenu.SetActive(true);
                    Time.timeScale = 0;
                }
                Destroy(gameObject);
                countStart = false;
                time = 0;
            }
        }
        percentage = hp / maxhp;
    }
    public void UnderAttack(float damage)
    {
        hp -= damage;
        if (hp <= 0) spr.sprite = spr7;
        else if (percentage < 0.1f) spr.sprite = spr6;
        else if (percentage < 0.2f) spr.sprite = spr5;
        else if (percentage < 0.4f)
        {
            spr.sprite = spr4;
            if (gameObject.layer.Equals(LayerMask.NameToLayer("NLI")) && c == 0)
            {
                GameObject.Find("NLI_Controller").GetComponent<Controller>().AddInSchedule("commander");
                print("step1");
                GameObject.Find("GameState").GetComponent<GameState>().CommanderDebut();
                c++;
            }
        }
        else if (percentage < 0.6f) spr.sprite = spr3;
        else if (percentage < 0.8f) spr.sprite = spr2;
        else spr.sprite = spr1;

        if (hp <= 0)
        {
            countStart = true;
            hp = 0;
            if (gameObject.layer.Equals(LayerMask.NameToLayer("SYWS")))
            {
                Controller controller = GameObject.Find("SYWS_Controller").GetComponent<Controller>();
                controller.enabled = false;
            }
            else if (gameObject.layer.Equals(LayerMask.NameToLayer("NLI")))
            {
                Controller controller = GameObject.Find("NLI_Controller").GetComponent<Controller>();
                controller.gameObject.GetComponent<AiController>().enabled = false;
                controller.enabled = false;
            }
        }
    }
    /*public void Addin(GameObject g)
    {
        bool isExit = false;
        foreach (GameObject G in atkUnit)
        {
            if (G.GetHashCode() == g.GetHashCode()) isExit = true;
            break;
        }
        if (!isExit)
        {
            atkUnit.Add(g);
        }
    }
    public Vector3 AllocatePos()
    {
        if (atkUnit.Count == 1)
        {
            return new Vector3(18.95811f, 0.6593503f, -1.942f);
        }
        else if (atkUnit.Count == 2)
        {
            return new Vector3(18.14799f, 0.5399947f, -1.140095f);
        }
        else if (atkUnit.Count == 3)
        {
            return new Vector3(17.2269f, 0.507015f, 0.59f);
        }
        else if (atkUnit.Count == 4)
        {
            return new Vector3(16.9841f, 0.7612107f, 2.119338f);
        }
        else if (atkUnit.Count == 5)
        {
            return new Vector3(17.50495f, 0.9263996f, 4.253066f);
        }
        else return Vector3.zero;
    }*/
}
