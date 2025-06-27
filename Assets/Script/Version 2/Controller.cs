using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Command m_currentCommand = Command.None;

        [Header("Position Data")]
        [SerializeField] private Vector3 m_currentIndicationPosition;
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_retreatPosition;

        [Header("Line Up Data")]
        [SerializeField] private float m_beginX = -10f;
        [SerializeField] private float m_commonDifferenceX = -3f;
        [SerializeField] private float m_totalLengthZ = 12f;
        [SerializeField] private int m_maxUnitPerRow = 4;

        [Header("Unit List")]
        [SerializeField] private List<Unit> m_units;

        public Command CurrentCommand => m_currentCommand;

        //Button Listener
        public void SwitchCommand(int newCommandFlag)
        {
            if (newCommandFlag > 3)
            {
                throw new System.Exception("Command flag can not be greater than 3.");
            }

            SwitchCommand((Command)newCommandFlag);
        }

        public void SwitchCommand(Command newCommand)
        {
            if (newCommand == m_currentCommand)
            {
                return;
            }

            m_currentCommand = newCommand;
            switch (m_currentCommand)
            {
                case Command.Attack:
                    m_currentIndicationPosition = m_enemyBasePosition;
                    break;
                case Command.Defend:
                    LineUp();
                    return;
                case Command.Retreat:
                    m_currentIndicationPosition = m_retreatPosition;
                    break;
                default:
                    m_currentIndicationPosition = Vector3.zero;
                    break;
            }

            for (int i = 0;i <  m_units.Count;i++)
            {
                m_units[i].SetDefaultPosition = m_currentIndicationPosition;
            }
        }

        public void LineUp()
        {
            if (m_units.Count == 0)
            {
                return;
            }

            int t_currentRow = 0;
            int t_remainUnitCount = m_units.Count;
            Vector3 t_unitPosition = Vector3.zero;
            while (t_remainUnitCount > 0)
            {
                int t_unitCountThisRow = (t_remainUnitCount > m_maxUnitPerRow) 
                    ? m_maxUnitPerRow : t_remainUnitCount;

                int t_rowStartIndex = t_currentRow * m_maxUnitPerRow;
                t_unitPosition.x = t_currentRow * m_commonDifferenceX + m_beginX;
                for (int i = 0; i < t_unitCountThisRow; i++)
                {
                    t_unitPosition.z = m_totalLengthZ * (i + 1) / (t_unitCountThisRow + 1);

                    int t_unitIndex = t_rowStartIndex + i;
                    if (t_unitIndex < m_units.Count)
                    {
                        m_units[t_unitIndex].SetDefaultPosition = t_unitPosition;
                    }
                }

                t_remainUnitCount -= t_unitCountThisRow;
                t_currentRow++;
            }
        }

        public void LineUpLastRow()
        {
            if (m_units.Count == 0)
            {
                return;
            }

            int t_lastRowIndex = m_units.Count / m_maxUnitPerRow;
            int t_unitCountLastRow = m_units.Count % m_maxUnitPerRow;
            if (t_unitCountLastRow == 0)
            {
                t_lastRowIndex--;
                t_unitCountLastRow = m_maxUnitPerRow;
            }

            int t_rowStartIndex = t_lastRowIndex * m_maxUnitPerRow;
            Vector3 t_unitPosition = Vector3.zero;
            t_unitPosition.x = t_lastRowIndex * m_commonDifferenceX + m_beginX;
            for (int i = 0;i < t_unitCountLastRow;i++)
            {
                t_unitPosition.z = m_totalLengthZ * (i + 1) / (t_unitCountLastRow + 1);

                int t_unitIndex = t_rowStartIndex + i;
                if (t_unitIndex < m_units.Count)
                {
                    m_units[t_unitIndex].SetDefaultPosition = t_unitPosition;
                }
            }
        }

        public void AddUnit(Unit unit)
        {
            m_units.Add(unit);
            LineUpLastRow();
        }

        private void Start()
        {
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