namespace Assets.Version2
{
    public interface IDamageable
    {
        public bool IsDead { get; }
        public void TakeDamage(float point);
    }
    public interface IHealable
    {
        public void Heal(float point);
    }
}