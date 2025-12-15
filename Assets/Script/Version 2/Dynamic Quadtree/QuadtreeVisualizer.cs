using UnityEngine;

namespace Assets.Version2.DynamicQuadTree
{
    public class QuadtreeVisualizer : MonoBehaviour
    {
        private QuadtreeNode m_root;

        public bool IsDrawObject;
        public float DrawedObjectSize;


        private void DrawNode(QuadtreeNode node)
        {
            Vector3 t_center = new(node.Bounds.MinX + node.Bounds.Width / 2f, 0f, node.Bounds.MinY + node.Bounds.Height / 2f);
            Vector3 t_size = new(node.Bounds.Width, 0f, node.Bounds.Height);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(t_center, t_size);

            if (IsDrawObject && node.Objects != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < node.Objects.Count; i++)
                {
                    Gizmos.DrawWireSphere(new Vector3(node.Objects[i].Pos2D.x, 3f, node.Objects[i].Pos2D.y), DrawedObjectSize);
                }
            }

            if (node.IsDivided)
            {
                for (int i = 0; i < node.Children.Length; i++)
                {
                    DrawNode(node.Children[i]);
                }
            }
        }

        public void Initialize(QuadtreeNode root)
        {
            m_root = root;
        }

        private void OnDrawGizmos()
        {
            if (m_root == null)
            {
                return;
            }

            DrawNode(m_root);
        }
    }
}