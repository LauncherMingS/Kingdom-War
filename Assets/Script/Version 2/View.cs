using UnityEngine;

namespace Assets.Version2
{
    public class View : MonoBehaviour
    {
        public static readonly int NLI_Group = 1 << 7;

        [SerializeField] private SpriteRenderer m_spriteRenderer;

        public void Face(float positionX, int group)
        {
            bool t_face = true;
            if (transform.position.x > positionX || (group & NLI_Group) != 0)
            {
                t_face = false;
            }

            m_spriteRenderer.flipX = t_face;
        }
    }
}