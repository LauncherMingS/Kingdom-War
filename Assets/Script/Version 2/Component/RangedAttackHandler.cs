using System;
using UnityEngine;

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

        [Header("Asset Reference")]
        [SerializeField] private GameObject m_arrow;

        public float ProjectileRadius => m_projectileRadius;


        public override void OnExecuteAttack()
        {
            Instantiate(m_arrow, m_launchPoint.position, Quaternion.identity, transform)
                .GetComponent<Projectile>().Initialize(m_launchVelocity, m_currentPoint, m_targetLayer);

            EnterColdDown();
        }

        public void Initialize(int targetLayer)
        {
            base.Initialize();

            m_targetLayer = targetLayer;
            float t_radian = m_launchDegree * Mathf.Deg2Rad;
            float t_velocityX = m_launchSpeed * Mathf.Cos(t_radian);
            float t_velocityY = m_launchSpeed * Mathf.Sin(t_radian);
            m_launchVelocity = new Vector2(t_velocityX, t_velocityY);
        }
    }
}