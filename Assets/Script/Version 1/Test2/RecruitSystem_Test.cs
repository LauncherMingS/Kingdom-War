using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RecruitStatus
{
    Idle,
    Recruit
}
public class RecruitSystem_Test : MonoBehaviour
{
    public List<UnitData> recruitSchedule = new List<UnitData>();
    public static Dictionary<int, GameObject> gObj = new Dictionary<int, GameObject>();
    public static int sum;

    GameObject MinerPrefab;
    GameObject InfantryPrefab;

    public int money;
    public float recruitTime;
    public bool _recruiting;

    public GameObject cancelMiner;
    public GameObject cancelInfantry;

    public int soldierOfsum;
    public int numPerRow;
    public float gapOfTeam = 3f; //����C���Z(x)
    public float fieldSize = 4f;//����e��(z)
    private void Start()
    {
        gapOfTeam = 5;

        _recruiting = false;
        money = 500;

        MinerPrefab = Resources.Load<GameObject>("Prefab/Miner");
        InfantryPrefab = Resources.Load<GameObject>("Prefab/Infantry");

        cancelMiner.SetActive(false);
        cancelInfantry.SetActive(false);

        soldierOfsum = gObj.Count;
        numPerRow = soldierOfsum / 4 + 1;
    }
    void Update()
    {
        if (_recruiting)
        {
            recruitTime -= Time.deltaTime;
            if (recruitTime <= 0)
            {
                GameObject g;
                switch (recruitSchedule[0].unitType)
                {
                    case "Miner":
                        g = Instantiate(MinerPrefab);
                        gObj.Add(g.GetHashCode(), g);
                        recruitSchedule.RemoveAt(0);
                        CheckUnitInSchedule("Miner");
                        break;
                    case "Infantry":
                        g = Instantiate(InfantryPrefab);
                        gObj.Add(g.GetHashCode(), g);
                        recruitSchedule.RemoveAt(0);
                        CheckUnitInSchedule("Infantry");
                        break;
                }
                _recruiting = false;
                CheckNextRecruit();
                soldierOfsum = gObj.Count;
                numPerRow = soldierOfsum / 4 + 1;
                ReLineUp();
            }
        }
    }
    public void AddRecruitSchedule(UnitData unit)
    {
        switch (unit.unitType)
        {
            case "Miner":
                if (money < unit.cost)
                {
                    Debug.Log("money isn't enough");
                    return;
                }
                else
                {
                    money -= unit.cost;
                    recruitSchedule.Add(unit);
                    CheckUnitInSchedule(unit.unitType);
                    if (!_recruiting)
                    {
                        CheckNextRecruit();
                    }
                }
                break;
            case "Infantry":
                if (money < unit.cost)
                {
                    Debug.Log("money isn't enough");
                    return;
                }
                else
                {
                    money -= unit.cost;
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
            case "Miner":
                cancelMiner.SetActive(_inSchedule);
                break;
            case "Infantry":
                cancelInfantry.SetActive(_inSchedule);
                break;
        }
    }
    void ReLineUp()//�������O�P�h�L�}�`��Ĳ�o�C�������s�ƦC
    {
        int q, m, c;
        if (soldierOfsum > 0)
        {
            q = (gObj.Count / 4)*4; //��4�H���`�H��
            m = gObj.Count % 4; //�̫�@�C���h�L�ƶq
            c = 1; //
        }
        else return;
        //�p�G�S���h�L�������X
        foreach (KeyValuePair<int,GameObject> keyValue in gObj)
        {
            float PosX = -11 - gapOfTeam * ((c / 4) + 1);
            float PosZ; 
            if (c <= q) //��4�ӤH
            {
                PosZ = (c % 4) / (5 * fieldSize);
                if (c % 4 == 0) PosZ = 4 / (5 * fieldSize);
            }
            else
            {
                PosZ = (c % 4) / (m * fieldSize);
            }
            Vector3 Pos = new Vector3(PosX, 0, PosZ);
            gObj[keyValue.Key].GetComponent<�ڤ�h�L>().defendSpot = Pos;
            c++;
            print(PosX + " " + PosZ);
        }
    }
}
