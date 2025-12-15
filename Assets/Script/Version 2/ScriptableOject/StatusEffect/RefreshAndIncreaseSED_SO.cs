using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 4, menuName = "Scriptable Object/Status Effect System/RefreshAndIncrease SED", fileName = "RefreshAndIncrease SED")]
    public class RefreshAndIncreaseSED_SO : StatusEffectDataSO
    {
        public override void Initialize()
        {
            m_stackBehavior = StatusEffectCentral.Instance.RefreshAndIncreaseStack;
        }
    }
}