using UnityEngine;

namespace Assets.Version2
{
    public abstract class AttackHandlerBase : MonoBehaviour
    {
        [Header("Parameter")]
        [Header("Attack Point")]
        [SerializeField] protected float m_basePoint = 3f;
        [SerializeField] protected float m_currentPoint;
        [Header("Attack Cool Down")]
        [SerializeField] protected float m_baseCD = 2.5f;
        [SerializeField] protected float m_currentCD;
        [Header("Attack Range")]
        [SerializeField] protected float m_range = 2f;

        public float CurrentCD => m_currentCD;
        public float Range => m_range;


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
        }

        //Trigger by Animation event
        public abstract void OnExecuteAttack();
    }
}