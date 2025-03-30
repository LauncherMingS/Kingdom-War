using UnityEngine;

namespace Assets.Version2
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float m_maxSpeed = 3f;
        [SerializeField] private float m_currentSpeed;

        [SerializeField] private float m_bufferSpeed = 0.75f;
        [SerializeField] private float m_bufferDistance = 4f;

        [SerializeField] private float m_stopDistance = 0.1f;

        public void MoveTo(Vector3 targetPosition, float targetDistance, float deltaTime)
        {
            if (targetDistance == 0f)
            {
                return;
            }

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