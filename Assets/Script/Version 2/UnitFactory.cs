using UnityEngine;

namespace Assets.Version2
{
    public class UnitFactory : MonoBehaviour
    {
        [SerializeField] private Controller m_controller;

        [SerializeField] private GameObject m_infantryPrefab;

        public void Recruit()
        {
            GameObject t_gameObject = Instantiate(m_infantryPrefab, transform.position, Quaternion.identity);
            Unit t_unit = t_gameObject.GetComponent<Unit>();
            t_unit.SetController = m_controller;

            m_controller.AddUnit(t_unit);
        }
    }
}