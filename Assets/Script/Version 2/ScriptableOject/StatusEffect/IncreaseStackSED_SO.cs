using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 2, menuName = "Scriptable Object/Status Effect System/IncreaseStack SED", fileName = "IncreaseStack SED")]
    public class IncreaseStackSED_SO : StatusEffectDataSO
    {
        public override void Initialize()
        {
            m_stackBehavior = StatusEffectCentral.Instance.IncreaseStack;
        }
    }
}