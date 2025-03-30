using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Component Reference")]
        [SerializeField] private Health m_health;

        [SerializeField] private Interaction m_interaction;

        [SerializeField] private Movement m_movement;
        [SerializeField] private Vector3 m_defaultPosition;

        [SerializeField] private Detector m_detector;

        private void Update()
        {
            float t_deltaTime = Time.deltaTime;
            m_interaction.UpdateCD(t_deltaTime);


            Transform t_target = m_detector.DetectClosestTarget(out float t_targetDistance);
            Vector3 t_targetPosition = t_target ? t_target.position : m_defaultPosition;

            if (t_target && m_interaction.Range >= t_targetDistance)
            {
                if (m_interaction.CurrentCD == 0f)
                {
                    m_interaction.Interact(t_target);
                }
            }
            else
            {
                m_movement.MoveTo(t_targetPosition);
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