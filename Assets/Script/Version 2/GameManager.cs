using UnityEngine;
using Assets.Version2.Factory;

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

        [SerializeField] private FactorySOSetting m_factorySetting;

        public int SYWS => m_SYWS;
        public int NLI => m_NLI;


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
            ScriptableObject.CreateInstance<CentralFactorySO>().Register(m_factorySetting.Factories);
        }
    }
}