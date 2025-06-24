using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Version2
{
    public class View : MonoBehaviour
    {
        [Header("Sprite Renderer")]
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private float m_hurtFlashDuration = 0.35f;
        [SerializeField] private IEnumerator m_coroutine;
        [SerializeField] private bool m_coroutineStatus;

        [Header("Animation")]
        [SerializeField] private PlayableGraph m_playableGraph;
        [SerializeField] private AnimationMixerPlayable m_mixerPlayable;
        [SerializeField] private AnimationClip m_attackClip;
        [SerializeField] private AnimationClip m_idleClip;
        [SerializeField] private AnimationClip m_moveClip;

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

        public bool AnimationIsDone(int unitState)
        {
            return m_mixerPlayable.GetInput(unitState).IsDone();
        }

        public void SwitchAnimation(int newState, int originState)
        {
            m_mixerPlayable.SetInputWeight(originState, 0f);
            m_mixerPlayable.SetInputWeight(newState, 1f);
        }

        public void ResetAnimation(int currentState)
        {
            m_mixerPlayable.GetInput(currentState).SetTime(0d);
            m_mixerPlayable.GetInput(currentState).SetDone(false);
        }

        public void Initialize()
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
            t_movePlayable.SetSpeed(6d);
            t_attackPlayable.SetDuration((double)m_attackClip.length);
            t_attackPlayable.SetSpeed(1.5d);
            m_playableGraph.Connect(t_idlePlayable, 0, m_mixerPlayable, (int)Unit.UnitState.Idle);
            m_playableGraph.Connect(t_movePlayable, 0, m_mixerPlayable, (int)Unit.UnitState.Move);
            m_playableGraph.Connect(t_attackPlayable, 0, m_mixerPlayable, (int)Unit.UnitState.Attack);

            SwitchAnimation(0, 1);

            m_playableGraph.Play();
            #endregion

            GetComponent<Health>().OnHurt += HurtVisualEffect;
            m_coroutineStatus = false;
        }

        public void Uninitialize()
        {
            GetComponent<Health>().OnHurt -= HurtVisualEffect;
        }
    }
}