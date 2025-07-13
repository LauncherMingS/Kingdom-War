using UnityEngine;
using Assets.Version2.Pool;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class RangedAttackHandler : AttackHandlerBase
    {
        [Space(32f)]
        [Header("RangedAttackHandler")]

        [Header("Game Reference")]
        [SerializeField] private Transform m_launchPoint;

        [Header("Parameter")]
        [Header("Target")]
        [SerializeField] private int m_targetLayer;
        [Header("Projectile")]
        [SerializeField] private float m_projectileRadius = 0.5f;//get from projectile collider radius
        [SerializeField] private float m_launchSpeed = 20f;
        [SerializeField] private float m_launchDegree = 25f;
        [SerializeField] private Vector2 m_launchVelocity;

        public float ProjectileRadius => m_projectileRadius;


        public override void OnExecuteAttack()
        {
            Projectile t_arrow = ObjectPoolManagerSO.Instance.Get<Projectile>(Group.None, UnitType.Projectile);
            t_arrow.Initialize(m_launchVelocity, m_currentPoint, m_targetLayer);
            t_arrow.transform.position = m_launchPoint.position;

            EnterColdDown();
        }

        public void Initialize(int targetLayer)
        {
            base.Initialize();

            m_targetLayer = targetLayer;
            float t_directionX = GameManager.Instance.IsSYWS(m_targetLayer) ? -1 : 1;
            float t_radian = m_launchDegree * Mathf.Deg2Rad;
            float t_velocityX = m_launchSpeed * Mathf.Cos(t_radian) * t_directionX;
            float t_velocityY = m_launchSpeed * Mathf.Sin(t_radian);
            m_launchVelocity = new Vector2(t_velocityX, t_velocityY);
        }
    }
}