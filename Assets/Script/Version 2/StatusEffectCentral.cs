using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    public class StatusEffectCentral : ScriptableObject
    {
        private static StatusEffectCentral m_instance;

        //Stack Behavior Strategy field
        [SerializeField] private IncreaseStack m_increaseStack;
        [SerializeField] private ExtendDuration m_extendDuration;
        [SerializeField] private RefreshDuration m_refreshDuration;
        [SerializeField] private RefreshAndIncreaseStack m_refreshAndIncreaseStack;

        public static StatusEffectCentral Instance => m_instance;

        //Stack Behavior Strategy getter
        public IncreaseStack IncreaseStack
        {
            get
            {
                m_increaseStack ??= new IncreaseStack();
                return m_increaseStack;
            }
        }
        public ExtendDuration ExtendDuration
        {
            get
            {
                m_extendDuration ??= new ExtendDuration();
                return m_extendDuration;
            }
        }
        public RefreshDuration RefreshDuration
        {
            get
            {
                m_refreshDuration ??= new RefreshDuration();
                return m_refreshDuration;
            }
        }
        public RefreshAndIncreaseStack RefreshAndIncreaseStack
        {
            get
            {
                m_refreshAndIncreaseStack ??= new RefreshAndIncreaseStack();
                return m_refreshAndIncreaseStack;
            }
        }
        

        public StatusModifierBase CreateStatusModifier(StatusEffectDataSO data, Unit target, Unit source)
        {
            StatusModifierBase t_modifier;
            switch (data.EffectType)
            {
                case StatusEffectType.Heal:
                    t_modifier = new HealStatusModifier();
                    (t_modifier as HealStatusModifier).Initialize(data, target, source);
                    break;
                case StatusEffectType.Weakened:
                    t_modifier = new WeakenedStatusModifier();
                    (t_modifier as WeakenedStatusModifier).Initialize(data, target);
                    break;
                case StatusEffectType.None:
                    t_modifier = null;
                    GameManager.LogWarningEditor($"StatusEffectCentral: The type of status effect is none.");
                    break;
                default:
                    t_modifier = null;
                    GameManager.LogWarningEditor($"StatusEffectCentral: No such type of status effect. {data.EffectType}");
                    break;
            }

            return t_modifier;
        }

        public void InitializeData(StatusEffectDataSO[] statusEffects)
        {
            for (int i = 0;i < statusEffects.Length; i++)
            {
                statusEffects[i].Initialize();
            }
        }

        private void OnEnable()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else if (m_instance != this)
            {
                Destroy(this);
            }
        }
    }
}