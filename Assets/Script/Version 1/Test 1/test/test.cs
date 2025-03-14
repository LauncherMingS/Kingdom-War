using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int TotalNum = 0;//�h�L���`�ƶq import
    private int RowNum;//����C��
    private float TeamGap = 3;//��C����������Z
    private float FieldSize = 4;//���쪺�j�p(�e)
    [SerializeField]GameObject TestCube;//�h�L����(���ե�)
    private List<GameObject> TestList = new List<GameObject>();//�Ҧ����h�L����
    private void Start()//����ReLineUp���\��O�_�ŦX�ĪG
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
    private void Deploy()//�۶�
    {
        GameObject gObj = Instantiate(TestCube,transform.position,transform.rotation);
        TestList.Add(gObj);
        TotalNum++;
        LineUp();
        RowNum = TotalNum / 4 + 1;
    }
    void LineUp()//�۶Ү�Ĳ�o�C�u�ƦC�ثe�ӦC���h�L
    {
        float RowPerNum = TotalNum - (RowNum - 1) * 4;//�ثe�n�ƦC���ӦC�h�L�ƶq
        for (int i = (RowNum - 1)*4;i < TotalNum;i++)
        {
            float PosX = -11 - TeamGap * RowNum;//���k
            float PosZ = (i + 1) % 4 / (RowPerNum + 1) * FieldSize;//�e��(�`�L)
            if ((i + 1)% 4 == 0) PosZ = 4 / (RowPerNum + 1) * FieldSize;
            //�h�L�ƶq��n���|�H�ɡA��PosZ��4/5���W����j�p
            Vector3 Pos = new Vector3(PosX, 3.15f, PosZ);
            StartCoroutine(MoveToPosition(TestList[i], Pos));//�Ϥh�L���󲾰ʨ�p��n���y��
            print(PosZ);
        }
    }
    void ReLineUp()//�������O�P�h�L�}�`��Ĳ�o�C�������s�ƦC
    {
        float totalNum;//�ΨӬ����W���ƦC���h�L�ƶq
        if (TotalNum > 0)
        {
            RowNum = 1;
            totalNum = TotalNum;
        }
        else return;
        //�p�G�S���h�L�������X
        while (totalNum > 0)
        {
            float RowPerNum;//�ثe�n�ƦC���ӦC�h�L�ƶq
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
                float PosX = -11 - TeamGap * RowNum;//���k
                float PosZ = (i + 1) % 4 / (RowPerNum + 1) * FieldSize;//�e��(�`�L)
                if ((i + 1) % 4 == 0) PosZ = 4 / (RowPerNum + 1) * FieldSize;
                //�h�L�ƶq��n���|�H�ɡA��PosZ��4/5���W����j�p
                Vector3 Pos = new Vector3(PosX, 3.15f, PosZ);
                StartCoroutine(MoveToPosition(TestList[i], Pos));//�Ϥh�L���󲾰ʨ�p��n���y��
            }
            totalNum -= RowPerNum;//�ӦC���h�L�Ʀn����O�o����
            if (totalNum > 0) RowNum++;//�p�G�٦��H�A����U�@�C
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