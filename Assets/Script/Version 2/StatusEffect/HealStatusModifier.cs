using UnityEngine;
using Assets.Version2.GameEnum;
using Assets.Version2.Pool;

namespace Assets.Version2.StatusEffectSystem
{
    public class HealStatusModifier : StatusModifier
    {
        [Header("Target")]
        [SerializeField] protected IHealable m_healable;
        [SerializeField] protected ParticleSystem m_particleEffect;
        [SerializeField] protected Unit m_source;


        public HealStatusModifier(StatusEffectDataSO data, int level, Unit target, Unit source) : base(data, level, target)
        {
            m_healable = target.Healable;
            m_source = source;

            UpdateEffectValue();
        }

        public override void UpdateEffectValue()
        {
            //Spread the base point and source unit point
            m_currentPoint = m_currentVariant.BasePoint + m_source.Interact.CurrentPoint;
            float t_frequence = m_remainDuration / m_currentVariant.TickInterval;
            m_currentPoint /= t_frequence;
        }

        public override void Apply()
        {
            m_particleEffect = ObjectPoolManagerSO.Instance.Get<ParticleSystem>(Group.None, m_particleType);
            m_particleEffect.transform.position = m_target.transform.position;
            m_particleEffect.transform.SetParent(m_target.transform);
            m_particleEffect.Play();
        }

        //If reamain duration is 0, return ture represent the modifier should be remove.
        public override bool Tick(float deltaTime)
        {
            m_currentTickCD = Mathf.Max(m_currentTickCD - deltaTime, 0f);
            if (m_currentTickCD == 0f)
            {
                m_currentTickCD = m_currentVariant.TickInterval;
                Effect();
            }

            m_remainDuration = Mathf.Max(m_remainDuration - deltaTime, 0f);
            if (m_remainDuration == 0f)
            {
                return true;
            }

            return false;
        }

        public override void Effect()
        {
            m_healable.BeingHealed(m_currentPoint);//還要再加上Operation Argument
        }

        public override void Remove()
        {
            m_particleEffect.Stop();
            ObjectPoolManagerSO.Instance.Recycle(Group.None, m_particleType, m_particleEffect);
        }
    }
}