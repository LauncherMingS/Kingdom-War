namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class RefreshDuration : IStackBehavior
    {
        public void OnApplyStack(StatusModifierBase modifier)
        {
            modifier.RemainingDuration = modifier.SourceData.Duration;
        }
    }
}