using UnityEngine;

namespace Assets.Version2
{
    public class MeleeAttackHandler : InteractHandler, IAttacking
    {
        [SerializeField] protected IDamageable m_damageable;

        public override Unit Target
        {
            get => m_target;
            set
            {
                m_target = value;
                if (m_target == null || m_target.Damageable == null)
                {
                    ClearTarget();
                    return;
                }

                m_damageable = m_target.Damageable;
            }
        }

        public IDamageable Damageable => m_damageable;


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