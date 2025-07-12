using UnityEngine;

namespace Assets.Version2.Pool
{
    [CreateAssetMenu(order = 1, menuName = "Scriptable Object/ObjectPoolSO/ObjectPoolSettingSO", fileName = "ObjectPoolSettingSO")]
    public class ObjectPoolSettingSO : ScriptableObject
    {
        [SerializeField] private ObjectPoolBaseSO[] m_pools;

        public ObjectPoolBaseSO[] Pools => m_pools;
    }
}