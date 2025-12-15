using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(order = 3, menuName = "Scriptable Object/Status Effect System/RefreshDuration SED", fileName = "RefreshDuration SED")]
    public class RefreshDurationSED_SO : StatusEffectDataSO
    {
        public override void Initialize()
        {
            m_stackBehavior = StatusEffectCentral.Instance.RefreshDuration;
        }
    }
}