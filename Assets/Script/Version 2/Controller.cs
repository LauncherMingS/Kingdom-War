using System.Collections.Generic;
using UnityEngine;

namespace Assets.Version2
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Command m_currentCommand = Command.None;

        [SerializeField] private Vector3 m_currentIndicationPosition;
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_defendPosition;
        [SerializeField] private Vector3 m_retreatPosition;

        //Test
        [SerializeField] private List<Unit> m_units;

        public Command CurrentCommand => m_currentCommand;

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
                    m_currentIndicationPosition = m_defendPosition;
                    break;
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

        public void AddUnit(Unit unit)
        {
            m_units.Add(unit);
            unit.SetDefaultPosition = m_currentIndicationPosition;
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