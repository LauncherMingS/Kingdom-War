using System;
using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Game Reference")]
        [SerializeField] protected Controller m_controller;

        [Header("Parameter")]
        [SerializeField] protected UnitState m_currentState;
        [Header("Detection")]
        [SerializeField] protected float m_attackDetectionRadius;
        [SerializeField] protected float m_defenseDetectionRadius;
        [Header("Position")]
        [SerializeField] protected Vector3 m_enemyBasePosition;
        [SerializeField] protected Vector3 m_defensePosition;
        [SerializeField] protected Vector3 m_retreatPosition;

        [Header("Component Reference")]
        [SerializeField] protected DetectionHandler m_detectionHandler;
        [SerializeField] protected Health m_health;
        [SerializeField] protected Movement m_movement;
        [SerializeField] protected View m_view;

        public event Action<float> OnUpdateCD;

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

        public virtual InteractHandler Interact { get; }


        //Switch unit's state and animation
        protected void SwitchUnitState(UnitState newUnitState)
        {
            if (m_currentState == newUnitState)
            {
                return;
            }

            m_view.SwitchAnimation((int)newUnitState, (int)m_currentState);
            m_currentState = newUnitState;
        }

        protected void MoveTo(Vector3 position, float distance, float deltaTime)
        {
            SwitchUnitState(UnitState.Move);
            m_movement.Move(position, distance, deltaTime);
        }

        protected Vector3 GetDetectionCenterByCommand(Command command)
        {
            return command switch
            {
                Command.Attack => transform.position,
                Command.Defend => m_defensePosition,
                _ => Vector3.zero
            };
        }

        protected float GetDetectionRadiusByCommand(Command command)
        {
            return command switch
            {
                Command.Attack => m_attackDetectionRadius,
                Command.Defend => m_defenseDetectionRadius,
                _ => 0f
            };
        }

        protected abstract void HandleAttackCommand(float deltaTime);

        protected virtual void HandleRetreatCommand(float deltaTime)
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

        protected abstract void NotifyWhenDying();

        public virtual void Initialize()
        {
            m_controller = GameManager.Instance.GetController(gameObject.layer);
            Interact.Initialize();
            m_detectionHandler.Initialize();
            m_health.Initialize();
            m_movement.Initialize();
            m_view.Initialize();

            m_health.OnDying += NotifyWhenDying;
        }

        public virtual void UnInitialize()
        {
            Interact.UnInitialize();
            m_view.Uninitialize();

            m_health.OnDying -= NotifyWhenDying;
        }

        protected virtual void Update()
        {
            float t_deltaTime = Time.deltaTime;
            OnUpdateCD.Invoke(t_deltaTime);


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