using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class IncreaseStack : IStackBehavior
    {
        public void OnApplyStack(StatusModifierBase modifier)
        {
            modifier.CurrentStack = Mathf.Min(modifier.CurrentStack + 1, modifier.SourceData.MaxStack);
        }
    }
}