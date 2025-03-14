using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class 我方士兵控制器 : MonoBehaviour
{
    public static List<GameObject> SYWS_minerList = new List<GameObject>();
    public static List<GameObject> SYWS_soldiersList = new List<GameObject>();

    public float power = 0;

    public float passTime;
    public float money;
    public Text money_text;

    public int soldierOfsum;
    public int numPerRow;
    public float space = 3f;
    public float fieldSize = 4f;
    public enum Status { attack, defend, retreat }
    public static Status status;

    //敵方控制面板
    public bool isActive = false;
    public GameObject enemyController;
    public GameObject enemyPannel;

    //面板
    public GameObject SettingBoard;

    //招募系統
    public List<UnitData> recruitSchedule = new List<UnitData>();
    public float recruitTime;
    public bool _recruiting;

    public GameObject MinerPrefab;
    public Text minerNumInSchedule_Text;
    public int minerNumInSchedule;
    public Text triggerTime_miner;
    public GameObject cancelMiner;

    public GameObject InfantryPrefab;
    public Text infantryNumInSchedule_Text;
    public int infantryNumInSchedule;
    public Text triggerTime_infantry;
    public GameObject cancelInfantry;

    //聲音播放器
    AudioSource audioSource;
    public AudioClip settingClick;
    private void Start()
    {
        passTime = 0;

        status = Status.defend;

        soldierOfsum = SYWS_soldiersList.Count;
        numPerRow = soldierOfsum / 4 + 1;

        _recruiting = false;
        money = 500;
        minerNumInSchedule_Text.text = "";
        triggerTime_miner.text = "";
        infantryNumInSchedule_Text.text = "";
        triggerTime_infantry.text = "";
        //MinerPrefab = Resources.Load<GameObject>("Prefab/Collector");
        //InfantryPrefab = Resources.Load<GameObject>("Prefab/Infantry");
        cancelMiner.SetActive(false);
        cancelInfantry.SetActive(false);
        //敵方控制面板
        enemyController = GameObject.Find("敵方兵種控制器").gameObject;

        //聲音
        audioSource = GetComponent<AudioSource>();
        settingClick = Resources.Load<AudioClip>("Audio/settingClick");
    }
    private void Update()
    {
        passTime += Time.deltaTime;

        if (passTime >= 10) //自動生成金錢
        {
            money += 25;
            passTime = 0;
        }
        money_text.text = "$ " + money.ToString(); //顯示目前持有的金錢

        if (minerNumInSchedule == 0) minerNumInSchedule_Text.text = ""; //顯示待招募的礦工數量
        else minerNumInSchedule_Text.text = minerNumInSchedule.ToString();

        if (infantryNumInSchedule == 0) infantryNumInSchedule_Text.text = ""; // 顯示待招募的步兵數量
        else infantryNumInSchedule_Text.text = infantryNumInSchedule.ToString();


        if (_recruiting)//目前招募清單有待招募的兵種
        {
            recruitTime -= Time.deltaTime; //招募所需的招募時間隨著時間減少
            if (recruitSchedule[0].unitType == "Miner") //下一個招募的兵種是礦工
            {
                triggerTime_miner.text = ((int)recruitTime).ToString(); //更新招募礦工剩餘的時間
            }
            else if (recruitSchedule[0].unitType == "Infantry") //下一個招募的兵種是步兵
            {
                triggerTime_infantry.text = ((int)recruitTime).ToString(); //更新招募步兵剩餘的時間
            }
            if (recruitTime <= 0) //如果招募時間歸0
            {
                switch (recruitSchedule[0].unitType)
                {
                    case "Miner":
                        triggerTime_miner.text = "";//清空步兵招募的時間
                        GameObject miner = Instantiate(MinerPrefab, transform);
                        SYWS_MinerScript minerScript = miner.GetComponent<SYWS_MinerScript>();
                        print(minerScript.id = "M_" + miner.GetHashCode());
                        SYWS_minerList.Add(miner);
                        recruitSchedule.RemoveAt(0);
                        minerNumInSchedule--;
                        CheckUnitInSchedule("Miner");
                        break;
                    case "Infantry":
                        GameObject gObj = Instantiate(InfantryPrefab, transform);
                        我方士兵 gObj_Scripte = gObj.GetComponent<我方士兵>();
                        gObj_Scripte.id = "I_" + gObj.GetHashCode();
                        SYWS_soldiersList.Add(gObj);
                        power += 17.5f;

                        soldierOfsum = SYWS_soldiersList.Count;
                        LineUp();
                        numPerRow = soldierOfsum / 4 + 1;

                        triggerTime_infantry.text = ""; 
                        recruitSchedule.RemoveAt(0);
                        infantryNumInSchedule--;
                        CheckUnitInSchedule("Infantry");
                        break;
                }
                _recruiting = false;
                CheckNextRecruit();
            }
        }
    }
    public void AddRecruitSchedule(UnitData unit)
    {
        audioSource.clip = settingClick;
        audioSource.Play();

        switch (unit.unitType)
        {
            case "miner":
                if (money < unit.cost)
                {
                    Debug.Log("it isn't enough to recruit miner");
                    return;
                }
                else
                {
                    money -= unit.cost;
                    minerNumInSchedule++;
                    recruitSchedule.Add(unit);
                    CheckUnitInSchedule(unit.unitType);
                    if (!_recruiting)
                    {
                        CheckNextRecruit();
                    }
                }
                break;
            case "infantry":
                if (money < unit.cost)
                {
                    Debug.Log("it isn't enough to recruit infantry");
                    return;
                }
                else
                {
                    money -= unit.cost;
                    infantryNumInSchedule++;
                    recruitSchedule.Add(unit);
                    CheckUnitInSchedule(unit.unitType);
                    if (!_recruiting)
                    {
                        CheckNextRecruit();
                    }
                }
                break;
        }
    }
    public void CancelRecruitSchedule(UnitData unit)
    {

        audioSource.clip = settingClick;
        audioSource.Play();

        for (int i = recruitSchedule.Count - 1; i >= 0; i--)
        {
            if (recruitSchedule[i].unitType == unit.unitType)
            {
                if (i == 0)
                {
                    _recruiting = false;
                    recruitSchedule.RemoveAt(i);
                    CheckNextRecruit();
                }
                else
                {
                    recruitSchedule.RemoveAt(i);
                }
                switch (unit.unitType)
                {
                    case "miner":
                        minerNumInSchedule--;
                        break;
                    case "infantry":
                        infantryNumInSchedule--;
                        break;
                }
                money += unit.cost;
                CheckUnitInSchedule(unit.unitType);
                break;
            }
        }
    }
    public void CheckNextRecruit()
    {
        if (recruitSchedule.Count > 0)
        {
            _recruiting = true;
            recruitTime = recruitSchedule[0].recruitTime;
        }
    }
    public void CheckUnitInSchedule(string unitName)
    {
        bool _inSchedule = false;
        foreach (UnitData unit in recruitSchedule)
        {
            if (unit.unitType == unitName)
            {
                _inSchedule = true;
                break;
            }
        }
        switch (unitName)
        {
            case "miner":
                cancelMiner.SetActive(_inSchedule);
                triggerTime_miner.text = "";
                break;
            case "infantry":
                cancelInfantry.SetActive(_inSchedule);
                triggerTime_infantry.text = "";
                break;
        }
    }
    public void DiedUnit()
    {
        for (int i = 0; i < SYWS_soldiersList.Count; i++)
        {
            if (SYWS_soldiersList[i].GetComponent<我方士兵>().hp <= 0)
            {
                SYWS_soldiersList.RemoveAt(i);
                power -= 17.5f;
            }
        }
        soldierOfsum = SYWS_soldiersList.Count;
        ReLineUp();
    }
    public void Attack()
    {
        audioSource.clip = settingClick;
        audioSource.Play();

        status = Status.attack;
        foreach (GameObject gObj in SYWS_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("SYWS");
            gObj.GetComponent<我方士兵>().NLI_enemyTransform = null;
        }
    }
    public void Defend()
    {
        audioSource.clip = settingClick;
        audioSource.Play();

        status = Status.defend;
        ReLineUp();
        foreach (GameObject gObj in SYWS_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("SYWS");
            gObj.GetComponent<我方士兵>().NLI_enemyTransform = null;
        }
    }
    public void Retreate()
    {
        audioSource.clip = settingClick;
        audioSource.Play();

        status = Status.retreat;
        foreach (GameObject gObj in SYWS_soldiersList)
        {
            gObj.layer = LayerMask.NameToLayer("retreat");
            gObj.GetComponent<我方士兵>().NLI_enemyTransform = null;
        }
    }
    void LineUp()//招募時觸發。只排列目前該列的士兵
    {
        float soldiersPerrow = soldierOfsum - (numPerRow - 1) * 4;//目前要排列的該列士兵數量
        for (int i = (numPerRow - 1) * 4; i < soldierOfsum; i++)
        {
            float PosX = -11 - space * numPerRow;//左右
            float PosZ = (i + 1) % 4 / (soldiersPerrow + 1) * fieldSize;//前後(深淺)
            if ((i + 1) % 4 == 0) PosZ = 4 / (soldiersPerrow + 1) * fieldSize;
            //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
            Vector3 Pos = new Vector3(PosX, 0, PosZ);
            SYWS_soldiersList[i].GetComponent<我方士兵>().defendSpot = Pos;
            //StartCoroutine(MoveToPosition(soldierObjectlist[i], Pos));//使士兵物件移動到計算好的座標
        }
    }
    void ReLineUp()//切換指令與士兵陣亡時觸發。全部重新排列
    {
        float _count;//用來紀錄上未排列的士兵數量
        if (soldierOfsum > 0)
        {
            numPerRow = 1;
            _count = soldierOfsum;
        }
        else return;
        //如果沒有士兵直接跳出
        while (_count > 0)
        {
            float soldierPerRow;//目前要排列的該列士兵數量
            if (_count > 4)
            {
                soldierPerRow = 4;
            }
            else
            {
                soldierPerRow = _count;
            }
            for (int i = (numPerRow - 1) * 4; i < (numPerRow - 1) * 4 + soldierPerRow; i++)
            {
                float PosX = -11 - space * numPerRow;//左右
                float PosZ = (i + 1) % 4 / (soldierPerRow + 1) * fieldSize;//前後(深淺)
                if ((i + 1) % 4 == 0) PosZ = 4 / (soldierPerRow + 1) * fieldSize;
                //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
                Vector3 Pos = new Vector3(PosX, 0, PosZ);
                SYWS_soldiersList[i].GetComponent<我方士兵>().defendSpot = Pos;
                //StartCoroutine(MoveToPosition(soldierObjectlist[i], Pos));//使士兵物件移動到計算好的座標
            }
            _count -= soldierPerRow;//該列的士兵排好之後記得扣掉
            if (_count > 0) numPerRow++;//如果還有人，換到下一列
        }
    }
    public void EnemyPannel() //Resume
    {
        audioSource.clip = settingClick;
        audioSource.Play();

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
    public void GUI_LoadInScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}