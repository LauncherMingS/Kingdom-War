using System;
using UnityEngine;

namespace Assets.Version2
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float m_maxHP = 20f;
        [SerializeField] private float m_currentHP;

        public event Action<float> OnHurt;

        void IDamageable.TakeDamage(float point)
        {
            m_currentHP = Mathf.Clamp(m_currentHP - point, 0f, m_maxHP);

            if (m_currentHP <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            OnHurt.Invoke(point);
        }

        public void Initialize()
        {
            m_currentHP = m_maxHP;
        }
    }
}