namespace Assets.Version2.StatusEffectSystem
{
    public interface IStackBehavior
    {
        public void OnApplyStack(StatusModifierBase modifier);
    }
}