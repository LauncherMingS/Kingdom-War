namespace Assets.Version2
{
    interface IAttacking
    {
        public IDamageable Damageable { get; }
        void OnExecuteAttack();
    }

    interface IHealing
    {
        public IHealable Healable { get; }
        void OnExecuteHeal();
    }
}