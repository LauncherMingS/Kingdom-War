using UnityEngine;
using static Assets.Version2.SwordMan;

namespace Assets.Version2
{
    public class Archer : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private Controller m_controller;

        [Header("Parameter")]
        [Header("Basics")]
        [SerializeField] private UnitState m_currentState = UnitState.Idle;
        [SerializeField] private int m_group;
        [Header("Detection")]
        [SerializeField] private float m_attackDetectionRadius;
        [SerializeField] private float m_defenseDetectionRadius;
        [Header("Position")]
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_defensePosition;
        [SerializeField] private Vector3 m_retreatPosition;

        [Header("Component Reference")]
        [SerializeField] private AttackHandler m_attackHandler;
        [SerializeField] private DetectionHandler m_detectionHandler;
        [SerializeField] private Health m_health;
        [SerializeField] private Movement m_movement;
        [SerializeField] private View m_view;


        public Controller SetController
        {
            set => m_controller = value;
        }

        public Vector3 EnemyBasePosition
        {
            set => m_enemyBasePosition = value;
        }

        public Vector3 DefensePosition
        {
            set => m_defensePosition = value;
        }

        public Vector3 RetreatPosition
        {
            set => m_retreatPosition = value;
        }

        //Switch unit's state and animation
        public void SwitchUnitState(UnitState newUnitState)
        {
            if (m_currentState == newUnitState)
            {
                return;
            }

            m_view.SwitchAnimation((int)newUnitState, (int)m_currentState);
            m_currentState = newUnitState;
        }

        private void TryAttackTarget(Transform target)
        {
            SwitchUnitState(UnitState.Attack);
            if (m_attackHandler.CurrentCD == 0f && m_view.AnimationIsDone((int)m_currentState))
            {
                m_attackHandler.SetTarget(target);
                m_view.ResetAnimation((int)m_currentState);
            }
        }

        private void MoveTo(Vector3 position, float distance, float deltaTime)
        {
            SwitchUnitState(UnitState.Move);
            m_movement.Move(position, distance, deltaTime);
        }

        private Vector3 GetDetectionCenterByCommand(Controller.Command command)
        {
            return command switch
            {
                Controller.Command.Attack => transform.position,
                Controller.Command.Defend => m_defensePosition,
                _ => Vector3.zero
            };
        }

        private float GetDetectionRadiusByCommand(Controller.Command command)
        {
            return command switch
            {
                Controller.Command.Attack => m_attackDetectionRadius,
                Controller.Command.Defend => m_defenseDetectionRadius,
                _ => 0f
            };
        }

        private void HandleAttackCommand(float deltaTime)
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
                t_targetPosition = (m_controller.CurrentCommand == Controller.Command.Attack)
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

        private void HandleRetreatCommand(float deltaTime)
        {
            float t_targetDistance = Vector3.Distance(transform.position, m_retreatPosition);

            m_view.Face(m_retreatPosition.x);

            if (t_targetDistance > 0f)
            {
                MoveTo(m_retreatPosition, t_targetDistance, deltaTime);
            }
            else
            {
                SwitchUnitState(UnitState.Idle);
            }
        }

        private void Update()
        {
            float t_deltaTime = Time.deltaTime;
            m_attackHandler.UpdateCD(t_deltaTime);


            switch (m_controller.CurrentCommand)
            {
                case Controller.Command.Attack:
                case Controller.Command.Defend:
                    HandleAttackCommand(t_deltaTime);
                    return;
                case Controller.Command.Retreat:
                    HandleRetreatCommand(t_deltaTime);
                    return;
            }
        }

        private void Start()
        {
            m_health.Initialize();
            m_attackHandler.Initialize();
            m_movement.Initialize();
            m_detectionHandler.Initialize(m_group);
            m_view.Initialize(m_group);
        }

        private void OnDisable()
        {
            m_view.Uninitialize();
        }
    }
}