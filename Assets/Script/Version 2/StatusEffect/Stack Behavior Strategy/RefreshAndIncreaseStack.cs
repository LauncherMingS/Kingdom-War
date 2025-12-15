using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class RefreshAndIncreaseStack : IStackBehavior
    {
        public void OnApplyStack(StatusModifierBase modifier)
        {
            modifier.RemainingDuration = modifier.SourceData.Duration;

            modifier.CurrentStack = Mathf.Min(modifier.CurrentStack + 1, modifier.SourceData.MaxStack);
        }
    }
}