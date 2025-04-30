using UnityEngine;

namespace Assets.Version2
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [Header("Group Layer Mask")]
        [SerializeField] private int m_SYWS;
        [SerializeField] private int m_NLI;

        public int SYWS => m_SYWS;
        public int NLI => m_NLI;

        protected override void Awake()
        {
            base.Awake();

            m_SYWS = LayerMask.GetMask("SYWS");
            m_NLI = LayerMask.GetMask("NLI");
        }
    }
}