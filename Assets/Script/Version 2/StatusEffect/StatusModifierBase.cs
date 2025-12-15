using UnityEngine;
using Assets.Version2.GameEnum;
using Assets.Version2.Pool;

namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class StatusModifierBase
    {
        [Header("Source Reference")]
        [SerializeField] protected StatusEffectDataSO m_sourceData;

        [Header("Parameter")]
        [SerializeField] protected Unit m_target;
        [SerializeField] protected float m_currentPoint;
        [SerializeField] protected float m_remainDuration;
        [SerializeField] protected float m_currentTickCD;
        [SerializeField] protected int m_currentStack;
        [SerializeField] protected UnitType m_particleType;
        [SerializeField] protected ParticleSystem m_particleEffect;

        public StatusEffectDataSO SourceData => m_sourceData;

        public float RemainingDuration
        {
            get => m_remainDuration;
            set => m_remainDuration = value;
        }

        public int CurrentStack
        {
            get => m_currentStack;
            set => m_currentStack = value;
        }


        public virtual bool CompareSourceData(int sourceDataID)
        {
            return m_sourceData.GetInstanceID() == sourceDataID;
        }

        public virtual void OnStack()
        {
            m_sourceData.StackBehavior?.OnApplyStack(this);
        }

        public virtual bool OnTick(float deltaTime)
        {
            m_remainDuration = Mathf.Max(m_remainDuration - deltaTime, 0f);
            if (m_remainDuration == 0f)
            {
                return true;
            }

            return false;
        }

        public virtual void OnApply()
        {
            m_particleEffect = ObjectPoolManagerSO.Instance.Get<ParticleSystem>(Group.None, m_particleType);
            m_particleEffect.transform.position = m_target.transform.position;
            m_particleEffect.transform.SetParent(m_target.transform);
            m_particleEffect.Play();
        }

        public virtual void OnRemove()
        {
            m_particleEffect.Stop();
            ObjectPoolManagerSO.Instance.Recycle(Group.None, m_particleType, m_particleEffect);
        }

        public virtual void OnInitialize(StatusEffectDataSO data, Unit target)
        {
            m_sourceData = data;

            m_target = target;
            m_remainDuration = m_sourceData.Duration;
            m_currentTickCD = m_sourceData.FirstTickDelay;
            m_currentStack = m_sourceData.InitialStack;
            m_particleType = m_sourceData.ParticleType;
        }
    }
}