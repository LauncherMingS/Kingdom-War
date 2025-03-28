using UnityEngine;

namespace Assets.Version2
{
    public class Unit : MonoBehaviour
    {
        [Header("Component Reference")]
        [SerializeField] private Movement m_movement;

        private void Awake()
        {
            m_movement.Initialize();
        }
    }
}