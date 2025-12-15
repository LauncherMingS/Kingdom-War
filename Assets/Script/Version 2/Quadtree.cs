using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2.DynamicQuadTree
{
    public class Quadtree : ScriptableObject
    {
        public static Quadtree m_instance;

        [SerializeField] private QuadtreeNode m_root;
        [SerializeField] private Dictionary<Unit, QuadtreeNode> m_backrefs;
        [SerializeField] private Unit[] m_resultBuffer;
        [SerializeField] private Unit[] m_tempBuffer;
        [SerializeField] private int m_resultIndex;
        [SerializeField] private int m_tempIndex;

        [SerializeField] private float m_mergeCD;
        [SerializeField] private float m_mergeCurrentCD;

        public static Quadtree Instance => m_instance;

        public QuadtreeNode Root => m_root; //draw use


        public bool Register(Unit obj) => m_root.Insert(obj, m_backrefs);

        public void Unregister(Unit obj) => m_root.Remove(obj, m_backrefs);

        public void UpdateObject(Unit obj)
        {
            if (obj == null)
            {
                GameManager.LogWarningEditor("QuadTree: Passed parameter obj is null.");
                return;
            }

            if (m_backrefs.TryGetValue(obj, out QuadtreeNode node))
            {
                node.Remove(obj, m_backrefs);
            }

            m_root.Insert(obj, m_backrefs);
        }

        public void UpdateMergeCD(float deltaTime)
        {
            m_mergeCurrentCD = Mathf.Max(m_mergeCurrentCD - deltaTime, 0f);
            if (m_mergeCurrentCD == 0f)
            {
                m_mergeCurrentCD = m_mergeCD;
                m_root.TryMerge(m_backrefs);
            }
        }

        public Unit[] QueryRect(AABB box, int layerMask)
        {
            ClearBuffer(false);

            m_root.QueryAABB(box, layerMask, m_resultBuffer, ref m_resultIndex);
            return m_resultBuffer;
        }

        public Unit[] QueryCircle(Vector2 center, float radius, int layerMask)
        {
            ClearBuffer(false);

            AABB t_circleAABB = new(center.x, center.y, radius);
            m_root.QueryCircle(t_circleAABB, layerMask, m_resultBuffer, ref m_resultIndex);
            return m_resultBuffer;
        }

        public Unit[] QuerySector(Vector2 origin, Vector2 forward, float radius, float halfDeg, int layerMask)
        {
            ClearBuffer(true);

            float t_halfRad = halfDeg * Mathf.Deg2Rad;
            Vector2 t_rightDir = new(forward.x * Mathf.Cos(t_halfRad) - forward.y * Mathf.Sin(t_halfRad)
                , forward.x * Mathf.Sin(t_halfRad) + forward.y * Mathf.Cos(t_halfRad));
            Vector2 t_leftDir = new(forward.x * Mathf.Cos(-t_halfRad) - forward.y * Mathf.Sin(-t_halfRad)
                , forward.x * Mathf.Sin(-t_halfRad) + forward.y * Mathf.Cos(-t_halfRad));

            Vector2 t_point1 = origin + t_rightDir * radius;
            Vector2 t_point2 = origin + t_leftDir * radius;

            float t_minX = Mathf.Min(origin.x, Mathf.Min(t_point1.x, t_point2.x));
            float t_minY = Mathf.Min(origin.y, Mathf.Min(t_point1.y, t_point2.y));
            float t_maxX = Mathf.Max(origin.x, Mathf.Max(t_point1.x, t_point2.x));
            float t_maxY = Mathf.Max(origin.y, Mathf.Max(t_point1.y, t_point2.y));
            AABB t_box = new(t_minX, t_minY, t_maxX - t_minX, t_maxY - t_minY);
            m_root.QueryAABB(t_box, layerMask, m_tempBuffer, ref m_tempIndex);

            float t_radiusSqr = radius * radius;
            float t_forwardSqr = forward.x * forward.x + forward.y * forward.y;
            float t_cosTheta = Mathf.Cos(t_halfRad);
            float t_cosThetaSqr = t_cosTheta * t_cosTheta;
            Vector2 t_vector;
            float t_distanceSqr;
            float t_dot;
            for (int i = 0; i < m_tempIndex; i++)
            {
                t_vector = m_tempBuffer[i].Pos2D - origin;
                t_distanceSqr = t_vector.x * t_vector.x + t_vector.y * t_vector.y;
                if (t_distanceSqr > t_radiusSqr)
                {
                    continue;
                }

                t_dot = forward.x * t_vector.x + forward.y * t_vector.y;
                if (t_dot <= 0 || t_dot * t_dot < t_cosThetaSqr * t_forwardSqr * t_distanceSqr)
                {
                    continue;
                }

                m_resultBuffer[m_resultIndex++] = m_tempBuffer[i];
            }

            return m_resultBuffer;
        }

        public void ClearBuffer(bool isClearAll)
        {
            if (!isClearAll)
            {
                for (int i = 0; i < m_resultIndex; i++)
                {
                    m_resultBuffer[i] = null;
                }
                m_resultIndex = 0;
            }
            else
            {
                for (int i = 0; i < m_tempIndex; i++)
                {
                    m_tempBuffer[i] = null;
                    m_resultBuffer[i] = null;
                }
                m_resultIndex = m_tempIndex = 0;
            }
        }

        public void Initialize(QuadtreeSetting setting)
        {
            m_root = new()
            {
                //Debug
                Name = "Root",
                Depth = 0
            };
            m_root.Initialize(setting.WorldBounds, setting.NodeCapacity);
            m_resultBuffer = new Unit[setting.BufferLength];
            m_tempBuffer = new Unit[m_resultBuffer.Length];
            m_tempIndex = m_resultIndex = 0;

            m_mergeCurrentCD = m_mergeCD = setting.MergeCD;

            m_backrefs = new();
        }

        public void OnEnable()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else if (m_instance != this)
            {
                Destroy(this);
            }
        }
    }
}