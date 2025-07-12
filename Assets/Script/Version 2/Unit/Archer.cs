using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class Archer : Unit
    {
        [SerializeField] protected RangedAttackHandler m_attackHandler;


        protected void TryAttackTarget()
        {
            SwitchUnitState(UnitState.Attack);
            if (m_attackHandler.CurrentCD == 0f && m_view.AnimationIsDone((int)m_currentState))
            {
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
                float t_targetDistanceZ = Mathf.Abs(t_targetPosition.z - transform.position.z);
                if (m_attackHandler.ProjectileRadius >= t_targetDistanceZ)
                {
                    TryAttackTarget();
                    return;
                }

                t_targetPosition.x = transform.position.x;
                MoveTo(t_targetPosition, t_targetDistance, deltaTime);
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
            m_controller.RemoveUnit(UnitType.Archer, this);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_attackHandler.Initialize(m_detectionHandler.TargetLayer);
        }

        protected override void Update()
        {
            float t_deltaTime = Time.deltaTime;
            m_attackHandler.UpdateCD(t_deltaTime);


            switch (m_controller.CurrentCommand)
            {
                case Command.Attack:
                case Command.Defend:
                    HandleAttackCommand(t_deltaTime);
                    return;
                case Command.Retreat:
                    HandleRetreatCommand(t_deltaTime);
                    return;
            }
        }
    }
}