using UnityEngine;

namespace Assets.Version2
{
    public class MeleeAttackHandler : AttackHandlerBase
    {
        [Space(32f)]
        [Header("MeleeAttackHandler")]

        [Header("Game Reference")]
        [SerializeField] private Transform m_target;

        
        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        public override void OnExecuteAttack()
        {
            if (m_target != null && m_target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(m_currentPoint);
                EnterColdDown();
            }

            m_target = null;
        }
    }
}