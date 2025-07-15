using UnityEngine;

namespace Assets.Version2
{
    public class MeleeAttackHandler : InteractHandler, IAttacking
    {
        [SerializeField] protected IDamageable m_damageable;

        public override Transform Target
        {
            get => m_target;
            set
            {
                m_target = value;
                m_target.TryGetComponent(out m_damageable);
            }
        }


        protected override void ClearTarget()
        {
            base.ClearTarget();

            m_damageable = null;
        }

        public void OnExecuteAttack()
        {
            if (m_damageable != null && !m_damageable.IsDead)
            {
                m_damageable.TakeDamage(m_currentPoint);
                EnterColdDown();
            }

            ClearTarget();
        }
    }
}