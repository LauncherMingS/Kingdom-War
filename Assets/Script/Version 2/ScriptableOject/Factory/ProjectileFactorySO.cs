using UnityEngine;

namespace Assets.Version2.Factory
{
    [CreateAssetMenu(order = 3, menuName = "Scriptable Object/FactorySO/ProjectileFactorySO", fileName = "ProjectileFactorySO")]
    public class ProjectileFactorySO : FactorySO<Projectile>
    {
        [SerializeField] private Projectile m_projectile;


        public override Projectile Prefab
        {
            get => m_projectile;
        }
    }
}