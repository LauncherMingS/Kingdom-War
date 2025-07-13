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
        private const int UnitNum = 2;//SwordMan, Archer
        private const int EnumValueShift = 1;
        [SerializeField] private int[] m_unitsLastRow;
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
                    RecruitUnit(m_swordManList, UnitType.SwordMan, 0, LineUpAfterSwordManRowChange);
                    break;
                case UnitType.Archer:
                    RecruitUnit(m_archerList, UnitType.Archer, 1);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Recruit this unit type[{(UnitType)typeValue}]");
                    break;
            }
        }

        private void RecruitUnit<T>(List<T> unitList, UnitType unitType, int lastRowIndex
            , Action lineUpOther = null) where T : Unit
        {
            T t_unit = ObjectPoolManagerSO.Instance.Get<T>(m_group, unitType);
            t_unit.EnemyBasePosition = m_enemyBasePosition;
            t_unit.RetreatPosition = m_retreatPosition;
            t_unit.Initialize();
            t_unit.transform.position = transform.position;

            unitList.Add(t_unit);

            int t_newLastRow = CalculateLastRowIndex(unitList);
            if (t_newLastRow > m_unitsLastRow[lastRowIndex])
            {
                m_unitsLastRow[lastRowIndex] = t_newLastRow;
                lineUpOther?.Invoke();
            }
            m_unitsLastRow[lastRowIndex] = t_newLastRow;
            LineUp(unitList, unitType, unitList.Count - 1);
        }

        public void RemoveUnit<T>(UnitType unitType, T unit)
        {
            switch (unitType)
            {
                case UnitType.SwordMan:
                    RemoveUnit(m_swordManList, unitType, unit as SwordMan, 0, LineUpAfterSwordManRowChange);
                    break;
                case UnitType.Archer:
                    RemoveUnit(m_archerList, unitType, unit as Archer, 1);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Remove this unit type[{unitType}]");
                    break;
            }
        }

        public void RemoveUnit<T>(List<T> unitList, UnitType unitType, T unit, int lastRowIndex
            , Action lineUpOther = null) where T : Unit
        {
            unit.UnInitialize();

            unitList.Remove(unit);
            ObjectPoolManagerSO.Instance.Recycle(m_group, unitType, unit);

            int t_newLastRow = CalculateLastRowIndex(unitList);
            if (t_newLastRow < m_unitsLastRow[lastRowIndex])
            {
                m_unitsLastRow[lastRowIndex] = t_newLastRow;
                lineUpOther?.Invoke();
            }
            m_unitsLastRow[lastRowIndex] = t_newLastRow;
            LineUp(unitList, unitType, unitList.Count - 1);
        }

        private int CalculateLastRowIndex<T>(List<T> unitList)
        {
            if (unitList.Count == 0)
            {
                return -1;
            }

            int t_lastRowIndex = unitList.Count / m_maxUnitPerRow;
            return (unitList.Count % m_maxUnitPerRow == 0) ? --t_lastRowIndex : t_lastRowIndex;
        }

        private int GetListCount(int index)
        {
            return index switch
            {
                0 => m_swordManList.Count,
                1 => m_archerList.Count,
                _ => -1
            };
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

            int t_typeValue = (int)unitType - EnumValueShift;
            int t_preOtherUnitRow = 0;
            for (int i = 0; i < t_typeValue; i++)
            {
                if (GetListCount(i) > 0)
                {
                    t_preOtherUnitRow += m_unitsLastRow[i] + 1;
                }
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
            m_unitsLastRow = new int[UnitNum] { -1, -1 };

            SwitchCommand(Command.Defend);
        }
    }
}