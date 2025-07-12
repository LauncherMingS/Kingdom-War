using UnityEngine;
using Assets.Version2.GameEnum;

namespace Assets.Version2.Pool
{
    public class ObjectPoolBaseSO : ScriptableObject
    {
        [SerializeField] protected bool m_hasBeenInitialized = false;
        [SerializeField] protected bool m_hasBeenPrewarmed = false;
#if UNITY_EDITOR
        [SerializeField] protected Transform m_root;
        
        public Transform Root
        {
            set => m_root = value;
        }
#endif

        [SerializeField] protected Group m_group;
        [SerializeField] protected UnitType m_unitType;

        public bool HasBeenInitialized => m_hasBeenInitialized;

        public bool HasBeenPrewarmed => m_hasBeenPrewarmed;

        public Group GetGroup => m_group;

        public UnitType GetUnitType => m_unitType;


        public virtual void Prewarm(int defaultCapacity = 10) { }

        public virtual void Initialize(int defaultCapacity = 10) { }

        public virtual void UnInitialize() { }
    }
}