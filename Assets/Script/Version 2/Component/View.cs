using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Version2
{
    public class View : MonoBehaviour
    {
        [Header("Sprite Renderer")]
        [SerializeField] private SpriteRenderer[] m_spriteRenderer;
        [SerializeField] private float m_hurtFlashDuration = 0.35f;
        [SerializeField] private IEnumerator m_coroutine;
        [SerializeField] private bool m_coroutineStatus;
        [SerializeField] private bool m_defaultFlipX;
        [SerializeField] private bool m_currentFlipX = false;

        [Header("Animation")]
        [SerializeField] private PlayableGraph m_playableGraph;
        [SerializeField] private AnimationMixerPlayable m_mixerPlayable;
        [SerializeField] private AnimationMixerPlayable m_flipMixerPlayable;
        [SerializeField] private AnimationClip m_attackClip;
        [SerializeField] private AnimationClip m_idleClip;
        [SerializeField] private AnimationClip m_moveClip;
        [SerializeField] private AnimationClip m_flipXTrue;
        [SerializeField] private AnimationClip m_flipXFalse;
        [SerializeField] private float m_duration = 0.5f;
        [SerializeField] private int m_currentState;
        [SerializeField] private int m_nextState;
        [SerializeField] private bool m_blendStatus;
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
            m_flipMixerPlayable.SetInputWeight(t_input ^ 1, 0f);
            m_flipMixerPlayable.SetInputWeight(t_input, 1f);
            m_flipMixerPlayable.GetInput(t_input).SetTime(0d);
            m_flipMixerPlayable.GetInput(t_input).SetDone(false);

            m_currentFlipX = t_face;
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

            m_coroutineStatus = false;
        }

        private IEnumerator BlendAnimation()
        {
            m_blendStatus = true;

            float t_time = 0f;

            while(t_time < m_duration)
            {
                t_time += Time.deltaTime;
                float t_weight = t_time / m_duration;

                m_mixerPlayable.SetInputWeight(m_currentState, 1f - t_weight);
                m_mixerPlayable.SetInputWeight(m_nextState, t_weight);
                yield return null;
            }

            m_mixerPlayable.SetInputWeight(m_currentState, 0f);
            m_mixerPlayable.SetInputWeight(m_nextState, 1f);

            m_blendStatus = false;
        }

        public bool AnimationIsDone(int unitState)
        {
            return m_mixerPlayable.GetInput(unitState).IsDone();
        }

        public void SwitchAnimation(int newState, int originState)
        {
            if (m_nextState == newState)
            {
                return;
            }

            if (m_blendStatus)
            {
                StopCoroutine(m_blendAnimation);
                m_mixerPlayable.SetInputWeight(m_currentState, 0f);
                m_mixerPlayable.SetInputWeight(m_nextState, 1f);
            }

            m_currentState = originState;
            m_nextState = newState;
            m_blendAnimation = BlendAnimation();
            StartCoroutine(m_blendAnimation);
        }

        public void ResetAnimation(int currentState)
        {
            m_mixerPlayable.GetInput(currentState).SetTime(0d);
            m_mixerPlayable.GetInput(currentState).SetDone(false);
        }

        public void Initialize(int group)
        {
            #region Animation
            Animator t_animator = GetComponent<Animator>();
            t_animator.cullingMode = AnimatorCullingMode.CullCompletely;

            m_playableGraph = PlayableGraph.Create("Infantry Graph");
            m_playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            AnimationPlayableOutput t_playableOutput = AnimationPlayableOutput.Create(m_playableGraph, "Infantry Output", t_animator);

            m_mixerPlayable = AnimationMixerPlayable.Create(m_playableGraph, 3);
            t_playableOutput.SetSourcePlayable(m_mixerPlayable);

            AnimationClipPlayable t_idlePlayable = AnimationClipPlayable.Create(m_playableGraph, m_idleClip);
            AnimationClipPlayable t_movePlayable = AnimationClipPlayable.Create(m_playableGraph, m_moveClip);
            AnimationClipPlayable t_attackPlayable = AnimationClipPlayable.Create(m_playableGraph, m_attackClip);
            t_idlePlayable.SetSpeed(0.5d);
            //t_movePlayable.SetSpeed(0.5d);
            t_attackPlayable.SetDuration((double)m_attackClip.length);
            //t_movePlayable.SetSpeed(6d);
            //t_attackPlayable.SetDuration((double)m_attackClip.length);
            //t_attackPlayable.SetSpeed(1.5d);
            m_playableGraph.Connect(t_idlePlayable, 0, m_mixerPlayable, (int)SwordMan.UnitState.Idle);
            m_playableGraph.Connect(t_movePlayable, 0, m_mixerPlayable, (int)SwordMan.UnitState.Move);
            m_playableGraph.Connect(t_attackPlayable, 0, m_mixerPlayable, (int)SwordMan.UnitState.Attack);


            AnimationClipPlayable t_flipXTrue = AnimationClipPlayable.Create(m_playableGraph, m_flipXTrue);
            AnimationClipPlayable t_flipXFalse = AnimationClipPlayable.Create(m_playableGraph, m_flipXFalse);
            t_flipXTrue.SetSpeed(2f);
            t_flipXFalse.SetSpeed(2f);

            m_flipMixerPlayable = AnimationMixerPlayable.Create(m_playableGraph, 2);
            m_playableGraph.Connect(t_flipXFalse, 0, m_flipMixerPlayable, 0);
            m_playableGraph.Connect(t_flipXTrue, 0, m_flipMixerPlayable, 1);

            AnimationPlayableOutput t_flipOutput = AnimationPlayableOutput.Create(m_playableGraph, "Infantry Output", t_animator);
            t_flipOutput.SetSourcePlayable(m_flipMixerPlayable);

            SwitchAnimation(0, 1);

            m_playableGraph.Play();
            #endregion

            GetComponent<Health>().OnHurt += HurtVisualEffect;
            m_coroutineStatus = false;
            m_defaultFlipX = (group & GameManager.Instance.SYWS) != 0;
        }

        public void Uninitialize()
        {
            GetComponent<Health>().OnHurt -= HurtVisualEffect;
            m_playableGraph.Destroy();
        }
    }
}