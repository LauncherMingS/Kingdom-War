using System;
using UnityEngine;

namespace Assets.Version2
{
    public class Health : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private float m_maxHP = 20f;
        [SerializeField] private float m_currentHP;
        [SerializeField] private bool m_isDead;
        [SerializeField] private bool m_isFullHP;
        [SerializeField] private bool m_isAcceptHealed;

        public event Action<float> OnHurt;
        public event Action<float> OnHealed;
        public event Action<float> OnDying;

        public bool IsDead => m_isDead;

        public bool IsFullHP => m_isFullHP;

        public bool IsAcceptHealed
        {
            get => m_isAcceptHealed;
            set => m_isAcceptHealed = value;
        }


        public void CheckIsFullHP()
        {
            m_isFullHP = (m_currentHP == m_maxHP);
        }

        public void TakeDamage(float point)
        {
            if (m_isDead)
            {
                return;
            }

            m_currentHP = Mathf.Clamp(m_currentHP - point, 0f, m_maxHP);

            if (m_currentHP <= 0f)
            {
                m_isDead = true;
                OnDying?.Invoke(point);
                return;
            }

            CheckIsFullHP();
            OnHurt?.Invoke(point);
        }

        public void BeingHealed(float point)
        {
            if (m_isDead)
            {
                return;
            }

            m_currentHP = Mathf.Clamp(m_currentHP + point, 0f, m_maxHP);

            CheckIsFullHP();
            OnHealed?.Invoke(point);
        }

        public void Initialize()
        {
            m_currentHP = m_maxHP;
            m_isDead = false;

            CheckIsFullHP();
        }
    }
}