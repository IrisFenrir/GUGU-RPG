using UnityEditor;
using UnityEngine;

namespace RPG.InputSystem
{
    public class InputCellEditor : Editor
    {
        private static string inputCellPath = "Assets/Resources/InputCell.prefab";

        [MenuItem("GameObject/My Tools/Input Cell",priority = 0)]
        public static void CreateInputCell()
        {
            GameObject inputCell_GO = AssetDatabase.LoadAssetAtPath<GameObject>(inputCellPath);
            if (inputCell_GO != null)
                Instantiate(inputCell_GO, Selection.activeTransform).name = "InputCell";
        }
    }
}

