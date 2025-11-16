using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class Priest : Unit
    {
        [SerializeField] private HealHandler m_healHandler;

        public bool IsBusyOnHealing => m_healHandler.IsBusyOnHealing;

        public override InteractHandler Interact => m_healHandler;


        //This unit is quite special.When a unit is hurt, it will invoke the FirstAid from Controller.
        //The Controller will look for a available(IsBusyOnHealing) priest and set the target to her.
        //So this unit does not use DetectionHandler to find targets.
        public bool SetHealTarget(Transform transform)
        {
            //Exclude self(because of setting that priest cannot heal herself)
            if (transform.GetHashCode() == this.transform.GetHashCode())
            {
                return false;
            }

            if (!transform.TryGetComponent(out Unit target))
            {
                return false;
            }

            m_healHandler.Target = target;
            if (m_healHandler.Healable == null)
            {
                return false;
            }

            return true;
        }

        protected override void HandleAttackCommand(float deltaTime)
        {
            Transform t_target = (m_healHandler.Target != null) ? m_healHandler.Target.transform : null;

            Vector3 t_targetPosition = (t_target != null) ? t_target.transform.position : m_defensePosition;
            float t_targetDistance = Vector3.Distance(transform.position, t_targetPosition);
            
            //When playing heal animation, interrupt the all action.
            if (m_currentState == UnitState.Attack && !m_view.AnimationIsDone((int)UnitState.Attack))
            {
                m_view.Face(t_targetPosition.x);
                return;
            }

            if (t_target != null && m_healHandler.Range >= t_targetDistance)
            {
                //Play healing animation and Execute heal
                if (Interact.CurrentCD == 0f)
                {
                    m_view.Face(t_targetPosition.x);
                    SwitchUnitState(UnitState.Attack);
                    m_view.ResetAnimation((int)UnitState.Attack);
                    m_healHandler.ReleaseSkill(this);

                    return;
                }

                //During the waiting period after healing, move to the indicated positoin,
                //but cannot move beyond the healing range(minus 0.5) of the healed unit
                //and is not at the indicated position.
                t_targetDistance += 0.5f;
                float t_defensePostionDistance = Vector3.Distance(transform.position, m_defensePosition);
                if (m_healHandler.Range > t_targetDistance && t_defensePostionDistance > 0f)
                {
                    t_targetPosition = m_defensePosition;
                    MoveTo(t_targetPosition, t_defensePostionDistance, deltaTime);
                }
                else
                {
                    SwitchUnitState(UnitState.Idle);
                }
            }
            else if (t_targetDistance > 0f)
            {
                MoveTo(t_targetPosition, t_targetDistance, deltaTime);
            }
            else
            {
                SwitchUnitState(UnitState.Idle);
            }

            m_view.Face(t_targetPosition.x);
        }

        protected override void NotifyWhenDying(float attackPoint)
        {
            m_controller.RemoveUnit(UnitType.Priest, this);
        }
    }
}