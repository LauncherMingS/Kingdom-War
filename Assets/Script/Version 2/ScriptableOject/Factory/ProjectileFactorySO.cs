using Assets.Version2;
using UnityEngine;

namespace Assets.Version2
{
    [CreateAssetMenu(order = 7, menuName = "Scriptable Object/FactorySO/ProjectileFactorySO", fileName = "ProjectileFactorySO")]
    public class ProjectileFactorySO : FactoryBaseSO<Projectile>
    {
        [SerializeField] private Projectile m_projectile;

        public override Projectile Create()
        {
            return Instantiate(m_projectile);
        }
    }
}