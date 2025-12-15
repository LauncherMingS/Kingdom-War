using Assets.Version2.StatusEffectSystem;
using System;
using UnityEngine;

namespace Assets.Version2
{
    public class Health : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private float m_maxHP;
        [SerializeField] private float m_currentHP;
        [SerializeField] private float m_damageModifier;
        [SerializeField] private bool m_isDead;

        [SerializeField] private StatusEffectManager m_statusEffectManager;

        public event Action<float> OnHurt;
        public event Action<float> OnHealed;
        public event Action<float> OnDying;

        public float DamageModifier
        {
            get => m_damageModifier * m_statusEffectManager.GetTotalDamageMultiplier();
            set => m_damageModifier = value;
        }

        public bool IsDead => m_isDead;

        public bool IsFullHP => m_currentHP == m_maxHP;


        public void TakeDamage(float point)
        {
            if (m_isDead)
            {
                return;
            }

            point *= DamageModifier;
            m_currentHP = Mathf.Clamp(m_currentHP - point, 0f, m_maxHP);

            if (m_currentHP <= 0f)
            {
                m_isDead = true;
                OnDying?.Invoke(point);
                return;
            }

            OnHurt?.Invoke(point);
        }

        public void BeingHealed(float point)
        {
            if (m_isDead)
            {
                return;
            }

            m_currentHP = Mathf.Clamp(m_currentHP + point, 0f, m_maxHP);

            OnHealed?.Invoke(point);
        }

        public void Initialize(StatusEffectManager manager)
        {
            m_currentHP = m_maxHP;
            m_damageModifier = 1f;
            m_isDead = false;

            m_statusEffectManager = manager;
        }

        public void Uninitialize()
        {
            OnDying = OnHealed = OnHurt = null;
        }
    }
}