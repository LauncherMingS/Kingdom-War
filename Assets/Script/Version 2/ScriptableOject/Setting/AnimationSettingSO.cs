using UnityEngine;

namespace Assets.Version2
{
    [CreateAssetMenu(menuName = "Scriptable Object/AnimationSetting", fileName = "AnimationSetting")]
    public class AnimationSettingSO : ScriptableObject
    {
        [SerializeField] private AnimationClip[] m_animationClips;
        [SerializeField] private float[] m_animationSpeed;
        [SerializeField] private bool[] m_isLoop;

        public AnimationClip[] AnimationClips => m_animationClips;

        public float[] AnimationSpeed => m_animationSpeed;

        public bool[] IsLoop => m_isLoop;
    }
}