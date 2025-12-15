using UnityEngine;

namespace Assets.Version2.DynamicQuadTree
{
    [System.Serializable]
    public struct AABB
    {
        //position(m_minX, m_minY)
        [SerializeField] private float m_centerX;
        [SerializeField] private float m_centerY;
        [SerializeField] private float m_width;
        [SerializeField] private float m_height;

        [SerializeField] private float m_minX;
        [SerializeField] private float m_minY;

        [SerializeField] private float m_maxX;
        [SerializeField] private float m_maxY;

        public float MinX => m_minX;

        public float MinY => m_minY;

        public float Width => m_width;

        public float Height => m_height;

        public float MaxX => m_maxX;

        public float MaxY => m_maxY;

        public float CenterX => m_centerX;
        
        public float CenterY => m_centerY;


        public AABB(float minX, float minY, float width, float height)
        {
            m_minX = minX;
            m_minY = minY;
            m_width = width;
            m_height = height;

            m_maxX = m_minX + m_width;
            m_maxY = m_minY + m_height;

            m_centerX = (m_minX + m_maxX) / 2f;
            m_centerY = (m_minY + m_maxY) / 2f;
        }

        //Circle build(Square)
        public AABB(float centerX, float centerY, float side)
        {
            m_centerX = centerX;
            m_centerY = centerY;
            m_height = m_width = side;

            float t_halfSide = side / 2f;
            m_minX = m_centerX - t_halfSide;
            m_minY = m_centerY - t_halfSide;

            m_maxX = m_centerX + t_halfSide;
            m_maxY = m_centerY + t_halfSide;
        }

        public bool IsContain(Vector2 position)
        {
            return position.x >= m_minX && position.x < m_maxX
                && position.y >= m_minY && position.y < m_maxY;
        }

        public bool IsOverlap(AABB box)
        {
            return box.MaxX > m_minX && box.MaxY > m_minY
                && box.MinX < m_maxX && box.MinY < m_maxY;
        }
    }
}