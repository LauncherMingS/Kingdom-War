using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //用套的*****
    [Header("Player's info")]
    public int money = 500;
    public Text money_Text;
    public int totalCombatValue = 0;

    [Header("Group")]
    public string group;
    public Controller enemyController;//*****

    [Header("Recruit Schedule")]
    public List<UnitData> unitSchedule;//排程
    public bool recruiting = false;
    public float recruitCD = 0;//招募目前單位所需的時間

    [Header("Collector")]
    public CollectorData collectorData;//欲實作物件的資料*****
    public int collectorNum_Schedule = 0;//在排程裡，該單位目前的招募數量
    public Text collectorNum_Schedule_Text;//*****
    public Text collector_CD_Text;//招募該單位目前所需的時間*****
    public GameObject collector_Cancel_Btn;//取消招募該單位的按鈕*****

    [Header("Infantry")]
    public InfantryData infantryData;
    public int infantryNum_Schedule = 0;
    public Text infantryNum_Schedule_Text;//*****
    public Text infantry_CD_Text;//*****
    public GameObject infantry_Cancel_Btn;//*****

    [Header("Archer")]
    public ArcherData archerData;
    public int archerNum_Schedule = 0;
    public Text archerNum_Schedule_Text;//*****
    public Text archer_CD_Text;//*****
    public GameObject archer_Cancel_Btn;//*****

    [Header("Priest")]
    public PriestData priestData;
    public int priestNum_Schedule = 0;
    public Text priestNum_Schedule_Text;//*****
    public Text priest_CD_Text;//*****
    public GameObject priest_Cancel_Btn;//*****

    [Header("Commander")]
    public CommanderData commanderData;
    public enum Order
    {
        Attack,
        Defend,
        Retreat
    }
    public Order order = Order.Defend;
    public List<Unit> collectorList;
    public List<Unit> unitList;

    [Header("Def Positon Setting")]
    public int unitPerRow = 4;
    public float beginX = 8f;
    public float distanceX = 3f;
    public float distanceZ = 4f;
    private void Start()
    {
        money = 500;
        totalCombatValue = 1;
        group = gameObject.name.Replace("_Controller", "");

        unitSchedule = new List<UnitData>();
        recruiting = false;
        recruitCD = 0;

        collectorNum_Schedule = 0;
        collector_Cancel_Btn.SetActive(false);

        infantryData = Resources.Load<InfantryData>("ScriptObject/" + group + "/" + group + "_InfantryData");
        infantryNum_Schedule = 0;
        infantry_Cancel_Btn.SetActive(false);

        archerData = Resources.Load<ArcherData>("ScriptObject/" + group + "/" + group + "_ArcherData");
        archerNum_Schedule = 0;
        archer_Cancel_Btn.SetActive(false);

        priestData = Resources.Load<PriestData>("ScriptObject/" + group + "/" + group + "_PriestData");
        priestNum_Schedule = 0;
        priest_Cancel_Btn.SetActive(false);

        order = Order.Defend;
        collectorList = new List<Unit>();
        unitList = new List<Unit>();
        SetOrder(order.ToString());

        unitPerRow = 4;
        beginX = 8f;
        distanceX = 3f;
        distanceZ = 4f;

        money_Text.text =
        collectorNum_Schedule_Text.text =
        collector_CD_Text.text =
        infantryNum_Schedule_Text.text =
        infantry_CD_Text.text = "";
        archerNum_Schedule_Text.text =
        archer_CD_Text.text =
        priestNum_Schedule_Text.text =
        priest_CD_Text.text = "";
    }
    private void Update()
    {
        money_Text.text = "$" + money.ToString();
        if (recruiting) Recruiting();
    }
    public void RenewText(Text text, int num)
    {
        text.text = num.ToString();
        if (num <= 0) text.text = "";
    }
    public void AutoGenerateMoney(int getMoney)
    {
        money += getMoney;
    }
    public void Recruiting()//計算當前招募的CD並更新Text
    {
        recruitCD -= Time.deltaTime;
        switch (unitSchedule[0].unitType)
        {
            case "collector":
                RenewText(collector_CD_Text, (int)(recruitCD));
                InstantiateUnit(collectorData);
                break;
            case "infantry":
                RenewText(infantry_CD_Text, (int)(recruitCD));
                InstantiateUnit(infantryData);
                break;
            case "archer":
                RenewText(archer_CD_Text, (int)(recruitCD));
                InstantiateUnit(archerData);
                break;
            case "priest":
                RenewText(priest_CD_Text, (int)(recruitCD));
                InstantiateUnit(priestData);
                break;
            case "commander":
                Debug.Log("Commander Debut");
                InstantiateUnit(commanderData);
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
    public void InstantiateUnit(UnitData unitData)
    {
        if (recruitCD <= 0)
        {
            switch (unitData.unitType)
            {
                case "collector":
                    collectorList.Add(new Collector(collectorData, this.gameObject));
                    RenewText(collectorNum_Schedule_Text, --collectorNum_Schedule);
                    break;
                case "infantry":
                    UnitAddByPriority(new Infantry(infantryData, this.gameObject));
                    RenewText(infantryNum_Schedule_Text, --infantryNum_Schedule);
                    ReLineUp();
                    break;
                case "archer":
                    UnitAddByPriority(new Archer(archerData, this.gameObject));
                    RenewText(archerNum_Schedule_Text, --archerNum_Schedule);
                    ReLineUp();
                    break;
                case "priest":
                    UnitAddByPriority(new Priest(priestData, this.gameObject));
                    RenewText(priestNum_Schedule_Text, --priestNum_Schedule);
                    ReLineUp();
                    break;
                case "commander":
                    UnitAddByPriority(new Commander(commanderData, this.gameObject));
                    ReLineUp();
                    Debug.Log("Commander Go Straight!!!");
                    break;
            }
            totalCombatValue += unitData.combatValue;
            unitSchedule.RemoveAt(0);
            CheckUnitInSchedule(unitData);
            CheckNextRecruit();
        }
    }//當招募CD完，實作單位
    public void UnitAddByPriority(Unit unit)
    {
        unit.gameObject.name = unit.unitType + " " + unitList.Count;
        unitList.Add(unit);
        //if (unitList.Count == 0)
        //{
        //    unitList.Add(unit);
        //}
        //else
        //{
        //    for (int i = unitList.Count - 1; i >= 0; i++)
        //    {
        //        if (unit.priority >= unitList[i].priority)
        //        {
        //            if (i == unitList.Count - 1)
        //            {
        //                unitList.Add(unit);
        //                break;
        //            }
        //            else if (i == 0)
        //            {
        //                unitList.Insert(i, unit);
        //                break;
        //            }
        //            else
        //            {
        //                unitList.Insert(i + 1, unit);
        //                break;
        //            }
        //        }
        //    }
        //}
    }
    public void AddInSchedule(UnitData unitData)
    {
        if (money < unitData.cost)
        {
            print("it isn't enough to recruit " + unitData.unitType);
        }
        else
        {
            switch (unitData.unitType)
            {
                case "collector":
                    RenewText(collectorNum_Schedule_Text, ++collectorNum_Schedule);
                    break;
                case "infantry":
                    RenewText(infantryNum_Schedule_Text, ++infantryNum_Schedule);
                    break;
                case "archer":
                    RenewText(archerNum_Schedule_Text, ++archerNum_Schedule);
                    break;
                case "priest":
                    RenewText(priestNum_Schedule_Text, ++priestNum_Schedule);
                    break;
            }
            money -= unitData.cost;
            unitSchedule.Add(unitData);
            CheckUnitInSchedule(unitData);
            if (!recruiting) CheckNextRecruit();
        }
    }//將單位加入排程
    public void AddInSchedule(string name)
    {
        if (commanderData != null && name.Equals("commander"))
        {
            new Commander(commanderData, this.gameObject);
        }
    }
    public void CancelRecruit(UnitData unitData)
    {
        for (int i = unitSchedule.Count - 1;i >= 0;i--)
        {
            if (unitData.unitType.Equals(unitSchedule[i].unitType))
            {
                unitSchedule.RemoveAt(i);
                if (i == 0) CheckNextRecruit();
                switch (unitData.unitType)
                {
                    case "collector":
                        RenewText(collector_CD_Text, 0);
                        RenewText(collectorNum_Schedule_Text, --collectorNum_Schedule);
                        break;
                    case "infantry":
                        RenewText(infantry_CD_Text, 0);
                        RenewText(infantryNum_Schedule_Text, --infantryNum_Schedule);
                        break;
                    case "archer":
                        RenewText(archer_CD_Text, (int)(recruitCD));
                        RenewText(archerNum_Schedule_Text, --archerNum_Schedule);
                        break;
                    case "priest":
                        RenewText(priest_CD_Text, (int)(recruitCD));
                        RenewText(priestNum_Schedule_Text, --priestNum_Schedule);
                        break;
                }
                money += unitData.cost;
                CheckUnitInSchedule(unitData);
                break;
            }
        }
    }//從排程取消該單位
    private void CheckUnitInSchedule(UnitData unitData) //(string unitType)
    {
        bool _unitInSchedule = false;
        if (unitSchedule.Contains(unitData)) _unitInSchedule = true;
        //foreach(UnitData unit in unitSchedule)
        //{
        //    if (unit.unitType == unitData)
        //    {
        //        _unitInSchedule = true;
        //        break;
        //    }
        //}
        switch (unitData.unitType)
        {
            case "collector":
                Debug.Log(_unitInSchedule);
                collector_Cancel_Btn.SetActive(_unitInSchedule);
                break;
            case "infantry":
                infantry_Cancel_Btn.SetActive(_unitInSchedule);
                break;
            case "archer":
                archer_Cancel_Btn.SetActive(_unitInSchedule);
                break;
            case "priest":
                priest_Cancel_Btn.SetActive(_unitInSchedule);
                break;
        }
    }//確認單位是否在排程裡，並更新文字
    private void CheckNextRecruit()
    {
        if (unitSchedule.Count > 0)
        {
            recruiting = true;
            recruitCD = unitSchedule[0].recruitTime;
        }
        else
        {
            recruiting = false;
            recruitCD = 0;
        }
    }//確認排程狀況
    public void DiedUnit(string unitType)
    {
        if (unitType.Equals("collector"))
        {
            for (int i = 0;i < collectorList.Count;i++)
            {
                if (collectorList[i].currentHP <= 0) collectorList.RemoveAt(i);
            }
        }
        else
        {
            for (int i = 0;i < unitList.Count;i++)
            {
                if (unitList[i].currentHP <= 0) unitList.RemoveAt(i);
            }
            ReLineUp();
        }
    }//處理死亡的單位
    public void SetOrder(string order)
    {
        switch (order)
        {
            case "Attack":
                this.order = Order.Attack;
                foreach (Unit collector in collectorList)
                {
                    collector.SwitchOrder(Unit.Order.Attack);
                }
                foreach (Unit unit in unitList)
                {
                    unit.SwitchOrder(Unit.Order.Attack);
                }
                break;
            case "Defend":
                this.order = Order.Defend;
                foreach (Unit collector in collectorList)
                {
                    collector.SwitchOrder(Unit.Order.Defend);
                }
                foreach (Unit unit in unitList)
                {
                    unit.SwitchOrder(Unit.Order.Defend);
                }
                ReLineUp();
                break;
            case "Retreat":
                this.order = Order.Retreat;
                foreach (Unit collector in collectorList)
                {
                    collector.SwitchOrder(Unit.Order.Retreat);
                }
                foreach (Unit unit in unitList)
                {
                    unit.SwitchOrder(Unit.Order.Retreat);
                }
                break;
        }
    }//下令
    //private void LineUp()
    //{
    //    int _unitPerRow = unitList.Count % 4;
    //    int _teamRow = unitList.Count / 4;
    //    float _beginX = beginX;
    //    float _distanceX = distanceX;
    //    if (group.Equals("SYWS"))
    //    {
    //        _beginX *= -1;
    //        _distanceX *= -1;
    //    }
    //    if (_unitPerRow == 0)
    //    {
    //        _unitPerRow = unitPerRow;
    //        _teamRow--;
    //    }
    //    for (int i = unitList.Count - 1;i > (unitList.Count - 1) - _unitPerRow;i--)
    //    {
    //        float _posX = _beginX + _distanceX * _teamRow;
    //        float _posZ = (float)((i + 1) - (_teamRow * unitPerRow)) / (_unitPerRow + 1) * distanceZ;
    //        unitList[i].defSpot.Set(_posX, 0, _posZ);
    //    }
    //}//列隊
    private void ReLineUp()
    {
        if (unitList.Count == 0) return;
        int _teamRow = 0;
        int _count = unitList.Count;
        float _beginX = beginX;
        float _distanceX = distanceX;
        if (group.Equals("SYWS"))
        {
            _beginX *= -1;
            _distanceX *= -1;
        }
        while (_count > 0)
        {
            int _unitPerRow;
            if (_count >= unitPerRow) _unitPerRow = unitPerRow;
            else _unitPerRow = _count;
            for (int i = _teamRow * unitPerRow; i < (_teamRow * unitPerRow) + _unitPerRow;i++)
            {
                float _posX = _beginX + _distanceX * _teamRow;
                float _posZ = (float)((i + 1) - (_teamRow * unitPerRow)) / (_unitPerRow + 1) * distanceZ;
                unitList[i].defSpot.Set(_posX, 0, _posZ);
            }
            _count -= _unitPerRow;
            _teamRow++;
        }
    }//整隊
}