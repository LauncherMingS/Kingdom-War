using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class SwordMan : Unit
    {
        [SerializeField] protected MeleeAttackHandler m_attackHandler;

        public override InteractHandler Interact => m_attackHandler;


        protected override void HandleAttackCommand(float deltaTime)
        {
            //Find target through DetectionHandler
            Vector3 t_detectionCenter = GetDetectionCenterByCommand(m_controller.CurrentCommand);
            float t_detectionRadius = GetDetectionRadiusByCommand(m_controller.CurrentCommand);
            Transform t_target = m_detectionHandler.DetectClosestTarget(t_detectionCenter, t_detectionRadius
                , out float t_targetSquaredDistance);

            //Set target or enemy base position/defense position related data
            Vector3 t_targetPosition;
            float t_targetDistance;
            if (t_target != null)
            {
                t_targetPosition = t_target.position;
                t_targetDistance = Mathf.Sqrt(t_targetSquaredDistance);
            }
            else
            {
                t_targetPosition = (m_controller.CurrentCommand == Command.Attack)
                    ? m_enemyBasePosition : m_defensePosition;
                t_targetDistance = Vector3.Distance(transform.position, t_targetPosition);
            }

            m_view.Face(t_targetPosition.x);

            if (t_target != null && m_attackHandler.Range >= t_targetDistance)
            {
                TryAttackOrHealTarget(t_target);
            }
            else if (t_targetDistance > 0f)
            {
                MoveTo(t_targetPosition, t_targetDistance, deltaTime);
            }
            else
            {
                SwitchUnitState(UnitState.Idle);
            }
        }

        protected override void NotifyWhenDying(float attackPoint)
        {
            m_controller.RemoveUnit(UnitType.SwordMan, this);
        }
    }
}