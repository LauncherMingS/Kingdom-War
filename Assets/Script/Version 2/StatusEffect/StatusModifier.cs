using System;
using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [Serializable]
    public abstract class StatusModifier
    {
        [Header("Source Info")]
        [SerializeField] protected Unit m_sourceUnit;
        [SerializeField] protected StatusEffectDataSO m_sourceData;
        [SerializeField] protected int m_level;
        [Header("Parameter")]
        [SerializeField] protected StatusEffectDataSO.Variant m_currentVariant;
        [SerializeField] protected float m_currentPoint;
        [SerializeField] protected float m_remainDuration;
        [SerializeField] protected float m_currentTickCD;
        [SerializeField] protected int m_currentStack;
        [SerializeField] protected float m_affectedPoint;//use to trace back affected point

        public StatusEffectDataSO.Variant CurrentVariant => m_currentVariant;


        public StatusModifier(StatusEffectDataSO data, int level, Unit sourceUnit)
        {
            m_sourceData = data;
            m_level = level;
            m_currentVariant = m_sourceData.GetVariant(m_level);

            m_sourceUnit = sourceUnit;

            m_remainDuration = m_currentVariant.Duration;
            m_currentTickCD = m_currentVariant.FirstTickDelay;
            m_currentStack = 1;
        }

        public int GetSourceDataID()
        {
            return m_sourceData.GetInstanceID();
        }

        public bool CompareSourceData(int sourceDataID)
        {
            return m_sourceData.GetInstanceID() == sourceDataID;
        }

        public void RefreshDuration()
        {
            m_remainDuration = m_currentVariant.Duration;
        }

        public void StackUp(int stackNum = 1)
        {
            m_currentStack = Mathf.Min(m_currentStack + stackNum, m_currentVariant.MaxStack);
            UpdateEffectValue();
        }

        public abstract void UpdateEffectValue();
        public abstract void Apply();
        public abstract bool Tick(float deltaTime);
        public abstract void Remove();
    }
}