using UnityEngine;

namespace Assets.Version2
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileMovement m_movement;

        [SerializeField] private float m_lifetime = 3f;
        [SerializeField] private float m_timer;


        private void Check(float deltaTime)
        {
            m_timer -= deltaTime;
            if (m_timer <= 0f)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            float t_deltaTime = Time.deltaTime;

            m_movement.Simulate(t_deltaTime);
            Check(t_deltaTime);
        }

        public void Initialize(float velocityX, float velocityY)
        {
            m_timer = m_lifetime;
            m_movement.Initialize(new Vector3(velocityX, velocityY, 0f));
        }
    }
}