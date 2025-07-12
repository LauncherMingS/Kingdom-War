using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 2, menuName = "Scriptable Object/ObjectPoolSO/SwordManPoolSO", fileName = "SwordManPoolSO")]
    public class SwordManPoolSO : ObjectPoolSO<SwordMan>
    {
        [SerializeField] private SwordMan m_swordMan;

        public override SwordMan Prefab
        {
            get => m_swordMan;
        }
    }
}