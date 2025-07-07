using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2
{
    public class Controller : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] private Command m_currentCommand = Command.None;
        [SerializeField] private Group m_group = Group.None;
        [Header("Line Up")]
        [SerializeField] private float m_beginX = -10f;
        [SerializeField] private float m_commonDifferenceX = -3f;
        [SerializeField] private float m_totalLengthZ = 12f;
        [SerializeField] private int m_maxUnitPerRow = 4;
        [Header("Position")]
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_retreatPosition;
        [Header("Unit List")]
        [SerializeField] private List<SwordMan> m_units;

        [Header("Asset Reference")]
        [SerializeField] private UnitFactory m_factory;

        public Command CurrentCommand => m_currentCommand;

        //Button Listener(Command)
        public void SwitchCommand(int newCommandFlag)
        {
            if (!Enum.IsDefined(typeof(Command), newCommandFlag))
            {
#if UNITY_EDITOR
                Debug.LogWarning(gameObject.name + ": newCommandFlag " + newCommandFlag + " is not defind.");
#endif
                return;
            }

            SwitchCommand((Command)newCommandFlag);
        }

        private void SwitchCommand(Command newCommand)
        {
            if (newCommand == m_currentCommand)
            {
                return;
            }

            m_currentCommand = newCommand;

            if (m_currentCommand == Command.Defend)
            {
                LineUp();
            }
        }

        private void LineUp(int startIndex = 0)
        {
            if (m_units.Count == 0 || startIndex >= m_units.Count)
            {
#if UNITY_EDITOR
                Debug.LogWarning(gameObject.name + ": units list is empty or startIndex is greater than list count.");
#endif
                return;
            }

            int t_currentRow = startIndex / m_maxUnitPerRow;
            int t_remainUnitCount = m_units.Count - (t_currentRow * m_maxUnitPerRow);
            Vector3 t_unitPosition = Vector3.zero;
            while (t_remainUnitCount > 0)
            {
                int t_unitCountThisRow = (t_remainUnitCount > m_maxUnitPerRow)
                    ? m_maxUnitPerRow : t_remainUnitCount;

                int t_thisRowStartIndex = t_currentRow * m_maxUnitPerRow;
                t_unitPosition.x = t_currentRow * m_commonDifferenceX + m_beginX;
                for (int i = 0; i < t_unitCountThisRow; i++)
                {
                    t_unitPosition.z = m_totalLengthZ * (i + 1) / (t_unitCountThisRow + 1);

                    int t_unitIndex = t_thisRowStartIndex + i;
                    if (t_unitIndex < m_units.Count)
                    {
                        m_units[t_unitIndex].DefensePosition = t_unitPosition;
                    }
                }

                t_remainUnitCount -= t_unitCountThisRow;
                t_currentRow++;
            }
        }

        public void CreateSwordMan()
        {
            SwordMan t_swordMan = m_factory.CreateSwordMan(m_group);
            t_swordMan.SetController = this;
            t_swordMan.EnemyBasePosition = m_enemyBasePosition;
            t_swordMan.RetreatPosition = m_retreatPosition;

            m_units.Add(t_swordMan);
            LineUp(m_units.Count - 1);
        }

        private void Start()
        {
            m_group = (GameManager.Instance.IsSYWS(gameObject.layer)) ? Group.SYWS : Group.NLI;

            SwitchCommand(Command.Defend);
        }

        public enum Command
        {
            None = 0,
            Attack = 1,
            Defend = 2,
            Retreat = 3
        }
    }
}