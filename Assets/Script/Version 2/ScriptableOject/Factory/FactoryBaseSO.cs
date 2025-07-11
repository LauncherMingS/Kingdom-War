using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.Factory
{
    public class FactoryBaseSO : ScriptableObject
    {
        [SerializeField] private Group m_group;
        [SerializeField] private UnitType m_unitType;

        public Group GetGroup
        {
            get => m_group;
        }

        public UnitType GetUnitType
        {
            get => m_unitType;
        }
    }
}