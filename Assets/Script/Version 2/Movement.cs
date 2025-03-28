using UnityEngine;

namespace Assets.Version2
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_maxSpeed = 3f;
        [SerializeField] private float m_currentSpeed;

        public void MoveTo(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(m_currentSpeed * Time.deltaTime * direction);
        }

        public void Initialize()
        {
            m_currentSpeed = m_maxSpeed;
        }
    }
}