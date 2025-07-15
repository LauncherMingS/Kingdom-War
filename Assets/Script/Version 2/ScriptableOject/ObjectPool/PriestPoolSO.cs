using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 4, menuName = "Scriptable Object/ObjectPoolSO/PriestPoolSO", fileName = "PriestPoolSO")]
    public class PriestPoolSO : ObjectPoolSO<Priest>
    {
        [SerializeField] private Priest m_priest;

        public override Priest Prefab
        {
            get => m_priest;
        }
    }
}