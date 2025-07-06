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
        [Header("Asset Reference")]
        [SerializeField] private AnimationClip m_attackClip;
        [SerializeField] private AnimationClip m_idleClip;
        [SerializeField] private AnimationClip m_moveClip;
        [SerializeField] private AnimationClip m_flipXTrueClip;
        [SerializeField] private AnimationClip m_flipXFalseClip;

        [SerializeField] private PlayableGraph m_graph;
        [SerializeField] private AnimationMixerPlayable m_mainMixer;
        [SerializeField] private AnimationMixerPlayable m_flipMixer;
        [SerializeField] private IEnumerator m_hurtFlash;
        [SerializeField] private IEnumerator m_blendAnimation;

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
            m_flipMixer.SetInputWeight(t_input ^ 1, 0f);
            m_flipMixer.SetInputWeight(t_input, 1f);
            m_flipMixer.GetInput(t_input).SetTime(0d);
            m_flipMixer.GetInput(t_input).SetDone(false);

            m_currentFlipX = t_face;
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

            int t_length = m_spriteRenderer.Length;
            for (int i = 0; i < t_length; i++)
            {
                m_spriteRenderer[i].color = Color.red;
            }
            yield return new WaitForSeconds(m_hurtFlashDuration);
            for (int i = 0; i < t_length; i++)
            {
                m_spriteRenderer[i].color = Color.white;
            }

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

                m_mainMixer.SetInputWeight(m_currentState, 1f - t_weight);
                m_mainMixer.SetInputWeight(m_nextState, t_weight);
                yield return null;
            }

            m_mainMixer.SetInputWeight(m_currentState, 0f);
            m_mainMixer.SetInputWeight(m_nextState, 1f);

            m_blendAnimationStatus = false;
        }

        public bool AnimationIsDone(int unitState)
        {
            return m_mainMixer.GetInput(unitState).IsDone();
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
                m_mainMixer.SetInputWeight(m_currentState, 0f);
                m_mainMixer.SetInputWeight(m_nextState, 1f);
            }

            m_currentState = originState;
            m_nextState = newState;
            m_blendAnimation = BlendAnimation();
            StartCoroutine(m_blendAnimation);
        }

        public void ResetAnimation(int currentState)
        {
            m_mainMixer.GetInput(currentState).SetTime(0d);
            m_mainMixer.GetInput(currentState).SetDone(false);
        }

        private void ConstructPlayable()
        {
            Animator t_animator = GetComponent<Animator>();
            t_animator.cullingMode = AnimatorCullingMode.CullCompletely;

            m_graph = PlayableGraph.Create("Infantry Graph");
            m_graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            AnimationClipPlayable t_idle = AnimationClipPlayable.Create(m_graph, m_idleClip);
            AnimationClipPlayable t_move = AnimationClipPlayable.Create(m_graph, m_moveClip);
            AnimationClipPlayable t_attack = AnimationClipPlayable.Create(m_graph, m_attackClip);
            t_idle.SetSpeed(0.5d);
            t_attack.SetDuration((double)m_attackClip.length);

            m_mainMixer = AnimationMixerPlayable.Create(m_graph, 3);
            m_graph.Connect(t_idle, 0, m_mainMixer, (int)SwordMan.UnitState.Idle);
            m_graph.Connect(t_move, 0, m_mainMixer, (int)SwordMan.UnitState.Move);
            m_graph.Connect(t_attack, 0, m_mainMixer, (int)SwordMan.UnitState.Attack);

            AnimationPlayableOutput t_mainOutput = AnimationPlayableOutput.Create(m_graph, "Infantry Output", t_animator);
            t_mainOutput.SetSourcePlayable(m_mainMixer);


            AnimationClipPlayable t_flipXTrue = AnimationClipPlayable.Create(m_graph, m_flipXTrueClip);
            AnimationClipPlayable t_flipXFalse = AnimationClipPlayable.Create(m_graph, m_flipXFalseClip);
            t_flipXTrue.SetSpeed(2f);
            t_flipXFalse.SetSpeed(2f);

            m_flipMixer = AnimationMixerPlayable.Create(m_graph, 2);
            m_graph.Connect(t_flipXFalse, 0, m_flipMixer, 0);
            m_graph.Connect(t_flipXTrue, 0, m_flipMixer, 1);

            AnimationPlayableOutput t_flipOutput = AnimationPlayableOutput.Create(m_graph, "Infantry Output", t_animator);
            t_flipOutput.SetSourcePlayable(m_flipMixer);

            m_graph.Play();
        }

        public void Initialize()
        {
            m_hurtFlashStatus = false;
            m_defaultFlipX = GameManager.Instance.IsSYWS(gameObject.layer);
            GetComponent<Health>().OnHurt += HurtVisualEffect;
            ConstructPlayable();
        }

        public void Uninitialize()
        {
            m_graph.Destroy();
            GetComponent<Health>().OnHurt -= HurtVisualEffect;
        }
    }
}