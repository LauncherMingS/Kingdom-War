using System;
using UnityEngine;

namespace Assets.Version2
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private Transform m_launchPoint;

        [Header("Parameter")]
        [Header("Projectile")]
        [SerializeField] private float m_projectileRadius;//get from projectile collider radius
        [SerializeField] private float m_launchSpeed;
        [SerializeField] private float m_launchDegree;
        [SerializeField] private Vector2 m_launchVelocity;
        [Header("Target")]
        [SerializeField] private int m_targetLayer;
        [Header("Attack Point")]
        [SerializeField] private float m_attackPoint;

        [Header("Asset Reference")]
        [SerializeField] private GameObject m_arrow;

        public float ProjectileRadius => m_projectileRadius;

        //Due to the order of execution, this value needs to be set separately.
        public float AttackPoint
        {
            set => m_attackPoint = value;
        }

        public event Action OnLaunch;


        //Trigger by Animation event in "Attack Archer"
        public void LaunchProjectile()
        {
            Instantiate(m_arrow, m_launchPoint.position, Quaternion.identity, transform)
                .GetComponent<Projectile>().Initialize(m_launchVelocity, m_attackPoint, m_targetLayer);

            OnLaunch.Invoke();
        }

        public void Initialize(int targetLayer)
        {
            float t_radian = m_launchDegree * Mathf.Deg2Rad;
            float t_velocityX = m_launchSpeed * Mathf.Cos(t_radian);
            float t_velocityY = m_launchSpeed * Mathf.Sin(t_radian);
            m_launchVelocity = new Vector2(t_velocityX, t_velocityY);
            m_targetLayer = targetLayer;
        }
    }
}