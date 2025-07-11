using UnityEngine;

namespace Assets.Version2.Factory
{
    [CreateAssetMenu(order = 2, menuName = "Scriptable Object/FactorySO/SwordManFactorySO", fileName = "SwordManFactorySO")]
    public class SwordManFactorySO : FactorySO<SwordMan>
    {
        [SerializeField] private SwordMan m_swordMan;


        public override SwordMan Prefab
        {
            get => m_swordMan;
        }
    }
}