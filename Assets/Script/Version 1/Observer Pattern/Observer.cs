using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour, IAction
{
    public int flag = 1;
    void Update()
    {
        //int subjectFlag = Subject.subject.flag;
        //if (subjectFlag == 0)
        //{
        //    return;
        //}
        //else if (subjectFlag == flag)
        //{
        //    Action(true);
        //}
        //else
        //{
        //    Action(false);
        //}
    }

    public void Action(bool isAction)
    {
        Debug.Log((isAction) ? "¥´¶}" : "Ãö³¬");
    }
}