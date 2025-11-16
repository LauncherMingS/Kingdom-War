using System;
using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.StatusEffectSystem
{
    [Serializable]
    public abstract class StatusModifier
    {
        [Header("Source Info")]
        [SerializeField] protected StatusEffectDataSO m_sourceData;
        [SerializeField] protected int m_level;
        [Header("Parameter")]
        [SerializeField] protected Transform m_target;
        [SerializeField] protected StatusEffectDataSO.Variant m_currentVariant;
        [SerializeField] protected float m_currentPoint;
        [SerializeField] protected float m_remainDuration;
        [SerializeField] protected float m_currentTickCD;
        [SerializeField] protected int m_currentStack;
        [SerializeField] protected UnitType m_particleType;

        public StatusEffectDataSO.Variant CurrentVariant => m_currentVariant;


        public StatusModifier(StatusEffectDataSO data, int level, Unit target)
        {
            m_sourceData = data;
            m_level = level;

            m_target = target.transform;
            m_currentVariant = m_sourceData[m_level];
            m_remainDuration = m_currentVariant.Duration;
            m_currentTickCD = m_currentVariant.FirstTickDelay;
            m_currentStack = 0;
            m_particleType = m_currentVariant.ParticleType;
        }

        public int GetSourceDataID()
        {
            return m_sourceData.GetInstanceID();
        }

        public bool CompareSourceData(int sourceDataID)
        {
            return m_sourceData.GetInstanceID() == sourceDataID;
        }

        public virtual void RefreshDuration()
        {
            m_remainDuration = m_currentVariant.Duration;
        }

        public virtual void StackUp(int stackNum = 1)
        {
            m_currentStack = Mathf.Min(m_currentStack + stackNum, m_currentVariant.MaxStack);
        }

        public virtual void UpdateEffectValue() { }
        public abstract void Apply();
        public abstract bool Tick(float deltaTime);
        public virtual void Effect() { }
        public abstract void Remove();
    }
}