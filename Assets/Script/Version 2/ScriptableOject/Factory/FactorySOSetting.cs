using UnityEngine;

namespace Assets.Version2.Factory
{
    [CreateAssetMenu(order = 1, menuName = "Scriptable Object/FactorySO/FactorySOSetting", fileName = "FactorySOSetting")]
    public class FactorySOSetting : ScriptableObject
    {
        [SerializeField] private FactoryBaseSO[] m_factories;

        public FactoryBaseSO[] Factories => m_factories;
    }
}