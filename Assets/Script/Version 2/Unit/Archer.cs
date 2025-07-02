using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Assets.Version2
{
    public class Archer : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Controller m_controller;

        [Header("Component")]
        [SerializeField] private Health m_health;
        [SerializeField] private Movement m_movement;
        [SerializeField] private DetectionHandler m_detectionHandler;
        [SerializeField] private View m_view;

        [Header("Data")]
        [SerializeField] private int m_group;
        [SerializeField] private float m_attackDetectionRadius;
        [SerializeField] private float m_defenseDetectionRadius;

        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_defensePosition;
        [SerializeField] private Vector3 m_retreatPosition;
                    

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchState(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchState(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchState(2);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FlipAnimation();
            }
        }

        [SerializeField] private int m_currentState = 0;
        [SerializeField] private bool m_currentFlip = false;
        [SerializeField] private Animator m_animator;
        [SerializeField] private PlayableGraph m_playableGraph;

        [SerializeField] private AnimationMixerPlayable m_mixerPlayable;
        [SerializeField] private AnimationMixerPlayable m_flipMixerPlayable;

        [SerializeField] private AnimationClip m_idleClip;
        [SerializeField] private AnimationClip m_moveClip;
        [SerializeField] private AnimationClip m_attackClip;

        [SerializeField] private AnimationClip m_flipXTrueClip;
        [SerializeField] private AnimationClip m_flipXFalseClip;


        private void Start()
        {
            m_animator.cullingMode = AnimatorCullingMode.CullCompletely;

            m_playableGraph = PlayableGraph.Create("Archer Graph");
            m_playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            AnimationPlayableOutput t_playableOutput = AnimationPlayableOutput.Create(m_playableGraph, "Archer Output", m_animator);
            AnimationPlayableOutput t_flipPlayableOutput = AnimationPlayableOutput.Create(m_playableGraph, "Flip Output", m_animator);

            m_mixerPlayable = AnimationMixerPlayable.Create(m_playableGraph, 3);

            AnimationClipPlayable t_idlePlayable = AnimationClipPlayable.Create(m_playableGraph, m_idleClip);
            AnimationClipPlayable t_movePlayable = AnimationClipPlayable.Create(m_playableGraph, m_moveClip);
            AnimationClipPlayable t_attackPlayable = AnimationClipPlayable.Create(m_playableGraph, m_attackClip);
            t_idlePlayable.SetSpeed(0.5d);
            t_attackPlayable.SetSpeed(0.5d);
            m_playableGraph.Connect(t_idlePlayable, 0, m_mixerPlayable, 0);
            m_playableGraph.Connect(t_movePlayable, 0, m_mixerPlayable, 1);
            m_playableGraph.Connect(t_attackPlayable, 0, m_mixerPlayable, 2);
            t_playableOutput.SetSourcePlayable(m_mixerPlayable);

            m_flipMixerPlayable = AnimationMixerPlayable.Create(m_playableGraph, 2);

            AnimationClipPlayable t_flipXTruePlayable = AnimationClipPlayable.Create(m_playableGraph, m_flipXTrueClip);
            AnimationClipPlayable t_flipXFalsePlayable = AnimationClipPlayable.Create(m_playableGraph, m_flipXFalseClip);
            m_playableGraph.Connect(t_flipXTruePlayable, 0, m_flipMixerPlayable, 0);
            m_playableGraph.Connect(t_flipXFalsePlayable, 0, m_flipMixerPlayable, 1);
            t_flipPlayableOutput.SetSourcePlayable(m_flipMixerPlayable);

            SwitchAnimation(0, 1);

            m_playableGraph.Play();

            //m_health.Initialize();
            //m_movement.Initialize();
            //m_detectionHandler.Initialize(m_group);
            //m_view.Initialize(m_group);
        }

        public void SwitchState(int newState)
        {
            if (m_currentState == newState)
            {
                return;
            }

            SwitchAnimation(newState, m_currentState);
            m_currentState = newState;
        }

        public void FlipAnimation()
        {
            int t_index = (!m_currentFlip) ? 1 : 0;
            int t_indexReverse = (m_currentFlip) ? 1 : 0;
            m_flipMixerPlayable.SetInputWeight(t_indexReverse, 0f);
            m_flipMixerPlayable.SetInputWeight(t_index, 1f);
            m_flipMixerPlayable.GetInput(t_index).SetTime(0d);
            m_flipMixerPlayable.GetInput(t_index).SetDone(false);

            m_currentFlip = !m_currentFlip;
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

        private void OnDisable()
        {
            m_view.Uninitialize();
            m_playableGraph.Destroy();
        }
    }
}