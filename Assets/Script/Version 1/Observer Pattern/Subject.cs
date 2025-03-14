using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    public static Subject subject;
    public List<IAction> actionList;

    public int flag = 0;
    void Start()
    {
        subject = this;
        flag = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(0))
        {
            LaunchSignal(true);
            //flag = 1;
        }
        else if (Input.GetKeyUp(0))
        {
            LaunchSignal(false);
            //flag = 0;
        }
    }
    public void LaunchSignal(bool isAction)
    {
        foreach (var action in actionList)
        {
            action.Action(isAction);
        }
    }
    public void Attach(Observer observer)
    {
        actionList.Add(observer);
    }
    public void Dettach(Observer observer)
    {
        actionList.Remove(observer);
    }
}
public interface IAction 
{
    public void Action(bool isAction);
}