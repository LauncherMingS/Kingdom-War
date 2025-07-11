using UnityEngine;

namespace Assets.Version2.Factory
{
    public class FactorySO<T> : FactoryBaseSO where T : Component
    {
        public virtual T Prefab { get; }


        public T Create()
        {
            return Instantiate(Prefab);
        }
    }
}