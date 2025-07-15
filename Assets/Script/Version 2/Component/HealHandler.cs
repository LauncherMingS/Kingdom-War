using UnityEngine;

namespace Assets.Version2
{
    public class HealHandler : InteractHandler, IHealing
    {
        [SerializeField] protected IHealable m_healable;

        [SerializeField] protected bool m_isBusyOnHealing;

        public override Transform Target
        {
            get => m_target;
            set
            {
                m_target = value;
                if (m_target.TryGetComponent(out m_healable))
                {
                    if (m_healable.IsDead || m_healable.IsFullHP)
                    {
                        ClearTarget();
                        return;
                    }

                    BindingStatus(true);
                    RegisterListener();
                }
            }
        }

        public bool IsBusyOnHealing => m_isBusyOnHealing;


        protected override void ClearTarget()
        {
            base.ClearTarget();

            m_healable = null;
        }

        protected void BindingStatus(bool isBinding)
        {
            m_isBusyOnHealing = isBinding;
            m_healable.IsAcceptHealed = isBinding;
        }

        protected void RegisterListener()
        {
            if (m_healable != null)
            {
                m_healable.OnHealed += CheckStopHeal;
                m_healable.OnDying += CheckStopHeal;
            }
        }

        protected void RemoveListener()
        {
            if (m_healable != null)
            {
                m_healable.OnHealed -= CheckStopHeal;
                m_healable.OnDying -= CheckStopHeal;
            }
        }

        //There are three situations that will interrupt the heal
        //1. Hurted unit is full HP(m_healable.OnHealed Invoke)
        //2. Hurted unit dies(m_healable.OnDying Invoke)
        //3. Priest dies(UnInitialize())
        protected void CheckStopHeal(float point)
        {
            if (m_healable.IsDead || m_healable.IsFullHP)
            {
                BindingStatus(false);
                RemoveListener();

                ClearTarget();
            }
        }

        //Invoke by animation event(Priest Heal)
        public void OnExecuteHeal()
        {
            if (m_healable != null && !m_healable.IsDead)
            {
                m_healable.BeingHealed(m_currentPoint);
                EnterColdDown();
            }
        }

        public override void UnInitialize()
        {
            base.UnInitialize();

            if (m_healable != null)
            {
                BindingStatus(false);
                RemoveListener();

                ClearTarget();
            }
        }
    }
}