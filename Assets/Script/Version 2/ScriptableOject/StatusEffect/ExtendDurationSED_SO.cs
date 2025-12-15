using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 5, menuName = "Scriptable Object/Status Effect System/ExtendDuration SED", fileName = "ExtendDuration SED")]
    public class ExtendDurationSED_SO : StatusEffectDataSO
    {
        public override void Initialize()
        {
            m_stackBehavior = StatusEffectCentral.Instance.ExtendDuration;
        }
    }
}