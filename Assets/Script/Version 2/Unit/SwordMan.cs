using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class SwordMan : Unit
    {
        [SerializeField] protected MeleeAttackHandler m_attackHandler;

        public override InteractHandler Interact => m_attackHandler;

        protected void TryAttackTarget(Transform target)
        {
            SwitchUnitState(UnitState.Attack);
            if (m_attackHandler.CurrentCD == 0f && m_view.AnimationIsDone((int)m_currentState))
            {
                m_attackHandler.SetTarget(target);
                m_view.ResetAnimation((int)m_currentState);
            }
        }

        protected override void HandleAttackCommand(float deltaTime)
        {
            Vector3 t_detectionCenter = GetDetectionCenterByCommand(m_controller.CurrentCommand);
            float t_detectionRadius = GetDetectionRadiusByCommand(m_controller.CurrentCommand);
            Transform t_target = m_detectionHandler.DetectClosestTarget(t_detectionCenter, t_detectionRadius
                , out float t_targetSquaredDistance);

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
                TryAttackTarget(t_target);
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

        protected override void NotifyWhenDying()
        {
            m_controller.RemoveUnit(UnitType.SwordMan, this);
        }
    }
}