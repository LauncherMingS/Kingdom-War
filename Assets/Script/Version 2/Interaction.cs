using UnityEngine;

namespace Assets.Version2
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private float m_basePoint = -3f;
        [SerializeField] private float m_currentPoint;
        [SerializeField] private float m_baseCD = 2.5f;
        [SerializeField] private float m_currentCD;
        [SerializeField] private float m_range = 1.5f;

        public float CurrentCD => m_currentCD;
        public float Range => m_range;

        public void Interact(Transform target)
        {
            if (target.TryGetComponent<Health>(out Health health))
            {
                health.ModifyHealth(m_currentPoint);
                m_currentCD = m_baseCD;
            }
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