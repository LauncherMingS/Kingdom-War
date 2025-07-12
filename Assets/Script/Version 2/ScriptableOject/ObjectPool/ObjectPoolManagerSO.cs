using System.Collections.Generic;
using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.Pool
{
    public sealed class ObjectPoolManagerSO : ScriptableObject
    {
        public static ObjectPoolManagerSO Instance { get; private set; }

        private const int m_LeftShift = 8;
        private readonly Dictionary<int, ObjectPoolBaseSO> m_pools = new();


        private ObjectPoolBaseSO FindPool(int key)
        {
            if (!m_pools.TryGetValue(key, out ObjectPoolBaseSO pool))
            {
                GameManager.LogWarningEditor($"{name}: Cannot find the corresponding pool.");

                return null;
            }

            return pool;
        }

        private int GetKey(Group group, UnitType unitType)
        {
            return (int)group << m_LeftShift | (int)unitType;
        }

        public T Get<T>(Group group, UnitType unitType) where T : Component
        {
            return (FindPool(GetKey(group, unitType)) as ObjectPoolSO<T>).Get();
        }

        public IEnumerable<T> GetMany<T>(Group group, UnitType unitType, int num) where T : Component
        {
            return (FindPool(GetKey(group, unitType)) as ObjectPoolSO<T>).GetMany(num);
        }

        public void Recycle<T>(Group group, UnitType unitType, T element) where T : Component
        {
            (FindPool(GetKey(group, unitType)) as ObjectPoolSO<T>).Recycle(element);
        }

        public void RecycleMany<T>(Group group, UnitType unitType, IEnumerable<T> elements) where T : Component
        {
            (FindPool(GetKey(group, unitType)) as ObjectPoolSO<T>).RecycleMany(elements);
        }

        //RegisterGroupPools目前先無腦將池子全部塞入，等後面其他腳本開發差不多，usageSettings也設定好
        //，再淘汰現在這個函式，使用下面那個
        public void RegisterPools(ObjectPoolBaseSO[] pools)
        {
            int t_poolLength = pools.Length;
            int t_key;
            for (int i = 0; i < t_poolLength; i++)
            {
                t_key = GetKey(pools[i].GetGroup, pools[i].GetUnitType);
                if (m_pools.TryGetValue(t_key, out ObjectPoolBaseSO pool))
                {
                    if (!pool.HasBeenPrewarmed)
                    {
                        pool.Prewarm();
                    }

                    continue;
                }

                pool = pools[i];
#if UNITY_EDITOR
                Transform t_poolRoot = new GameObject().transform;
                t_poolRoot.name = $"{pool.GetGroup} {pool.GetUnitType} Object Pool";
                pool.Root = t_poolRoot;
#endif
                pool.Initialize();

                m_pools.Add(t_key, pool);
            }
        }

        public void RegisterPools(ObjectPoolBaseSO[] pools, bool[] usageSettings)
        {
            int t_poolLength = pools.Length;
            int t_key;
            for (int i = 0; i < t_poolLength; i++)
            {
                t_key = GetKey(pools[i].GetGroup, pools[i].GetUnitType);
                if (m_pools.TryGetValue(t_key, out ObjectPoolBaseSO pool))
                {
                    if (usageSettings[i] && !pool.HasBeenPrewarmed)
                    {
                        pool.Prewarm();
                    }
                    else if (!usageSettings[i] && pool.HasBeenPrewarmed)
                    {
                        pool.UnInitialize();
                    }

                    continue;
                }

                if (!usageSettings[i])
                {
                    continue;
                }

                pool = pools[i];
#if UNITY_EDITOR
                Transform t_poolRoot = new GameObject().transform;
                t_poolRoot.name = $"{pool.GetGroup} {pool.GetUnitType} Object Pool";
                pool.Root = t_poolRoot;
#endif
                pool.Initialize();

                m_pools.Add(t_key, pool);
            }
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