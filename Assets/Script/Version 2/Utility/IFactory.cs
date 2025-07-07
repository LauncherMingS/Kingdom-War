using UnityEngine;

namespace Assets.Version2
{
    public interface IFactory<T>
    {
        public T Create();
    }
}