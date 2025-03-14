using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int TotalNum = 0;//士兵的總數量 import
    private int RowNum;//隊伍的列數
    private float TeamGap = 3;//兩列隊伍之間的間距
    private float FieldSize = 4;//場域的大小(寬)
    [SerializeField]GameObject TestCube;//士兵物件(測試用)
    private List<GameObject> TestList = new List<GameObject>();//所有的士兵物件
    private void Start()//測試ReLineUp的功能是否符合效果
    {
        RowNum = 1;
        int i = 0;
        while (i < 4)
        {
            GameObject gObj = Instantiate(TestCube, transform.position, transform.rotation);
            TestList.Add(gObj);
            TotalNum++;
            i++;
        }
    }
    private void Deploy()//招募
    {
        GameObject gObj = Instantiate(TestCube,transform.position,transform.rotation);
        TestList.Add(gObj);
        TotalNum++;
        LineUp();
        RowNum = TotalNum / 4 + 1;
    }
    void LineUp()//招募時觸發。只排列目前該列的士兵
    {
        float RowPerNum = TotalNum - (RowNum - 1) * 4;//目前要排列的該列士兵數量
        for (int i = (RowNum - 1)*4;i < TotalNum;i++)
        {
            float PosX = -11 - TeamGap * RowNum;//左右
            float PosZ = (i + 1) % 4 / (RowPerNum + 1) * FieldSize;//前後(深淺)
            if ((i + 1)% 4 == 0) PosZ = 4 / (RowPerNum + 1) * FieldSize;
            //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
            Vector3 Pos = new Vector3(PosX, 3.15f, PosZ);
            StartCoroutine(MoveToPosition(TestList[i], Pos));//使士兵物件移動到計算好的座標
            print(PosZ);
        }
    }
    void ReLineUp()//切換指令與士兵陣亡時觸發。全部重新排列
    {
        float totalNum;//用來紀錄上未排列的士兵數量
        if (TotalNum > 0)
        {
            RowNum = 1;
            totalNum = TotalNum;
        }
        else return;
        //如果沒有士兵直接跳出
        while (totalNum > 0)
        {
            float RowPerNum;//目前要排列的該列士兵數量
            if (totalNum > 4)
            {
                RowPerNum = 4;
            }
            else
            {
                RowPerNum = totalNum;
            }
            for (int i = (RowNum - 1) * 4; i < (RowNum - 1) * 4 + RowPerNum; i++)
            {
                float PosX = -11 - TeamGap * RowNum;//左右
                float PosZ = (i + 1) % 4 / (RowPerNum + 1) * FieldSize;//前後(深淺)
                if ((i + 1) % 4 == 0) PosZ = 4 / (RowPerNum + 1) * FieldSize;
                //士兵數量剛好為四人時，使PosZ為4/5乘上場域大小
                Vector3 Pos = new Vector3(PosX, 3.15f, PosZ);
                StartCoroutine(MoveToPosition(TestList[i], Pos));//使士兵物件移動到計算好的座標
            }
            totalNum -= RowPerNum;//該列的士兵排好之後記得扣掉
            if (totalNum > 0) RowNum++;//如果還有人，換到下一列
        }
    }
    IEnumerator MoveToPosition(GameObject gObj,Vector3 Pos)
    {
        while (gObj.transform.position != Pos)
        {
            gObj.transform.position = Vector3.MoveTowards(gObj.transform.position, Pos, 5f * Time.deltaTime);
            yield return 0;
        }
    }
}