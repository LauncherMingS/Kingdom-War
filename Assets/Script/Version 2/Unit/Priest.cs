using Assets.Version2.GameEnum;
using UnityEngine;

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
            if (transform.GetHashCode() == this.transform.GetHashCode())
            {
                return false;
            }

            m_healHandler.Target = transform;
            return true;
        }

        protected override void HandleAttackCommand(float deltaTime)
        {
            Transform t_target = m_healHandler.Target;

            Vector3 t_targetPosition = (t_target != null) ? t_target.transform.position : m_defensePosition;
            float t_targetDistance = Vector3.Distance(transform.position, t_targetPosition);

            if (t_target != null && m_healHandler.Range >= t_targetDistance)
            {
                //Must wait for the healing animation to finish playing
                if (m_view.AnimationIsDone((int)UnitState.Attack))
                {
                    //Play healing animation and Execute heal
                    if (Interact.CurrentCD == 0f)
                    {
                        m_view.Face(t_targetPosition.x);
                        SwitchUnitState(UnitState.Attack);
                        m_view.ResetAnimation((int)UnitState.Attack);
                        return;
                    }

                    //During the waiting period after healing, move to the indicated positoin,
                    //but cannot move beyond the healing range(minus 0.5) of the healed unit
                    //and is not at the indicated position.
                    t_targetDistance += 0.5f;
                    float t_defensePostionDistance = Vector3.Distance(transform.position, m_defensePosition);
                    if (m_healHandler.Range > t_targetDistance && t_defensePostionDistance > 0f)
                    {
                        m_view.Face(m_defensePosition.x);
                        MoveTo(m_defensePosition, t_defensePostionDistance, deltaTime);
                    }
                    else
                    {
                        m_view.Face(t_targetPosition.x);
                        SwitchUnitState(UnitState.Idle);
                    }
                }
            }
            else if (t_targetDistance > 0f)
            {
                m_view.Face(t_targetPosition.x);
                MoveTo(t_targetPosition, t_targetDistance, deltaTime);
            }
            else
            {
                m_view.Face(t_targetPosition.x);
                SwitchUnitState(UnitState.Idle);
            }
        }

        protected override void NotifyWhenDying(float attackPoint)
        {
            m_controller.RemoveUnit(UnitType.Priest, this);
        }
    }
}