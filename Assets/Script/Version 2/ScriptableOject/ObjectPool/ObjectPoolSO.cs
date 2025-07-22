using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2.Pool
{
    public class ObjectPoolSO<T> : ObjectPoolBaseSO where T : Component
    {
        protected Queue<T> m_pool;
#if UNITY_EDITOR
        protected int m_counter;
#endif

        public virtual T Prefab { get; }


        protected virtual T Create()
        {
            T element = Instantiate(Prefab);
#if UNITY_EDITOR
            element.name = $"{Prefab.name} {m_counter++}";
            element.transform.SetParent(m_root);
#endif

            return element;
        }

        public T Get()
        {
            T element = (m_pool.Count > 0) ? m_pool.Dequeue() : Create();
            element.gameObject.SetActive(true);

            return element;
        }

        public IEnumerable<T> GetMany(int num = 1)
        {
            T[] t_elements = new T[num];
            for (int i = 0; i < num; i++)
            {
                t_elements[i] = Get();
            }

            return t_elements;
        }

        public void Recycle(T element)
        {
#if UNITY_EDITOR
            element.transform.SetParent(m_root);
#endif
            element.gameObject.SetActive(false);
            m_pool.Enqueue(element);
        }

        public void RecycleMany(IEnumerable<T> elements)
        {
            foreach (T element in elements)
            {
                Recycle(element);
            }
        }

        //ScriptableObject.CreateInstance called by Initialize()
        //or called by UnInitialize once
        public override void Prewarm(int defaultCapacity = 10)
        {
            if (m_hasBeenPrewarmed)
            {
                GameManager.LogWarningEditor($"{name}: Pool has already been prewarmed[{m_hasBeenPrewarmed}]");

                return;
            }

            for (int i = 0; i < defaultCapacity; i++)
            {
                Recycle(Create());
            }

            m_hasBeenPrewarmed = true;
        }

        //Only ScriptableObject.CreateInstance call
        public override void Initialize(int defaultCapacity = 10)
        {
            if (m_hasBeenInitialized)
            {
                GameManager.LogWarningEditor($"{name}: Pool has already been initialized[{m_hasBeenInitialized}]");

                return;
            }
#if UNITY_EDITOR
            m_counter = 0;
#endif
            m_pool = new Queue<T>(defaultCapacity);
            Prewarm(defaultCapacity);

            m_hasBeenInitialized = true;
        }

        //The level unneeded call
        public override void UnInitialize()
        {
            m_pool.Clear();
            m_hasBeenPrewarmed = false;
        }

#if UNITY_EDITOR
        private void OnDisable()
        {
            m_hasBeenInitialized = false;
            m_hasBeenPrewarmed = false;
            m_root = null;
            m_pool?.Clear();
        }
#endif
    }
}