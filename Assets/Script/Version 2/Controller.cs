using UnityEngine;

namespace Assets.Version2
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Command m_currentCommand = Command.Defend;

        public void SwitchCommand(int newCommandFlag)
        {
            Command t_newCommand = (Command)newCommandFlag;
            if (m_currentCommand == t_newCommand)
            {
                return;
            }

            m_currentCommand = t_newCommand;
        }

        private void Start()
        {
            m_currentCommand = Command.Defend;
        }

        public enum Command
        {
            Defend = 0,
            Attack = 1,
            Retreat = 2
        }
    }
}