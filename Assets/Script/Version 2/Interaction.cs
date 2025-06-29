using UnityEngine;

namespace Assets.Version2
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        [SerializeField] private float m_basePoint = 3f;
        [SerializeField] private float m_currentPoint;
        [SerializeField] private float m_baseCD = 2.5f;
        [SerializeField] private float m_currentCD;
        [SerializeField] private float m_range = 2f;

        public float CurrentCD => m_currentCD;
        public float Range => m_range;

        public void SetTarget(Transform target) => m_target = target;

        //Trigger by Animation event in Infantry_atk
        public void OnExecuteAttack()
        {
            if (m_target != null && m_target.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(m_currentPoint);
                m_currentCD = m_baseCD;
            }

            m_target = null;
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