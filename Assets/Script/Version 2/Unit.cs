using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Component Reference")]
        [SerializeField] private Health m_health;

        [SerializeField] private Movement m_movement;
        [SerializeField] private Vector3 m_defaultPosition;

        [SerializeField] private Detector m_detector;

        private void Update()
        {
            Transform t_target = m_detector.DetectClosestTarget();
            Vector3 t_targetPosition = t_target ? t_target.position : m_defaultPosition;

            m_movement.MoveTo(t_targetPosition);
        }

        private void Awake()
        {
            m_health.Initialize();
            m_movement.Initialize();
        }
    }
}