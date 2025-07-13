using UnityEngine;
using Assets.Version2.GameEnum;
using Assets.Version2.Pool;

namespace Assets.Version2
{
    public class Projectile : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] private float m_lifetime = 3f;
        [SerializeField] private float m_timer;
        [SerializeField] private float m_attackPoint;
        [SerializeField] private int m_targetLayer;
        [SerializeField] private bool m_isRecycle;

        [Header("Component Reference")]
        [SerializeField] private ProjectileMovement m_movement;


        private void Check(float deltaTime)
        {
            m_timer -= deltaTime;
            if (m_timer <= 0f && !m_isRecycle)
            {
                Recycle();
            }
        }

        private void Recycle()
        {
            m_isRecycle = true;
            ObjectPoolManagerSO.Instance.Recycle(Group.None, UnitType.Projectile, this);
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
            m_isRecycle = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == m_targetLayer && other.TryGetComponent(out IDamageable damageable)
                && !m_isRecycle)
            {
                damageable.TakeDamage(m_attackPoint);

                Recycle();
            }
        }
    }
}