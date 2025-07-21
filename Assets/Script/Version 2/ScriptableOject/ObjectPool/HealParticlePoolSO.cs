using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 6, menuName = "Scriptable Object/ObjectPoolSO/HealParticlePoolSO", fileName = "HealParticlePoolSO")]
    public class HealParticlePoolSO : ObjectPoolSO<ParticleSystem>
    {
        [SerializeField] private ParticleSystem m_healParticle;

        public override ParticleSystem Prefab
        {
            get => m_healParticle;
        }
    }
}