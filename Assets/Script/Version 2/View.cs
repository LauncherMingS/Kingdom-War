using System.Collections;
using UnityEngine;

namespace Assets.Version2
{
    public class View : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private float m_hurtFlashDuration = 0.35f;
        [SerializeField] private IEnumerator m_coroutine;
        [SerializeField] private bool m_coroutineStatus;

        public void Face(float positionX, int group)
        {
            bool t_face = true;
            if (transform.position.x > positionX || (group & GameManager.Instance.NLI) != 0)
            {
                t_face = false;
            }

            m_spriteRenderer.flipX = t_face;
        }

        public void HurtVisualEffect(float point)
        {
            if (m_coroutineStatus)
            {
                StopCoroutine(m_coroutine);
            }

            m_coroutine = HurtFlash();
            StartCoroutine(m_coroutine);
        }

        private IEnumerator HurtFlash()
        {
            m_coroutineStatus = true;

            m_spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(m_hurtFlashDuration);
            m_spriteRenderer.color = Color.white;

            m_coroutineStatus = false;
        }

        public void Initialize()
        {
            GetComponent<Health>().OnHurt += HurtVisualEffect;
            m_coroutineStatus = false;
        }

        public void Uninitialize()
        {
            GetComponent<Health>().OnHurt -= HurtVisualEffect;
        }
    }
}