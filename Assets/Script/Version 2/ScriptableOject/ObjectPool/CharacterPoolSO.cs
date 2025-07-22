using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Version2.Pool
{
    public class CharacterPoolSO<T> : ObjectPoolSO<T> where T : Component
    {
        [SerializeField] protected AnimationSettingSO m_mainSettings;
        [SerializeField] protected AnimationSettingSO m_flipSettings;


        protected override T Create()
        {
            T t_instance = base.Create();

            //Construct Playable(Currently only Animation)
            Animator t_animator = t_instance.GetComponent<Animator>();
            View t_view = t_instance.GetComponent<View>();
            CreatePlayableGraph(out t_view.Graph);
            CreateAnimationMixerPlayable(out t_view.MainMixer, ref t_view.Graph, m_mainSettings, t_animator);
            CreateAnimationMixerPlayable(out t_view.FlipMixer, ref t_view.Graph, m_flipSettings, t_animator);
            t_view.Graph.Play();

            return t_instance;
        }

        protected void CreatePlayableGraph(out PlayableGraph graph)
        {
            graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        }

        protected void CreateAnimationMixerPlayable(out AnimationMixerPlayable mixer, ref PlayableGraph graph
            , AnimationSettingSO settings, Animator animator)
        {
            //Get settings
            AnimationClip[] t_clips = settings.AnimationClips;
            float[] t_speed = settings.AnimationSpeed;
            bool[] t_isLoop = settings.IsLoop;

            //According settings handle animations
            int t_clipLength = t_clips.Length;
            mixer = AnimationMixerPlayable.Create(graph, t_clipLength);
            for (int i = 0; i < t_clipLength; i++)
            {
                AnimationClipPlayable t_clipPlayable = AnimationClipPlayable.Create(graph, t_clips[i]);
                t_clipPlayable.SetSpeed(t_speed[i]);
                if (!t_isLoop[i])
                {
                    t_clipPlayable.SetDuration(t_clips[i].length);
                }

                graph.Connect(t_clipPlayable, 0, mixer, i);
            }

            //Handle output
            AnimationPlayableOutput t_outputPlayable = AnimationPlayableOutput.Create(graph, string.Empty, animator);
            t_outputPlayable.SetSourcePlayable(mixer);
        }
    }
}