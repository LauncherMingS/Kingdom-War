using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Version2.GameEnum;
using Assets.Version2.Pool;

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
        [SerializeField] private int m_swordManLastRow = 0;
        [SerializeField] private int m_archerLastRow = 0;
        [Header("Position")]
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_retreatPosition;
        [Header("Unit List")]
        [SerializeField] private List<SwordMan> m_swordManList;
        [SerializeField] private List<Archer> m_archerList;

        public Command CurrentCommand => m_currentCommand;


        //Button Listener(Command)
        public void SwitchCommand(int newCommandFlag)
        {
            if (!GameManager.EnumIsDefined<Command>(newCommandFlag, name))
            {
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
                LineUp(m_swordManList, UnitType.SwordMan);
                LineUp(m_archerList, UnitType.Archer);
            }
        }

        //Button Listener(Recruit)
        public void RecruitUnit(int typeValue)
        {
            if (!GameManager.EnumIsDefined<UnitType>(typeValue, name))
            {
                return;
            }

            switch ((UnitType)typeValue)
            {
                case UnitType.SwordMan:
                    RecruitUnit(m_swordManList, UnitType.SwordMan, ref m_swordManLastRow, LineUpAfterSwordManRowChange);
                    break;
                case UnitType.Archer:
                    RecruitUnit(m_archerList, UnitType.Archer, ref m_archerLastRow, null);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Recruit this unit type[{(UnitType)typeValue}]");
                    break;
            }
        }

        private void RecruitUnit<T>(List<T> unitList, UnitType unitType, ref int lastRow, Action lineUpOther) where T : Unit
        {
            T t_unit = ObjectPoolManagerSO.Instance.Get<T>(m_group, unitType);
            t_unit.EnemyBasePosition = m_enemyBasePosition;
            t_unit.RetreatPosition = m_retreatPosition;
            t_unit.Initialize();
            t_unit.transform.position = transform.position;

            unitList.Add(t_unit);

            int t_newLastRowIndex = CalculateLastRowIndex(unitList);
            if (t_newLastRowIndex > lastRow)
            {
                lastRow = t_newLastRowIndex;
                lineUpOther?.Invoke();
            }
            LineUp(unitList, unitType, unitList.Count - 1);
            lastRow = t_newLastRowIndex;
        }

        public void RemoveUnit<T>(UnitType unitType, T unit)
        {
            switch (unitType)
            {
                case UnitType.SwordMan:
                    RemoveUnit(m_swordManList, unitType, unit as SwordMan, ref m_swordManLastRow, LineUpAfterSwordManRowChange);
                    break;
                case UnitType.Archer:
                    RemoveUnit(m_archerList, unitType, unit as Archer, ref m_archerLastRow, null);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Remove this unit type[{unitType}]");
                    break;
            }
        }

        public void RemoveUnit<T>(List<T> unitList, UnitType unitType, T unit, ref int lastRow, Action lineUpOther) where T : Unit
        {
            unit.UnInitialize();

            unitList.Remove(unit);
            ObjectPoolManagerSO.Instance.Recycle(m_group, unitType, unit);

            int t_newLastRowIndex = CalculateLastRowIndex(unitList);
            if (t_newLastRowIndex < lastRow)
            {
                lastRow = t_newLastRowIndex;
                lineUpOther?.Invoke();
            }
            LineUp(unitList, unitType, unitList.Count - 1);
            lastRow = t_newLastRowIndex;
        }

        private int CalculateLastRowIndex<T>(List<T> unitList)
        {
            int t_lastRowIndex = unitList.Count / m_maxUnitPerRow;
            return (unitList.Count % m_maxUnitPerRow == 0) ? --t_lastRowIndex : t_lastRowIndex;
        }

        private void LineUp<T>(List<T> unitList, UnitType unitType, int startIndex = 0) where T : Unit
        {
            if (m_currentCommand != Command.Defend)
            {
                return;
            }

            if (unitList.Count == 0 || startIndex >= unitList.Count)
            {
                return;
            }

            int t_preOtherUnitRow = 0;
            switch (unitType)
            {
                case UnitType.SwordMan:
                    if (unitType != UnitType.SwordMan)
                    {
                        goto case UnitType.Archer;
                    }
                    break;
                case UnitType.Archer:
                    t_preOtherUnitRow += m_swordManLastRow + 1;
                    break;
            }

            int t_currentRow = startIndex / m_maxUnitPerRow;
            int t_remainUnitCount = unitList.Count - (t_currentRow * m_maxUnitPerRow);
            Vector3 t_unitPosition = Vector3.zero;
            while (t_remainUnitCount > 0)
            {
                int t_unitCountThisRow = (t_remainUnitCount > m_maxUnitPerRow)
                    ? m_maxUnitPerRow : t_remainUnitCount;

                int t_thisRowStartIndex = t_currentRow * m_maxUnitPerRow;
                t_unitPosition.x = (t_currentRow + t_preOtherUnitRow) * m_commonDifferenceX + m_beginX;
                for (int i = 0; i < t_unitCountThisRow; i++)
                {
                    t_unitPosition.z = m_totalLengthZ * (i + 1) / (t_unitCountThisRow + 1);

                    int t_unitIndex = t_thisRowStartIndex + i;
                    if (t_unitIndex < unitList.Count)
                    {
                        unitList[t_unitIndex].DefensePosition = t_unitPosition;
                    }
                }

                t_remainUnitCount -= t_unitCountThisRow;
                t_currentRow++;
            }
        }

        private void LineUpAfterSwordManRowChange()
        {
            LineUp(m_archerList, UnitType.Archer);
        }

        private void Start()
        {
            m_group = (GameManager.Instance.IsSYWS(gameObject.layer)) ? Group.SYWS : Group.NLI;

            SwitchCommand(Command.Defend);
        }
    }
}