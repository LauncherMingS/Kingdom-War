using System;
using UnityEngine;
using Assets.Version2.Pool;

namespace Assets.Version2
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [Header("Group Layer")]
        [SerializeField] private int m_SYWS;
        [SerializeField] private int m_NLI;
        [Header("Group Controller")]
        [SerializeField] private Controller m_SYWS_Controller;
        [SerializeField] private Controller m_NLI_Controller;

        [Header("Setting")]
        [SerializeField] private ObjectPoolSettingSO m_poolSetting;

        public int SYWS => m_SYWS;
        public int NLI => m_NLI;


        //在編輯器中，才會被call
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogWarningEditor(string message) => Debug.LogWarning(message);

        public static bool EnumIsDefined<TEnum>(int enumValue, string sourceName) where TEnum : struct, Enum
        {
            bool t_result = Enum.IsDefined(typeof(TEnum), enumValue);
            if (!t_result)
            {
                LogWarningEditor($"{sourceName}: {enumValue} is not defind in {typeof(TEnum).Name}");
            }

            return t_result;
        }

        public bool IsSYWS(int ourGroupLayer)
        {
            return ourGroupLayer == m_SYWS;
        }

        public int GetOppositeGroupLayer(int ourGroupLayer)
        {
            return (ourGroupLayer == m_SYWS) ? m_NLI : m_SYWS;
        }

        public Controller GetController(int ourGroupLayer)
        {
            return (IsSYWS(ourGroupLayer)) ? m_SYWS_Controller : m_NLI_Controller;
        }

        protected override void Awake()
        {
            base.Awake();

            m_SYWS = LayerMask.NameToLayer("SYWS");
            m_NLI = LayerMask.NameToLayer("NLI");
        }

        private void Start()
        {
            ScriptableObject.CreateInstance<ObjectPoolManagerSO>().RegisterPools(m_poolSetting.Pools);
        }
    }
}