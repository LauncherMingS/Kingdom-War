using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Test : MonoBehaviour, IPointerClickHandler
{
    public Sprite sprite;
    public DialogueSystem dialogueSystem;
    public UnityEvent events;
    void Start()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Msg();
    }
    public void Msg()
    {
        Debug.Log("QAQ");
    }
}
