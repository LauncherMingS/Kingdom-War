using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Component Reference")]
        [SerializeField] private Detector m_detector;

        [SerializeField] private Movement m_movement;
        [SerializeField] private Vector3 m_defaultPosition;

        private void Update()
        {
            Transform t_target = m_detector.DetectClosestTarget();
            Vector3 t_targetPosition = t_target ? t_target.position : m_defaultPosition;

            m_movement.MoveTo(t_targetPosition);
        }

        private void Awake()
        {
            m_movement.Initialize();
        }
    }
}