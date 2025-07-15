using System;

namespace Assets.Version2
{
    public interface IDamageable
    {
        public bool IsDead { get; }
        public void TakeDamage(float point);
    }
    public interface IHealable
    {
        public event Action<float> OnHealed;
        public event Action<float> OnDying;
        public bool IsDead { get; }
        public bool IsFullHP { get; }
        public bool IsAcceptHealed { get; set; }
        public void BeingHealed(float point);
    }
}