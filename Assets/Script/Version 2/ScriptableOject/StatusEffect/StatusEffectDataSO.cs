using System;
using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 1, menuName = "Scriptable Object/Status Effect System/StatusEffectDataSO", fileName = "StatusEffectDataSO")]
    public class StatusEffectDataSO : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private string m_description;
        [SerializeField] private StatusEffectType m_type;
        [SerializeField] private Variant[] m_variants = new Variant[1];

        public StatusEffectType EffectType => m_type;


        public Variant GetVariant(int level)
        {
            if (m_variants.Length == 0 || level >= m_variants.Length)
            {
                return null;
            }

            return m_variants[level];
        }

        [Serializable]
        public class Variant
        {
            [Header("Visual Effect")]
            [SerializeField] private string m_name;
            [SerializeField] private float m_animationPlaySpeed;
            [SerializeField] private Color m_tint;
            [SerializeField] private ParticleSystem m_particleEffect;

            [Header("Parameter")]
            [SerializeField] private StatusEffectStack m_stackType;
            [SerializeField] private float m_basePoint;
            [SerializeField] private float m_duration;
            [SerializeField] private float m_tickInterval;
            [SerializeField] private float m_firstTickDelay;
            [SerializeField] private int m_maxStack;

            public ParticleSystem ParticleEffect => m_particleEffect;

            public StatusEffectStack StackType => m_stackType;

            public string Name => m_name;

            public float BasePoint => m_basePoint;

            public float Duration => m_duration;

            public float TickInterval => m_tickInterval;

            public float FirstTickDelay => m_firstTickDelay;

            public int MaxStack => m_maxStack;
        }
    }
}