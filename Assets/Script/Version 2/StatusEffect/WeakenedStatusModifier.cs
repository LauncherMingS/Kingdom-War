using UnityEngine;
using Assets.Version2.GameEnum;
using Assets.Version2.Pool;

namespace Assets.Version2.StatusEffectSystem
{
    public class WeakenedStatusModifier : StatusModifier
    {
        [SerializeField] protected IDamageable m_damageable;
        [SerializeField] protected ParticleSystem m_particleEffect;


        public WeakenedStatusModifier (StatusEffectDataSO data, int level, Unit target): base(data, level, target)
        {
            m_currentPoint = m_currentVariant.BasePoint;
            m_damageable = target.Damageable;
        }

        public override void RefreshDuration()
        {
            m_remainDuration = m_currentVariant.Duration;
        }

        public override void StackUp(int stackNum = 1)
        {
            m_damageable.DamageModifier = Mathf.Ceil(m_damageable.DamageModifier - m_currentPoint * m_currentStack);

            base.StackUp(stackNum);

            m_damageable.DamageModifier = Mathf.Ceil(m_damageable.DamageModifier + m_currentPoint * m_currentStack);
        }

        public override void Apply()
        {
            m_particleEffect = ObjectPoolManagerSO.Instance.Get<ParticleSystem>(Group.None, m_particleType);
            m_particleEffect.transform.position = m_target.transform.position;
            m_particleEffect.transform.SetParent(m_target.transform);
            m_particleEffect.Play();
            StackUp();
        }

        public override bool Tick(float deltaTime)
        {
            m_remainDuration = Mathf.Max(m_remainDuration - deltaTime, 0f);
            if (m_remainDuration == 0f)
            {
                return true;
            }

            return false;
        }

        public override void Remove()
        {
            m_particleEffect.Stop();
            ObjectPoolManagerSO.Instance.Recycle(Group.None, m_particleType, m_particleEffect);

            m_damageable.DamageModifier = Mathf.Ceil(m_damageable.DamageModifier - m_currentPoint * m_currentStack);
        }
    }
}