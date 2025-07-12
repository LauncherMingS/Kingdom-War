using System;
using UnityEngine;

namespace Assets.Version2
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float m_maxHP = 20f;
        [SerializeField] private float m_currentHP;
        [SerializeField] private bool m_isDead;

        public event Action<float> OnHurt;
        public event Action OnDying;

        public bool IsDead => m_isDead;


        public void TakeDamage(float point)
        {
            m_currentHP = Mathf.Clamp(m_currentHP - point, 0f, m_maxHP);

            if (m_currentHP <= 0f && !m_isDead)
            {
                OnDying?.Invoke();
                m_isDead = true;
                return;
            }

            OnHurt?.Invoke(point);
        }

        public void Initialize()
        {
            m_currentHP = m_maxHP;
            m_isDead = false;
        }
    }
}