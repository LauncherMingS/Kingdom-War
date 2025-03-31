using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private int m_group = 64;//LayerMask

        [Header("Component Reference")]
        [SerializeField] private Health m_health;

        [SerializeField] private Interaction m_interaction;

        [SerializeField] private Movement m_movement;
        [SerializeField] private Vector3 m_defaultPosition;

        [SerializeField] private Detector m_detector;

        [SerializeField] private View m_view;

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

            if (t_target && m_interaction.Range >= t_targetDistance)
            {
                if (m_interaction.CurrentCD == 0f)
                {
                    m_interaction.Interact(t_target);
                }
            }
            else
            {
                m_movement.MoveTo(t_targetPosition, t_targetDistance, t_deltaTime);
            }
        }

        private void Awake()
        {
            m_health.Initialize();
            m_interaction.Initialize();
            m_movement.Initialize();
        }
    }
}