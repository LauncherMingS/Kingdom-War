using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 1, menuName = "Scriptable Object/Status Effect System/StatusEffectDataSO", fileName = "StatusEffectDataSO")]
    public class StatusEffectDataSO : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private string m_description;

        [Header("Parameter")]
        [SerializeField] private StatusEffectType m_type;
        [SerializeField] private float m_basePoint;
        [SerializeField] private float m_duration;
        [SerializeField] private float m_tickInterval;
        [SerializeField] private float m_firstTickDelay;
        [SerializeField] private int m_maxStack;
        [SerializeField] private int m_initialStack;
        [SerializeField] protected IStackBehavior m_stackBehavior;

        [Header("Visual Effect")]
        [SerializeField] private UnitType m_particleType;

        public StatusEffectType EffectType => m_type;

        public float BasePoint => m_basePoint;

        public float Duration => m_duration;

        public float TickInterval => m_tickInterval;

        public float FirstTickDelay => m_firstTickDelay;

        public int MaxStack => m_maxStack;

        public int InitialStack => m_initialStack;

        public IStackBehavior StackBehavior => m_stackBehavior;

        public UnitType ParticleType => m_particleType;


        public virtual void Initialize() { }
    }
}