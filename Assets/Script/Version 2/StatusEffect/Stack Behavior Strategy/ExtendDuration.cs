namespace Assets.Version2.StatusEffectSystem
{
    [System.Serializable]
    public class ExtendDuration : IStackBehavior
    {
        public void OnApplyStack(StatusModifierBase modifier)
        {
            modifier.RemainingDuration += modifier.SourceData.Duration;
        }
    }
}