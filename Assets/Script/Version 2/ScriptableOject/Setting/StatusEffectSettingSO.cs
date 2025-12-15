using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [CreateAssetMenu(menuName = "Scriptable Object/Setting/Status Effect Setting", fileName = "Status Effect Setting")]
    public class StatusEffectSettingSO : ScriptableObject
    {
        [SerializeField] private StatusEffectDataSO[] m_statusEffects;

        public StatusEffectDataSO[] StatusEffects => m_statusEffects;
    }
}