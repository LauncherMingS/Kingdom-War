using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private UnitState m_currentState = UnitState.Idle;
        [SerializeField] private int m_group;//LayerMask

        [Header("Component Reference")]
        [SerializeField] private Health m_health;

        [SerializeField] private Interaction m_interaction;

        [SerializeField] private Movement m_movement;
        [SerializeField] private Vector3 m_defaultPosition;

        [SerializeField] private Detector m_detector;

        [SerializeField] private View m_view;

        //Switch unit's state also switch unit's animation
        public void SwitchUnitState(UnitState newUnitState)
        {
            if (m_currentState == newUnitState)
            {
                return;
            }

            m_view.SwitchAnimation((int)newUnitState, (int)m_currentState);
            m_currentState = newUnitState;
        }

        private void Update()
        {
            float t_deltaTime = Time.deltaTime;
            m_interaction.UpdateCD(t_deltaTime);


            Vector3 t_targetPosition;
            float t_targetDistance;
            Transform t_target = m_detector.DetectClosestTarget(out float t_targetSquaredDistance);
            if (t_target)
            {
                t_targetPosition = t_target.position;
                t_targetDistance = Mathf.Sqrt(t_targetSquaredDistance);
            }
            else
            {
                t_targetPosition = m_defaultPosition;
                t_targetDistance = Vector3.Distance(transform.position, t_targetPosition);
            }

            m_view.Face(t_targetPosition.x, m_group);

            if (t_target != null && m_interaction.Range >= t_targetDistance)
            {
                SwitchUnitState(UnitState.Attack);
                if (m_interaction.CurrentCD == 0f && m_view.AnimationIsDone((int)m_currentState))
                {
                    m_view.ResetAnimation((int)m_currentState);
                    m_interaction.SetTarget(t_target);
                }
            }
            else
            {
                m_movement.MoveTo(t_targetPosition, t_targetDistance, t_deltaTime);
            }
        }

        private void Start()
        {
            m_health.Initialize();
            m_interaction.Initialize();
            m_movement.Initialize();
            m_detector.Initialize(m_group);
            m_view.Initialize();
        }

        private void OnDisable()
        {
            m_view.Uninitialize();
        }

        public enum UnitState : byte
        {
            Idle = 0,
            Move = 1,
            Attack = 2
        }
    }
}