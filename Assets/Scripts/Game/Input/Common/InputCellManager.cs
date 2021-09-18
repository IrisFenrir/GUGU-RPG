using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RPG.InputSystem
{
    public class InputCellManager
    {
        private static List<InputCell> cells = new List<InputCell>();

        // 添加单元格
        public static void AddCell(InputCell cell)
        {
            cells.Add(cell);
        }
        // 设置所有单元格的显示文本
        public static void SetAllCellsKeyText()
        {
            foreach (InputCell cell in cells)
            {
                switch(cell.type)
                {
                    case KeyType.Key:
                        cell.SetKeyText(InputManager.GetKeyCode(cell.keyName));
                        break;
                    case KeyType.ValueKey:
                        cell.SetKeyText(InputManager.GetValueKeyCode(cell.keyName));
                        break;
                    case KeyType.AxisPosKey:
                        cell.SetKeyText(InputManager.GetAxisPosKeyCode(cell.keyName));
                        break;
                    case KeyType.AxisNegKey:
                        cell.SetKeyText(InputManager.GetAxisNegKeyCode(cell.keyName));
                        break;
                }
                
            }
        }
    }
}
