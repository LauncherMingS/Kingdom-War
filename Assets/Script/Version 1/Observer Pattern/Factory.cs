using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public static Factory factory;
    public GameObject instance;
    public Subject subject;

    private void Start()
    {
        factory = this;
    }
    public void Produce()
    {
        Observer observer = Instantiate(instance).AddComponent<Observer>();
        subject.Attach(observer);
    }
}