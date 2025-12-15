using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class HealStatusModifier : StatusModifierBase
    {
        [Header("Target")]
        [SerializeField] protected IHealable m_healable;
        [SerializeField] protected Unit m_source;


        public void UpdateEffectPoint()
        {
            //Spread the base point and source unit point
            m_currentPoint = m_sourceData.BasePoint + m_source.Interact.CurrentPoint;
            float t_frequence = m_remainDuration / m_sourceData.TickInterval;
            m_currentPoint /= t_frequence;
        }

        //If reamain duration is 0, return ture represent the modifier should be remove.
        public override bool OnTick(float deltaTime)
        {
            m_currentTickCD = Mathf.Max(m_currentTickCD - deltaTime, 0f);
            if (m_currentTickCD == 0f)
            {
                m_currentTickCD = m_sourceData.TickInterval;

                m_healable.BeingHealed(m_currentPoint);//還要再加上Operation Argument
            }

            return base.OnTick(deltaTime);
        }

        public void Initialize(StatusEffectDataSO data, Unit target, Unit source)
        {
            base.OnInitialize(data, target);

            m_healable = target.Healable;
            m_source = source;

            UpdateEffectPoint();
        }
    }
}