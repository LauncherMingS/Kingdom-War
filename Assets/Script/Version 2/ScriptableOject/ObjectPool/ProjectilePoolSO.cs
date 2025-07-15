using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 5, menuName = "Scriptable Object/ObjectPoolSO/ProjectilePoolSO", fileName = "ProjectilePoolSO")]
    public class ProjectilePoolSO : ObjectPoolSO<Projectile>
    {
        [SerializeField] private Projectile m_projectile;

        public override Projectile Prefab
        {
            get => m_projectile;
        }
    }
}