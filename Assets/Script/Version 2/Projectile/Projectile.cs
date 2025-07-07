using UnityEngine;

namespace Assets.Version2
{
    public class Projectile : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] private float m_lifetime = 3f;
        [SerializeField] private float m_timer;
        [SerializeField] private float m_attackPoint;
        [SerializeField] private int m_targetLayer;

        [Header("Component Reference")]
        [SerializeField] private ProjectileMovement m_movement;


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

        public void Initialize(Vector2 velocity, float attackPoint, int targetLayer)
        {
            m_timer = m_lifetime;
            m_attackPoint = attackPoint;
            m_targetLayer = targetLayer;
            m_movement.Initialize(new Vector3(velocity.x, velocity.y, 0f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == m_targetLayer
                && other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(m_attackPoint);

                Destroy(gameObject);
                return;
            }
        }
    }
}