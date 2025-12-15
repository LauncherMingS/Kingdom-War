using UnityEngine;

namespace Assets.Version2.DynamicQuadTree
{
    [CreateAssetMenu(menuName = "Scriptable Object/Setting/Quadtree Setting", fileName = "Quadtree Setting")]
    public class QuadtreeSetting : ScriptableObject
    {
        [SerializeField] private int m_capacity;
        [SerializeField] private int m_bufferLength;
        [SerializeField] private float m_mergeCD;
        [Header("World bounds size")]
        [SerializeField] private float m_minX;
        [SerializeField] private float m_minY;
        [SerializeField] private float m_width;
        [SerializeField] private float m_height;

        public AABB WorldBounds => new(m_minX, m_minY, m_width, m_height);

        public int NodeCapacity => m_capacity;

        public int BufferLength => m_bufferLength;

        public float MergeCD => m_mergeCD;
    }
}