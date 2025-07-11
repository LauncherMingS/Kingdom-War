using System.Collections.Generic;
using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.Factory
{
    public class CentralFactorySO : ScriptableObject
    {
        public static CentralFactorySO Instance { get; private set; }

        private const int m_LeftShift = 8;
        private readonly Dictionary<int, FactoryBaseSO> m_factories = new();


        public T Create<T>(Group group, UnitType unitType) where T : Component
        {
            int t_key = (int)group << m_LeftShift | (int)unitType;

            if (!m_factories.TryGetValue(t_key, out FactoryBaseSO factory))
            {
#if UNITY_EDITOR
                Debug.LogAssertion($"Central factory don't have this {typeof(T)} of factory.");
#endif
                return default;
            }

            return (factory as FactorySO<T>).Create();
        }

        public void Register(FactoryBaseSO[] factories)
        {
            int t_factoriesLength = factories.Length;
            FactoryBaseSO t_factory;
            int t_key;
            for (int i = 0; i < t_factoriesLength; i++)
            {
                t_factory = factories[i];
                t_key = (int)t_factory.GetGroup << m_LeftShift | (int)t_factory.GetUnitType;
                if (m_factories.ContainsKey(t_key))
                {
                    continue;
                }

                m_factories.Add(t_key, t_factory);
            }

            Debug.Log(m_factories.Count);
        }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
        }
    }
}