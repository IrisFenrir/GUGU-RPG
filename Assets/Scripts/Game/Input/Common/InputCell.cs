using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.InputSystem
{
    public enum KeyType
    {
        Key,
        ValueKey,
        AxisPosKey,
        AxisNegKey
    }

    public class InputCell : MonoBehaviour, IPointerClickHandler
    {
        public KeyType type;
        public string keyName;
        private Text keyCode_Text;
        private Image keyBG_Image;
        private Action<KeyCode> SetKey;

        private void Awake()
        {
            InputCellManager.AddCell(this);
            keyBG_Image = GetComponentInChildren<Image>();
            keyCode_Text = keyBG_Image.GetComponentInChildren<Text>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            keyCode_Text.text = "Press Key";
            switch(type)
            {
                case KeyType.Key:
                    SetKey = (key) => InputManager.SetKey(keyName, key);
                    break;
                case KeyType.ValueKey:
                    SetKey = (key) => InputManager.SetValueKey(keyName, key);
                    break;
                case KeyType.AxisPosKey:
                    SetKey = (key) => InputManager.SetAxisPosKey(keyName, key);
                    break;
                case KeyType.AxisNegKey:
                    SetKey = (key) => InputManager.SetAxisNegKey(keyName, key);
                    break;
            }
            InputManager.StartSetKey(SetKey, (key) => keyCode_Text.text = key.ToString());
        }

        public void SetKeyText(KeyCode keyCode)
        {
            keyCode_Text.text = keyCode.ToString();
        }
    }
}
