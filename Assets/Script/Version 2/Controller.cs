using UnityEngine;

namespace Assets.Version2
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Command m_currentCommand = Command.None;

        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_defendPosition;
        [SerializeField] private Vector3 m_retreatPosition;

        //Test
        [SerializeField] private Unit m_unit;

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
                    m_unit.SetDefaultPosition = m_enemyBasePosition;
                    break;
                case Command.Defend:
                    m_unit.SetDefaultPosition = m_defendPosition;
                    break;
                case Command.Retreat:
                    m_unit.SetDefaultPosition = m_retreatPosition;
                    break;
            }
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