using UnityEngine;

namespace Assets.Version2
{
    public class AttackHandler : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private Transform m_target;

        [Header("Attack Point")]
        [SerializeField] private float m_basePoint = 3f;
        [SerializeField] private float m_currentPoint;
        [Header("Attack Cool Down")]
        [SerializeField] private float m_baseCD = 2.5f;
        [SerializeField] private float m_currentCD;
        [Header("Attack Range")]
        [SerializeField] private float m_range = 2f;

        public float CurrentPoint => m_currentPoint;
        public float CurrentCD => m_currentCD;
        public float Range => m_range;

        public void SetTarget(Transform target) => m_target = target;

        //Trigger by Animation event in Infantry_atk
        public void OnExecuteAttack()
        {
            if (m_target != null && m_target.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(m_currentPoint);
                EnterCoolDown();
            }

            m_target = null;
        }

        public void EnterCoolDown()
        {
            m_currentCD = m_baseCD;
        }

        public void UpdateCD(float deltaTime)
        {
            m_currentCD = Mathf.Max(m_currentCD - deltaTime, 0f);
        }

        public void Initialize()
        {
            m_currentPoint = m_basePoint;
            m_currentCD = 0f;
        }
    }
}