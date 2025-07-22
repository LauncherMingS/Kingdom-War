using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Version2
{
    public class View : MonoBehaviour
    {
        [Header("Parameter")]
        [Header("Hurt Flash")]
        [SerializeField] private float m_hurtFlashDuration = 0.35f;
        [SerializeField] private bool m_hurtFlashStatus;
        [Header("Flip")]
        [SerializeField] private bool m_defaultFlipX;
        [SerializeField] private bool m_currentFlipX = false;
        [Header("Animation Blend")]
        [SerializeField] private float m_blendDuration = 0.5f;
        [SerializeField] private int m_currentState;
        [SerializeField] private int m_nextState;
        [SerializeField] private bool m_blendAnimationStatus;

        [Header("Component Reference")]
        [SerializeField] private SpriteRenderer[] m_spriteRenderer;

        public PlayableGraph Graph;
        public AnimationMixerPlayable MainMixer;
        public AnimationMixerPlayable FlipMixer;
        private IEnumerator m_hurtFlash;
        private IEnumerator m_blendAnimation;


        public void Face(float positionX)
        {
            bool t_face = m_defaultFlipX;
            if (transform.position.x < positionX)
            {
                t_face = true;
            }
            else if (transform.position.x > positionX)
            {
                t_face = false;
            }

            if (m_currentFlipX == t_face)
            {
                return;
            }

            int t_input = (t_face) ? 1 : 0;
            FlipMixer.SetInputWeight(t_input ^ 1, 0f);
            FlipMixer.SetInputWeight(t_input, 1f);
            FlipMixer.GetInput(t_input).SetTime(0d);

            m_currentFlipX = t_face;
        }

        public void Coloring(Color color)
        {
            int t_length = m_spriteRenderer.Length;
            for (int i = 0; i < t_length; i++)
            {
                m_spriteRenderer[i].color = color;
            }
        }

        public void HurtVisualEffect(float point)
        {
            if (m_hurtFlashStatus)
            {
                StopCoroutine(m_hurtFlash);
            }

            m_hurtFlash = HurtFlash();
            StartCoroutine(m_hurtFlash);
        }

        private IEnumerator HurtFlash()
        {
            m_hurtFlashStatus = true;

            Coloring(Color.red);
            yield return new WaitForSeconds(m_hurtFlashDuration);
            Coloring(Color.white);

            m_hurtFlashStatus = false;
        }

        private IEnumerator BlendAnimation()
        {
            m_blendAnimationStatus = true;

            float t_time = 0f;

            while(t_time < m_blendDuration)
            {
                t_time += Time.deltaTime;
                float t_weight = t_time / m_blendDuration;

                MainMixer.SetInputWeight(m_currentState, 1f - t_weight);
                MainMixer.SetInputWeight(m_nextState, t_weight);
                yield return null;
            }

            MainMixer.SetInputWeight(m_currentState, 0f);
            MainMixer.SetInputWeight(m_nextState, 1f);

            m_blendAnimationStatus = false;
        }

        public bool AnimationIsDone(int unitState)
        {
            return MainMixer.GetInput(unitState).IsDone();
        }

        public void SwitchAnimation(int newState, int originState)
        {
            if (m_nextState == newState)
            {
                return;
            }

            if (m_blendAnimationStatus)
            {
                StopCoroutine(m_blendAnimation);
                MainMixer.SetInputWeight(m_currentState, 0f);
                MainMixer.SetInputWeight(m_nextState, 1f);
            }

            m_currentState = originState;
            m_nextState = newState;
            m_blendAnimation = BlendAnimation();
            StartCoroutine(m_blendAnimation);
        }

        public void ResetAnimation(int currentState)
        {
            MainMixer.GetInput(currentState).SetTime(0d);
            MainMixer.GetInput(currentState).SetDone(false);
        }

        public void Initialize()
        {
            m_hurtFlashStatus = false;
            m_defaultFlipX = GameManager.Instance.IsSYWS(gameObject.layer);
            GetComponent<Health>().OnHurt += HurtVisualEffect;
            Coloring(Color.white);
        }

        public void Uninitialize()
        {
            GetComponent<Health>().OnHurt -= HurtVisualEffect;
        }

        private void OnDestroy()
        {
            if (Graph.IsValid())
            {
                Graph.Destroy();
            }
        }
    }
}