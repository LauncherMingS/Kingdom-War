using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public List<Collector> collectorOccupiedList = new List<Collector>();
    public Vector3 p1, p2, p3;
    public bool b1, b2, b3;
    public Vector3 p4, p5, p6;
    public bool b4, b5, b6;

    public int storage = 3500;
    public int maxCollectorNum = 3;
    public int minerNum;
    private void Start()
    {
        storage = 3500;
        maxCollectorNum = 3;
        minerNum = collectorOccupiedList.Count;

        b1 = b2 = b3 = b4 = b5 = b6 = true;
        p1 = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z - 1);
        p2 = new Vector3(transform.position.x - 2.53f, transform.position.y, transform.position.z + 0.75f);
        p3 = new Vector3(transform.position.x + 1.912f, transform.position.y, transform.position.z - 0.35f);
        p4 = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z - 1);
        p5 = new Vector3(transform.position.x + 2.53f, transform.position.y, transform.position.z + 0.75f);
        p6 = new Vector3(transform.position.x - 1.912f, transform.position.y, transform.position.z - 0.35f);
    }
    public void UnderCollect(int miningAmount)
    {
        storage -= miningAmount;
        if (storage < 0)
        {
            Destroy(gameObject);
        }
    }
    public void AddOccupiedList(Collector c)
    {
        if (collectorOccupiedList.Count >= maxCollectorNum)//如果滿了清除該採集者的採集目標
        {
            c.resourceTransform = null;
            return;
        }
        if (c.resourceTransform == transform)//確認該採集者的目標為此
        {
            collectorOccupiedList.Add(c);//加入該資源的採集者List
            c.resourcePos = AllocatePos(c);//分配位置給採集者
        }
        if (collectorOccupiedList.Count >= maxCollectorNum)//如果採集者List滿了，設為occupied
        {
            gameObject.layer = LayerMask.NameToLayer("occupied");
        }
    }
    public void RemoveOccupiedList(Collector c)
    {
        if (collectorOccupiedList.Contains(c))
        {
            ReturnPos(c);
            collectorOccupiedList.Remove(c);
        }
        if (collectorOccupiedList.Count < maxCollectorNum)
        {
            gameObject.layer = LayerMask.NameToLayer("resource");
        }
    }
    public Vector3 AllocatePos(Collector c)
    {
        Vector3 p = Vector3.zero;
        switch (c.allyController.group)
        {
            case "SYWS":
                if (b1)
                {
                    b1 = false;
                    p = p1;
                }
                else if (b2)
                {
                    b2 = false;
                    p = p2;
                }
                else if (b3)
                {
                    b3 = false;
                    p = p3;
                }
                else Debug.Log("allocate fail");
                break;
            case "NLI":
                if (b4)
                {
                    b4 = false;
                    p = p4;
                }
                else if (b5)
                {
                    b5 = false;
                    p = p5;
                }
                else if (b6)
                {
                    b6 = false;
                    p = p6;
                }
                else Debug.Log("allocate fail");
                break;
        }
        return p;
    }
    public void ReturnPos(Collector c)
    {
        switch (c.allyController.group)
        {
            case "SYWS":
                if (c.resourcePos.Equals(p1)) b1 = true;
                else if (c.resourcePos.Equals(p2)) b2 = true;
                else if (c.resourcePos.Equals(p3)) b3 = true;
                else Debug.Log("return fail");
                break;
            case "NLI":
                if (c.resourcePos.Equals(p4)) b4 = true;
                else if (c.resourcePos.Equals(p5)) b5 = true;
                else if (c.resourcePos.Equals(p6)) b6 = true;
                else Debug.Log("return fail");
                break;
        }
        c.resourceTransform = null;
        c.resourcePos = Vector3.zero;
    }
}