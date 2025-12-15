using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    public class WeakenedStatusModifier : StatusModifierBase
    {
        [SerializeField] protected StatusEffectManager m_targetStatusManager;


        public void UpdateEffectPoint()
        {
            m_currentPoint = 1f + m_sourceData.BasePoint * m_currentStack;
        }

        public void Initialize(StatusEffectDataSO data, Unit target)
        {
            base.OnInitialize(data, target);

            m_targetStatusManager = target.StatusManager;

            UpdateEffectPoint();
        }

        public override void OnStack()
        {
            m_targetStatusManager.RemoveDamageModifier(m_currentPoint);

            base.OnStack();
            UpdateEffectPoint();

            m_targetStatusManager.AddDamageModifier(m_currentPoint);
        }

        public override void OnApply()
        {
            base.OnApply();
            m_targetStatusManager.AddDamageModifier(m_currentPoint);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            m_targetStatusManager.RemoveDamageModifier(m_currentPoint);
        }
    }
}