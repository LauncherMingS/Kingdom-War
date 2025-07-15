using UnityEngine;

namespace Assets.Version2
{
    public abstract class InteractHandler : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] protected Transform m_target;

        [Header("Parameter")]
        [Header("Attack/Heal Point")]
        [SerializeField] protected float m_basePoint = 3f;
        [SerializeField] protected float m_currentPoint;
        [Header("Attack/Heal Cool Down")]
        [SerializeField] protected float m_baseCD = 2.5f;
        [SerializeField] protected float m_currentCD;
        [Header("Attack/Heal Range")]
        [SerializeField] protected float m_range = 2f;

        public float CurrentCD => m_currentCD;

        public float Range => m_range;

        public virtual Transform Target
        {
            get => m_target;
            set => m_target = value;
        }


        protected virtual void ClearTarget()
        {
            m_target = null;
        }

        public void EnterColdDown()
        {
            m_currentCD = m_baseCD;
        }

        public void UpdateCD(float deltaTime)
        {
            m_currentCD = Mathf.Max(m_currentCD - deltaTime, 0f);
        }

        public virtual void Initialize()
        {
            m_currentPoint = m_basePoint;
            m_currentCD = 0f;

            GetComponent<Unit>().OnUpdateCD += UpdateCD;
        }

        public virtual void UnInitialize()
        {
            GetComponent<Unit>().OnUpdateCD -= UpdateCD;
        }
    }
}