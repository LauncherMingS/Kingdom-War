using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [Serializable]
    public class StatusEffectManager
    {
        [SerializeField] private List<StatusModifier> m_modifiers;


        public StatusModifier QueryModifier(int id)
        {
            int modifierCount = m_modifiers.Count;
            for (int i = 0; i < modifierCount; i++)
            {
                if (m_modifiers[i].CompareSourceData(id))
                {
                    return m_modifiers[i];
                }
            }

            return null;
        }

        //Return ture means the current target has the status effect, false means do not have.
        public bool ApplyModifier(StatusEffectDataSO data, int level, Unit target, Unit source)
        {
            //Check whether the target has had the status effect.
            StatusModifier t_modifier = QueryModifier(data.GetInstanceID());
            //If don't have, add new status effect.
            if (t_modifier == null)
            {
                switch(data.EffectType)
                {
                    case StatusEffectType.Heal:
                        t_modifier = new HealStatusModifier(data, level, target, source);
                        break;
                    case StatusEffectType.Weakened:
                        t_modifier = new WeakenedStatusModifier(data, level, target);
                        break;
                    case StatusEffectType.None:
                        t_modifier = null;
                        GameManager.LogWarningEditor($"StatusEffectManager: The type of status effect is none.");
                        break;
                    default:
                        t_modifier = null;
                        GameManager.LogWarningEditor($"StatusEffectManager: No such type of status effect.");
                        break;
                }

                if (t_modifier == null)
                {
                    GameManager.LogWarningEditor($"StatusEffectManager: Cannot add status effect.");
                    return false;
                }

                m_modifiers.Add(t_modifier);
                t_modifier.Apply();
                return false;
            }

            //If have, modify the existing status effect.
            if (t_modifier.CurrentVariant.CanStack)
            {
                t_modifier.StackUp();
            }
            if (t_modifier.CurrentVariant.CanRefresh)
            {
                t_modifier.RefreshDuration();
            }

            return true;
        }

        public void TickModifier(float deltaTime)
        {
            int t_modifierCount = m_modifiers.Count;
            for (int i = 0; i < t_modifierCount; i++)
            {
                if (m_modifiers[i].Tick(deltaTime))
                {
                    RemoveModifier(m_modifiers[i], i);
                }
            }
        }

        public void RemoveModifier(StatusModifier modifier, int index)
        {
            modifier.Remove();
            m_modifiers.RemoveAt(index);
        }

        //When the carrier dying, remove all modifier and their visual effect(particle...)
        public void ForceRemoveAllModifier()
        {
            int t_modifierCount = m_modifiers.Count;
            for (int i = 0; i < t_modifierCount; i++)
            {
                RemoveModifier(m_modifiers[i], i);
            }
        }

        public void Initialize(Unit holder)
        {
            m_modifiers = new List<StatusModifier>();
            holder.OnUpdateCD += TickModifier;
        }

        public void UnInitialize(Unit holder)
        {
            ForceRemoveAllModifier();
            m_modifiers.Clear();
            holder.OnUpdateCD -= TickModifier;
        }
    }
}