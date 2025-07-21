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

        public void ApplyModifier(StatusModifier modifier)
        {
            StatusModifier t_existModifier = QueryModifier(modifier.GetSourceDataID());
            if (t_existModifier != null)
            {
                switch (modifier.CurrentVariant.StackType)
                {
                    case StatusEffectStack.UniqueRefresh:
                        t_existModifier.RefreshDuration();
                        break;
                    case StatusEffectStack.Stack:
                        t_existModifier.StackUp();
                        break;
                    case StatusEffectStack.StackRefresh:
                        t_existModifier.StackUp();
                        t_existModifier.RefreshDuration();
                        break;
                }
            }
            else
            {
                m_modifiers.Add(modifier);
                modifier.Apply();
            }
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