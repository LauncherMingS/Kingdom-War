using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2.StatusEffectSystem
{
    [Serializable]
    public class StatusEffectManager
    {
        [SerializeField] private List<StatusModifierBase> m_modifiers;
        [SerializeField] private List<float> m_damageModifiers;


        public StatusModifierBase QueryModifier(int id)
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

        public void ApplyModifier(StatusEffectDataSO data, Unit target, Unit source)
        {
            //Check whether the target has had the status effect.
            StatusModifierBase t_modifier = QueryModifier(data.GetInstanceID());

            //If don't have, add new status effect.
            if (t_modifier == null)
            {
                t_modifier = StatusEffectCentral.Instance.CreateStatusModifier(data, target, source);

                if (t_modifier == null)
                {
                    GameManager.LogWarningEditor($"StatusEffectManager: Cannot add status effect.");
                    return;
                }

                m_modifiers.Add(t_modifier);
                t_modifier.OnApply();
                return;
            }

            //If have, modify the existing status effect.
            t_modifier.OnStack();
        }

        public void TickModifier(float deltaTime)
        {
            for (int i = m_modifiers.Count - 1; i >= 0; i--)
            {
                if (m_modifiers[i].OnTick(deltaTime))
                {
                    RemoveModifier(i);
                }
            }
        }

        public void RemoveModifier(int removeIndex)
        {
            m_modifiers[removeIndex].OnRemove();
            m_modifiers.RemoveAt(removeIndex);
        }

        //When the carrier dying, remove all modifier and their visual effect(particle...)
        public void ForceRemoveAllModifier()
        {
            for (int i = m_modifiers.Count - 1; i >= 0; i--)
            {
                RemoveModifier(i);
            }
            for (int i = m_damageModifiers.Count - 1; i >= 0; i--)
            {
                m_damageModifiers.RemoveAt(i);
            }
        }

        public void AddDamageModifier(float multiplier)
        {
            m_damageModifiers.Add(multiplier);
        }

        public void RemoveDamageModifier(float multiplier)
        {
            m_damageModifiers.Remove(multiplier);
        }

        public float GetTotalDamageMultiplier()
        {
            float t_totalMultiplier = 1f;
            for (int i = 0; i < m_damageModifiers.Count; i++)
            {
                t_totalMultiplier *= m_damageModifiers[i];
            }

            return t_totalMultiplier;
        }

        public void Initialize(Unit holder)
        {
            m_modifiers ??= new();
            m_damageModifiers ??= new();
            holder.OnUpdateCD += TickModifier;
        }

        public void UnInitialize()
        {
            ForceRemoveAllModifier();
        }
    }
}