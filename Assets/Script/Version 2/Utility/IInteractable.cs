using UnityEngine;

namespace Assets.Version2
{
    public interface IDamageable
    {
        public void TakeDamage(float point);
    }
    public interface IHealable
    {
        public void Heal(float point);
    }
}