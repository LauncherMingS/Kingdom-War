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
        private const int UnitNum = 3;//SwordMan, Archer, Priest
        private const int EnumValueShift = -1;
        [SerializeField] private int[] m_unitsLastRow;
        [Header("Position")]
        [SerializeField] private Vector3 m_enemyBasePosition;
        [SerializeField] private Vector3 m_retreatPosition;
        [Header("Unit List")]
        [SerializeField] private List<SwordMan> m_swordManList;
        [SerializeField] private List<Archer> m_archerList;
        [SerializeField] private List<Priest> m_priestList;

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

            if (m_currentCommand == Command.Attack)
            {
                InvokeRepeating(nameof(LineUpPriestToStandBy), 0f, 1f);
            }
            else
            {
                if (IsInvoking(nameof(LineUpPriestToStandBy)))
                {
                    CancelInvoke(nameof(LineUpPriestToStandBy));
                }

                if (m_currentCommand == Command.Defend)
                {
                    LineUp(m_swordManList, UnitType.SwordMan);
                    LineUp(m_archerList, UnitType.Archer);
                    LineUp(m_priestList, UnitType.Priest);
                }
            }
        }

        //Button Listener(Recruit)
        public void RecruitUnit(int unitEnumValue)//SwordMan: 1, Archer: 2, Priest: 3
        {
            if (!GameManager.EnumIsDefined<UnitType>(unitEnumValue, name))
            {
                return;
            }

            //Use for array m_unitsLastRow
            int t_typeValue = unitEnumValue + EnumValueShift;
            switch ((UnitType)unitEnumValue)
            {
                case UnitType.SwordMan:
                    RecruitUnit(m_swordManList, UnitType.SwordMan, t_typeValue, LineUpAfterChange);
                    break;
                case UnitType.Archer:
                    RecruitUnit(m_archerList, UnitType.Archer, t_typeValue, LineUpAfterChange);
                    break;
                case UnitType.Priest:
                    RecruitUnit(m_priestList, UnitType.Priest, t_typeValue);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Recruit this unit type[{(UnitType)unitEnumValue}]");
                    break;
            }
        }

        private void RecruitUnit<T>(List<T> unitList, UnitType unitType, int unitIndex
            , Action<int> lineUpOther = null) where T : Unit
        {
            //Get unit, and handle unit settings
            T t_unit = ObjectPoolManagerSO.Instance.Get<T>(m_group, unitType);
            t_unit.EnemyBasePosition = m_enemyBasePosition;
            t_unit.RetreatPosition = m_retreatPosition;
            t_unit.Initialize();
            t_unit.transform.position = transform.position;

            unitList.Add(t_unit);

            //Calculate the total row number of current unit type
            //If the row number changes, adjust the other units' line up after the current unit type
            int t_originalLastRow = m_unitsLastRow[unitIndex];
            m_unitsLastRow[unitIndex] = CalculateLastRowIndex(unitList);
            if (m_unitsLastRow[unitIndex] > t_originalLastRow)
            {
                lineUpOther?.Invoke(unitIndex);
            }
            LineUp(unitList, unitType, unitList.Count - 1);
        }

        public void RemoveUnit<T>(UnitType unitType, T unit)
        {
            //Use for array m_unitsLastRow
            int t_typeValue = (int)unitType + EnumValueShift;
            switch (unitType)
            {
                case UnitType.SwordMan:
                    RemoveUnit(m_swordManList, unitType, unit as SwordMan, t_typeValue, LineUpAfterChange);
                    break;
                case UnitType.Archer:
                    RemoveUnit(m_archerList, unitType, unit as Archer, t_typeValue, LineUpAfterChange);
                    break;
                case UnitType.Priest:
                    RemoveUnit(m_priestList, unitType, unit as Priest, t_typeValue);
                    break;
                default:
                    GameManager.LogWarningEditor($"{name}: Cannot Remove this unit type[{unitType}]");
                    break;
            }
        }

        public void RemoveUnit<T>(List<T> unitList, UnitType unitType, T unit, int unitIndex
            , Action<int> lineUpOther = null) where T : Unit
        {
            //Prevent the next unit from being detected by the enemy unit the moment it is generated(recruited)
            unit.transform.position = transform.position;
            unit.UnInitialize();

            unitList.Remove(unit);
            ObjectPoolManagerSO.Instance.Recycle(m_group, unitType, unit);

            //Calculate the total row number of current unit type
            //If the row number changes, adjust the other units' line up after the current unit type
            int t_originalLastRow = m_unitsLastRow[unitIndex];
            m_unitsLastRow[unitIndex] = CalculateLastRowIndex(unitList);
            if (m_unitsLastRow[unitIndex] < t_originalLastRow)
            {
                lineUpOther?.Invoke(unitIndex);
            }
            LineUp(unitList, unitType);
        }

        public void FirstAid(Transform target)
        {
            int t_priestLength = m_priestList.Count;
            if (t_priestLength == 0)
            {
                return;
            }

            for (int i = 0; i < t_priestLength; i++)
            {
                if (!m_priestList[i].IsBusyOnHealing)
                {
                    if (m_priestList[i].SetHealTarget(target))
                    {
                        return;
                    }
                }
            }
        }

        //-1 means the unit list is empty, row numbers start from 0
        private int CalculateLastRowIndex<T>(List<T> unitList)
        {
            if (unitList.Count == 0)
            {
                return -1;
            }

            int t_lastRowIndex = unitList.Count / m_maxUnitPerRow;
            return (unitList.Count % m_maxUnitPerRow == 0) ? --t_lastRowIndex : t_lastRowIndex;
        }

        private void LineUp<T>(List<T> unitList, UnitType unitType, int startIndex = 0
            , float beginX = float.NaN) where T : Unit
        {
            //Allow entry when Defense Command or lining up priests when Attack Command
            if (m_currentCommand != Command.Defend && float.IsNaN(beginX))
            {
                return;
            }

            //Block empty unit list or incorrect start index
            if (unitList.Count == 0 || startIndex >= unitList.Count)
            {
                return;
            }
            
            //When Defense Command, according to the current unit type, calculate
            //the number of rows of the previous unit type
            int t_preOtherUnitRow = 0;
            if (float.IsNaN(beginX))
            {
                beginX = m_beginX;
                int t_typeValue = (int)unitType + EnumValueShift;
                for (int i = 0; i < t_typeValue; i++)
                {
                    if (m_unitsLastRow[i] == -1)
                    {
                        continue;
                    }

                    t_preOtherUnitRow += m_unitsLastRow[i] + 1;
                }
            }

            //Start lining up from specified unit index(startIndex), arranging four unit in a loop
            int t_currentRow = startIndex / m_maxUnitPerRow;
            int t_remainUnitCount = unitList.Count - (t_currentRow * m_maxUnitPerRow);
            Vector3 t_unitPosition = Vector3.zero;
            while (t_remainUnitCount > 0)
            {
                //Base on the current row, determine how many units are left to be arranged.
                //If there are more than four(m_maxUnitPerRow), only four are arranged.
                int t_unitCountThisRow = (t_remainUnitCount > m_maxUnitPerRow)
                    ? m_maxUnitPerRow : t_remainUnitCount;

                //Calculate unit start index
                int t_thisRowStartIndex = t_currentRow * m_maxUnitPerRow;
                t_unitPosition.x = (t_currentRow + t_preOtherUnitRow) * m_commonDifferenceX + beginX;
                for (int i = 0; i < t_unitCountThisRow; i++)
                {
                    //Evenly distribute according to the current number of units
                    t_unitPosition.z = m_totalLengthZ * (i + 1) / (t_unitCountThisRow + 1);

                    //Calculate unit index
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

        //這部分因為泛型的限制，真的不知道該如何取得對應List
        private float FindFrontRowPositionX()
        {
            if (m_swordManList.Count > 0)
            {
                return m_swordManList[^1].transform.position.x;
            }

            if (m_archerList.Count > 0)
            {
                return m_archerList[^1].transform.position.x;
            }

            return float.NaN;
        }

        //Repeating Invoke by InvokeRepeating(), 1 invoke / second, no initial buffer time
        private void LineUpPriestToStandBy()
        {
            float t_positionX = FindFrontRowPositionX();

            if (!float.IsNaN(t_positionX))
            {
                t_positionX += m_commonDifferenceX;
            }
            else
            {
                t_positionX = m_enemyBasePosition.x;
                t_positionX += 3 * m_commonDifferenceX;
            }

            LineUp(m_priestList, UnitType.Priest, 0, t_positionX);
        }

        //這部分因為泛型的限制，真的不知道該如何取得對應List
        private void LineUpAfterChange(int typeValue)
        {
            typeValue++;//Don't line up current unit type

            if (typeValue == 1)
            {
                LineUp(m_archerList, UnitType.Archer);
                typeValue++;
            }

            if (typeValue == 2)
            {
                LineUp(m_priestList, UnitType.Priest);
                typeValue++;
            }
        }

        private void Start()
        {
            m_group = (GameManager.Instance.IsSYWS(gameObject.layer)) ? Group.SYWS : Group.NLI;
            m_unitsLastRow = new int[UnitNum] { -1, -1, -1 };

            SwitchCommand(Command.Defend);
        }
    }
}