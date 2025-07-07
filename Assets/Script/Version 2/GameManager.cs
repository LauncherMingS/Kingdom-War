using UnityEngine;

namespace Assets.Version2
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [Header("Group Layer")]
        [SerializeField] private int m_SYWS;
        [SerializeField] private int m_NLI;

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

        protected override void Awake()
        {
            base.Awake();

            m_SYWS = LayerMask.NameToLayer("SYWS");
            m_NLI = LayerMask.NameToLayer("NLI");
        }
    }
}