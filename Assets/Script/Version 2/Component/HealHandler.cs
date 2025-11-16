using UnityEngine;
using Assets.Version2.StatusEffectSystem;

namespace Assets.Version2
{
    public class HealHandler : InteractHandler, IHealing
    {
        [SerializeField] protected IHealable m_healable;
        [SerializeField] protected bool m_isBusyOnHealing;

        [SerializeField] protected StatusEffectDataSO m_data;
        [SerializeField] protected int m_level;

        public override Unit Target
        {
            get => m_target;
            set
            {
                m_target = value;
                if (m_target == null || m_target.Healable == null)
                {
                    ClearTarget();
                    return;
                }

                m_healable = m_target.Healable;
                if (CheckStopHeal())
                {
                    return;
                }

                BindingStatus(true);
                RegisterListener();
            }
        }

        public IHealable Healable => m_healable;

        public bool IsBusyOnHealing => m_isBusyOnHealing;


        public void ReleaseSkill(Unit holder)
        {
            //Check if the target has StatusManager
            if (Target.StatusManager == null)
            {
                GameManager.LogWarningEditor($"{name}: No target or StatusManager.");
                return;
            }

            //Apply the status effect
            bool t_isHaveModifier = Target.StatusManager.ApplyModifier(m_data, m_level, Target, holder);
            //If return true, meaning the current target has the status effect,
            //and if the status effect is unique(cannot stack and cannot refresh),
            //do not enter cold down.
            if (t_isHaveModifier && !m_data[m_level].CanStack && !m_data[m_level].CanRefresh)
            {
                return;
            }
            
            EnterColdDown();
        }

        protected override void ClearTarget()
        {
            base.ClearTarget();

            if (m_healable != null)
            {
                BindingStatus(false);
                RemoveListener();
            }
            m_healable = null;
        }

        protected void BindingStatus(bool isBinding)
        {
            m_isBusyOnHealing = isBinding;
            m_healable.IsAcceptHealed = isBinding;
        }

        protected void RegisterListener()
        {
            m_healable.OnHealed += CheckStopHeal;
            m_healable.OnDying += CheckStopHeal;
        }

        protected void RemoveListener()
        {
            m_healable.OnHealed -= CheckStopHeal;
            m_healable.OnDying -= CheckStopHeal;
        }

        //There are three situations that will interrupt the heal
        //1. Hurted unit is full HP(m_healable.OnHealed Invoke)
        //2. Hurted unit dies(m_healable.OnDying Invoke)
        //3. Priest dies(UnInitialize())
        protected bool CheckStopHeal()
        {
            if (m_healable.IsDead || m_healable.IsFullHP)
            {
                ClearTarget();
                return true;
            }

            return false;
        }

        protected void CheckStopHeal(float point)
        {
            CheckStopHeal();
        }
    }
}