using UnityEngine;

namespace Assets.Version2
{
    public class Movement : MonoBehaviour
    {
        [Header("Basic Speed")]
        [SerializeField] private float m_maxSpeed = 3f;
        [SerializeField] private float m_currentSpeed;
        [Header("Buffer Speed")]
        [SerializeField] private float m_bufferSpeed = 0.75f;
        [SerializeField] private float m_bufferDistance = 4f;
        [SerializeField] private float m_stopDistance = 0.1f;

        public void Move(Vector3 targetPosition, float targetDistance, float deltaTime)
        {
            if (m_stopDistance > targetDistance)
            {
                transform.position = targetPosition;
                return;
            }

            if (m_bufferDistance > targetDistance)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, m_bufferSpeed * deltaTime);
                return;
            }
            
            Vector3 direction = Vector3.Normalize(targetPosition - transform.position);
            Vector3 distanceDelta = m_currentSpeed * deltaTime * direction;
            transform.position += distanceDelta;
        }

        public void Initialize()
        {
            m_currentSpeed = m_maxSpeed;
        }
    }
}