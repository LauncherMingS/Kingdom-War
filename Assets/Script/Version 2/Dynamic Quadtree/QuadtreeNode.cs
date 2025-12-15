using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2.DynamicQuadTree
{
    public class QuadtreeNode
    {
        public string Name;//Debug
        [SerializeField] private List<Unit> m_objects;
        [SerializeField] private QuadtreeNode[] m_children;
        [SerializeField] private AABB m_bounds;
        [SerializeField] private int m_capacity;
        public int Depth;

        public AABB Bounds => m_bounds; //draw use

        public List<Unit> Objects => m_objects; //draw use

        public QuadtreeNode[] Children => m_children; // draw use

        public bool IsDivided => m_children != null;


        public bool IsContain(Unit obj) => m_bounds.IsContain(obj.Pos2D);

        public bool Insert(Unit obj, Dictionary<Unit, QuadtreeNode> backrefs)
        {
            if (!m_bounds.IsContain(obj.Pos2D))
            {
                return false;
            }

            if (m_objects.Count < m_capacity && !IsDivided)
            {
                m_objects.Add(obj);
                backrefs[obj] = this;

                return true;
            }

            if (!IsDivided)
            {
                Subdivide(backrefs);
            }

            for (int i = 0; i < 4; i++)
            {
                if (m_children[i].Insert(obj, backrefs))
                {
                    return true;
                }
            }

            //Exception
            GameManager.LogWarningEditor($"QuadtreeNode: Contain {obj.name}, but it's children fail to insert {obj.name}.");
            return false;
        }

        public bool Remove(Unit obj, Dictionary<Unit, QuadtreeNode> backrefs)
        {
            if (IsDivided)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m_children[i].Remove(obj, backrefs))
                    {
                        TryMerge(backrefs);

                        return true;
                    }
                }
            }

            if (m_objects.Remove(obj))
            {
                backrefs.Remove(obj);

                return true;
            }

            return false;
        }

        private void Subdivide(Dictionary<Unit, QuadtreeNode> backrefs)
        {
            m_children = new QuadtreeNode[4];
            float t_halfWidth = m_bounds.Width / 2f;
            float t_halfHeight = m_bounds.Height / 2f;
            Vector2 t_minPoint = new(m_bounds.MinX, m_bounds.MinY);

            m_children[0] = new QuadtreeNode();
            m_children[0].Initialize(new AABB(t_minPoint.x + t_halfWidth, t_minPoint.y + t_halfHeight, t_halfWidth, t_halfHeight), m_capacity);
            m_children[0].Depth = Depth + 1;
            m_children[0].Name = $"Right Top [{Depth + 1}]";

            m_children[1] = new QuadtreeNode();
            m_children[1].Initialize(new AABB(t_minPoint.x, t_minPoint.y + t_halfHeight, t_halfWidth, t_halfHeight), m_capacity);
            m_children[1].Depth = Depth + 1;
            m_children[1].Name = $"Left Top [{Depth + 1}]";

            m_children[2] = new QuadtreeNode();
            m_children[2].Initialize(new AABB(t_minPoint.x, t_minPoint.y, t_halfWidth, t_halfHeight), m_capacity);
            m_children[2].Depth = Depth + 1;
            m_children[2].Name = $"Left Bottom [{Depth + 1}]";

            m_children[3] = new QuadtreeNode();
            m_children[3].Initialize(new AABB(t_minPoint.x + t_halfWidth, t_minPoint.y, t_halfWidth, t_halfHeight), m_capacity);
            m_children[3].Depth = Depth + 1;
            m_children[3].Name = $"Right Bottom [{Depth + 1}]";

            //redistribute
            for (int i = m_objects.Count - 1; i >= 0; i--)
            {
                Unit t_object = m_objects[i];
                for (int j = 0; j < 4; j++)
                {
                    if (m_children[j].m_bounds.IsContain(t_object.Pos2D))
                    {
                        m_children[j].m_objects.Add(t_object);
                        backrefs[t_object] = m_children[j];
                        m_objects.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public bool TryMerge(Dictionary<Unit, QuadtreeNode> backrefs)
        {
            if (!IsDivided)
            {
                return true;
            }

            //First determine if merging is possible
            int m_totalObjectNum = 0;
            for (int i = 0; i < 4; i++)
            {
                m_totalObjectNum += m_children[i].m_objects.Count;
                if (!m_children[i].TryMerge(backrefs) || m_children[i].IsDivided || m_totalObjectNum > m_capacity)
                {
                    return false;
                }
            }

            //Merge
            List<Unit> t_objects;
            for (int i = 0; i < 4; i++)
            {
                t_objects = m_children[i].m_objects;
                for (int j = 0; j < m_children[i].m_objects.Count; j++)
                {
                    m_objects.Add(t_objects[j]);
                    backrefs[t_objects[j]] = this;
                }
            }

            m_children = null;

            return true;
        }

        public void QueryAABB(AABB box, int layerMask, Unit[] buffer, ref int bufferIndex)
        {
            if (!m_bounds.IsOverlap(box))
            {
                return;
            }

            if (IsDivided)
            {
                for (int i = 0; i < m_children.Length; i++)
                {
                    m_children[i].QueryAABB(box, layerMask, buffer, ref bufferIndex);
                }

                return;
            }

            for (int i = 0; i < m_objects.Count; i++)
            {
                if ((layerMask & m_objects[i].LayerMask) == 0 || !box.IsContain(m_objects[i].Pos2D))
                {
                    continue;
                }

                buffer[bufferIndex++] = m_objects[i];
            }
        }

        public void QueryCircle(AABB square, int layerMask, Unit[] buffer, ref int bufferIndex)
        {
            if (!m_bounds.IsOverlap(square))
            {
                return;
            }

            if (IsDivided)
            {
                for (int i = 0; i < m_children.Length; i++)
                {
                    m_children[i].QueryCircle(square, layerMask, buffer, ref bufferIndex);
                }

                return;
            }

            Vector2 t_center = new(square.CenterX, square.CenterY);
            float t_radiusSqr = square.Width * square.Width;
            for (int i = 0; i < m_objects.Count; i++)
            {
                if ((layerMask & m_objects[i].LayerMask) == 0 || !square.IsContain(m_objects[i].Pos2D))
                {
                    continue;
                }

                if (Vector2.SqrMagnitude(t_center - m_objects[i].Pos2D) <= t_radiusSqr)
                {
                    buffer[bufferIndex++] = m_objects[i];
                }
            }
        }

        public void Initialize(AABB bounds, int capacity = 8)
        {
            m_bounds = bounds;
            m_capacity = capacity;
            m_objects = new List<Unit>();
            m_children = null;
        }
    }
}