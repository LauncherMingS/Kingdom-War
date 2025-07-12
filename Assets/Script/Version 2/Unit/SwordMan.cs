using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public class SwordMan : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] private Controller m_controller;

        [Header("Parameter")]
        [Header("Basics")]
        [SerializeField] private UnitState m_currentState = UnitState.Idle;
        [Header("Detection")]
        [SerializeField] private float m_attackDetectionRadius = 5f;
        [SerializeField] private float m_defenseDetectionRadius = 3f;
        [Header("Position")]
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_defensePosition;
        [SerializeField] private Vector3 m_retreatPosition;

        [Header("Component Reference")]
        [SerializeField] private MeleeAttackHandler m_attackHandler;
        [SerializeField] private DetectionHandler m_detectionHandler;
        [SerializeField] private Health m_health;
        [SerializeField] private Movement m_movement;
        [SerializeField] private View m_view;

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

        private Vector3 GetDetectionCenterByCommand(Command command)
        {
            return command switch
            {
                Command.Attack => transform.position,
                Command.Defend => m_defensePosition,
                _ => Vector3.zero
            };
        }

        private float GetDetectionRadiusByCommand(Command command)
        {
            return command switch
            {
                Command.Attack => m_attackDetectionRadius,
                Command.Defend => m_defenseDetectionRadius,
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

        public void NotifyWhenDying()
        {
            m_controller.RemoveSwordMan(this);
        }

        public void Initialize()
        {
            m_attackHandler.Initialize();
            m_health.Initialize();
            m_movement.Initialize();
            m_detectionHandler.Initialize();
            m_view.Initialize();

            m_controller = GameManager.Instance.GetController(gameObject.layer);
            m_health.OnDying += NotifyWhenDying;
        }

        public void UnInitialize()
        {
            m_view.Uninitialize();

            m_health.OnDying -= NotifyWhenDying;
        }

        private void Update()
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