using UnityEngine;

namespace Assets.Version2
{
    public class ProjectileMovement : MonoBehaviour
    {
        [SerializeField] private float m_gravity = -9.8f;
        [SerializeField] private Vector3 m_velocity;

        public void Simulate(float deltaTime)
        {
            //Move
            m_velocity.y += m_gravity * deltaTime;
            transform.position += m_velocity * deltaTime;

            //Face
            transform.right = m_velocity;
        }

        public void Initialize(Vector3 initialVelocity)
        {
            m_velocity = initialVelocity;
        }
    }
}