using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class 敵方士兵控制器 : MonoBehaviour
{
    public static List<GameObject> NLI_soldiersList = new List<GameObject>();
    public int minerNum = 0;
    public float power = 0;

    public float money;
    public float time;

    public int soldierOfsum;
    public int numOfrow;
    public float gapOfteam = 3f;
    public float fieldSize = 4f;

    public float cd;
    public bool isProduce;
    public GameObject miner;
    public float spend_miner = 135;
    public float time_miner = 5;
    public bool produce_miner = false;

    public GameObject soldierObject;
    public float spend_infantry = 105;
    public float time_infantry = 5;
    public bool produce_infantry = false;
    public enum Status { attack, defend, retreat }
    public static Status status;
    public Status GetStatus => status;

    //聲音撥放器
    public AudioSource audioSource;
    public AudioClip UIclick;
    private void Start()
    {
        money = 300;
        time = 0;
        cd = 0;
        isProduce = false;
        status = Status.defend;
        soldierOfsum = NLI_soldiersList.Count;
        numOfrow = soldierOfsum / 4 + 1;

        audioSource = GetComponent<AudioSource>();
        UIclick = Resources.Load<AudioClip>("Audio/UIclick");
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 5)
        {
            money += 45;
            time = 0;
        }
        if (isProduce)
        {
            cd += Time.deltaTime;
            if (cd >= time_miner && produce_miner)
            {
                minerNum++;
                Instantiate(miner, transform.position, transform.rotation);
                isProduce = produce_miner = false;
                cd = 0;
            }
            else if (cd >= time_infantry && produce_infantry)
            {
                GameObject gObj = Instantiate(soldierObject, transform.position, transform.rotation);
                gObj.transform.parent = transform;

                敵方士兵 gObj_Scripte = gObj.GetComponent<敵方士兵>();
                NLI_soldiersList.Add(gObj);
                power += 17.5f;

                soldierOfsum = NLI_soldiersList.Count;
                LineUp();
                numOfrow = soldierOfsum / 4 + 1;
                isProduce = produce_infantry = false;
                cd = 0;
            }
        }
    }
    public void Recruit_miner()
    {
        if (!isProduce && money >= spend_miner)
        {
            money -= spend_miner;
            /*minerNum++;
            Instantiate(miner, transform.position, transform.rotation);*/
            isProduce = produce_miner = true;
        }
    }
    public void Recruit()
    {
        if (!isProduce && money >= spend_infantry)
        {
            money -= spend_infantry;
            /*GameObject gObj = Instantiate(soldierObject, transform.position, transform.rotation);
            gObj.transform.parent = transform;

            敵方士兵 gObj_Scripte = gObj.GetComponent<敵方士兵>();
            NLI_soldiersList.Add(gObj);
            power += 17.5f;

            soldierOfsum = NLI_soldiersList.Count;
            LineUp();
            numOfrow = soldierOfsum / 4 + 1;*/
            isProduce = produce_infantry = true;
        }
    }
    public void DyingSoldier()
    {
        for (int i = 0; i < NLI_soldiersList.Count; i++)
        {
            if (NLI_soldiersList[i].GetComponent<敵方士兵>().hp <= 0)
            {
                NLI_soldiersList.RemoveAt(i);
                power -= 17.5f;
            }
        }
        soldierOfsum = NLI_soldiersList.Count;
        /*for (int i = id;i < SYWS_soldiersList.Count;i++)
        {
            SYWS_soldiersList[i].GetComponent<我方士兵>().id--;
            SYWS_soldiersList[i].name = "祭司" + (i + 1);
        }*///list的index與實際的站位順序不一致
        ReLineUp();
        //Reorgnize();
    }
    public void Attack()
    {
        audioSource.clip = UIclick;
        audioSource.Play();

        status = Status.attack;
        foreach (GameObject gObj in NLI_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("NLI");
            gObj.GetComponent<敵方士兵>().SYWS_enemyTransform = null;
        }
    }
    public void Defend()
    {
        status = Status.defend;
        foreach (GameObject gObj in NLI_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("NLI");
            gObj.GetComponent<敵方士兵>().SYWS_enemyTransform = null;
        }
    }
    public void Retreate()
    {
        status = Status.retreat;
        foreach (GameObject gObj in NLI_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("retreat");
            gObj.GetComponent<敵方士兵>().SYWS_enemyTransform = null;
        }
    }
    void LineUp()//招募時觸發。只排列目前該列的士兵
    {
        float soldiersPerrow = soldierOfsum - (numOfrow - 1) * 4;//目前要排列的該列士兵數量
        for (int i = (numOfrow - 1) * 4; i < soldierOfsum; i++)
        {
            float PosX = 11 + gapOfteam * numOfrow;//左右
            float PosZ = (i + 1) % 4 / (soldiersPerrow + 1) * fieldSize;//前後(深淺)
            if ((i + 1) % 4 == 0) PosZ = 4 / (soldiersPerrow + 1) * fieldSize;
            //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
            Vector3 Pos = new Vector3(PosX, 0, PosZ);
            NLI_soldiersList[i].GetComponent<敵方士兵>().defendSpot = Pos;//

            //StartCoroutine(MoveToPosition(soldierObjectlist[i], Pos));//使士兵物件移動到計算好的座標
        }
    }
    void ReLineUp()//切換指令與士兵陣亡時觸發。全部重新排列
    {
        float _count;//用來紀錄上未排列的士兵數量
        if (soldierOfsum > 0)
        {
            numOfrow = 1;
            _count = soldierOfsum;
        }
        else return;
        //如果沒有士兵直接跳出
        while (_count > 0)
        {
            float soldierPerrow;//目前要排列的該列士兵數量
            if (_count > 4)
            {
                soldierPerrow = 4;
            }
            else
            {
                soldierPerrow = _count;
            }
            for (int i = (numOfrow - 1) * 4; i < (numOfrow - 1) * 4 + soldierPerrow; i++)
            {
                float PosX = 11 + gapOfteam * numOfrow;//左右
                float PosZ = (i + 1) % 4 / (soldierPerrow + 1) * fieldSize;//前後(深淺)
                if ((i + 1) % 4 == 0) PosZ = 4 / (soldierPerrow + 1) * fieldSize;
                //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
                Vector3 Pos = new Vector3(PosX, 0, PosZ);
                NLI_soldiersList[i].GetComponent<敵方士兵>().defendSpot = Pos;
                //StartCoroutine(MoveToPosition(soldierObjectlist[i], Pos));//使士兵物件移動到計算好的座標
            }
            _count -= soldierPerrow;//該列的士兵排好之後記得扣掉
            if (_count > 0) numOfrow++;//如果還有人，換到下一列
        }
    }
    /*public void DyingSoldier(int id)
    {
        NLI_soldiersList.RemoveAt(id);
        for (int i = id; i < NLI_soldiersList.Count; i++)
        {
            NLI_soldiersList[i].GetComponent<敵方士兵>().id--;
            NLI_soldiersList[i].name = "敵方士兵"  + id;
        }
    }*/
}
