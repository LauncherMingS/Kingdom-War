using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] PlayerData data;

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public float hp;
        public int level;
    }
}