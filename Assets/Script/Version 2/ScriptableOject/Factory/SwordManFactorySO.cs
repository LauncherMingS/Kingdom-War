using Assets.Version2;
using UnityEngine;

namespace Assets.Version2
{
    [CreateAssetMenu(order = 3, menuName = "Scriptable Object/FactorySO/SwordManFactorySO", fileName = "SwordManFactorySO")]
    public class SwordManFactorySO : FactoryBaseSO<SwordMan>
    {
        [SerializeField] private SwordMan m_swordMan;

        public override SwordMan Create()
        {
            return Instantiate(m_swordMan);
        }
    }
}