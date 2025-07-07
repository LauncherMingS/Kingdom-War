using UnityEngine;

namespace Assets.Version2
{
    [CreateAssetMenu(order = 1, menuName = "Scriptable Object/FactorySO/UnitFactory", fileName = "UnitFactory")]
    public class UnitFactory : ScriptableObject
    {
        [Header("Sword Man SYWS")]
        [SerializeField] private SwordManFactorySO m_SYWS_SwordManFactory;
        [Header("Sword Man NLI")]
        [SerializeField] private SwordManFactorySO m_NLI_SwordManFactory;
        [Header("Projectile")]
        [SerializeField] private ProjectileFactorySO m_projectileFactory;


        public SwordMan CreateSwordMan(Group group)
        {
            return group switch
            {
                Group.SYWS => m_SYWS_SwordManFactory.Create(),
                Group.NLI => m_NLI_SwordManFactory.Create(),
                Group.None => null,
                _ => throw new System.NotImplementedException(),
            };
        }

        public Projectile CreateProjectile()
        {
            return m_projectileFactory.Create();
        }
    }
}