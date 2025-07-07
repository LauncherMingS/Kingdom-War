using UnityEngine;

namespace Assets.Version2
{
    public abstract class FactoryBaseSO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}