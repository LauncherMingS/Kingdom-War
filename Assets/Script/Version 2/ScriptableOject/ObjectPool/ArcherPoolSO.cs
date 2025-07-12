using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 3, menuName = "Scriptable Object/ObjectPoolSO/ArcherPoolSO", fileName = "ArcherPoolSO")]
    public class ArcherPoolSO : ObjectPoolSO<Archer>
    {
        [SerializeField] private Archer m_archer;

        public override Archer Prefab
        {
            get => m_archer;
        }
    }
}