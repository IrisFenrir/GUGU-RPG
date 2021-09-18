using System;
using UnityEngine;

namespace RPG.InputSystem
{
    public class InputManager
    {
        // 默认键位数据保存位置
        private static string m_DefaultDataSavePath = "/Resources/DefaultInputData.json";
        // 自定义键位数据保存位置
        private static string m_CustomDataSavePath = "/Resources/CustomInputData.json";
        // 键位数据
        private static InputData m_InputData;
        // 是否激活键位更新
        private static bool m_ActiveInput;
        // 设置键位委托
        private static Action<KeyCode> SetKeyHandle;
        // 展示键位委托
        private static Action<KeyCode> DisplayKeyHandle;

        public InputManager(InputData inputData)
        {
            m_InputData = inputData;
            SaveDefaultSetting();
            LoadDefaultSetting();
        }

        // 输入更新，在主循环里调用
        public void Update()
        {
            m_InputData.AcceptInput();

            if(m_ActiveInput)
            {
                if(Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if(Input.GetKeyDown(keyCode))
                        {
                            if(SetKeyHandle != null)
                                SetKeyHandle(keyCode);
                            if(DisplayKeyHandle != null)
                                DisplayKeyHandle(keyCode);
                            m_ActiveInput = false;
                            SetKeyHandle = null;
                            DisplayKeyHandle = null;
                        }
                    }
                }
            }
        }

        public static bool GetKey(string name)
        {
            return m_InputData.GetKeyDown(name);
        }
        public static bool GetKeyDown(string name)
        {
            return m_InputData.GetKeyDown(name);
        }
        public static bool GetKeyDonwTwice(string name)
        {
            return m_InputData.GetKeyDownTwice(name);
        }
        public static float GetValue(string name)
        {
            return m_InputData.GetValue(name);
        }
        public static float GetAxis(string name)
        {
            return m_InputData.GetAxis(name);
        }

        public static void SetKey(string name, KeyCode keyCode)
        {
            m_InputData.SetKey(name, keyCode);
        }
        public static void SetValueKey(string name, KeyCode keyCode)
        {
            m_InputData.SetValueKey(name, keyCode);
        }
        public static void SetAxisKey(string name,KeyCode pos,KeyCode neg)
        {
            m_InputData.SetAxisKey(name, pos, neg);
        }
        public static void SetAxisPosKey(string name,KeyCode pos)
        {
            m_InputData.SetAxisPosKey(name, pos);
        }
        public static void SetAxisNegKey(string name, KeyCode neg)
        {
            m_InputData.SetAxisNegKey(name, neg);
        }

        public static void StartSetKey(Action<KeyCode> setKey,Action<KeyCode> displayKey)
        {
            m_ActiveInput = true;
            SetKeyHandle = setKey;
            DisplayKeyHandle = displayKey;
        }

        public static void SaveDefaultSetting()
        {
            m_InputData.SaveInputSetting(m_DefaultDataSavePath);
        }
        public static void LoadDefaultSetting()
        {
            m_InputData.LoadInputSetting(m_DefaultDataSavePath);
        }
        public static void SaveCustomSetting()
        {
            m_InputData.SaveInputSetting(m_CustomDataSavePath);
        }
        public static void LoadCustomSetting()
        {
            m_InputData.LoadInputSetting(m_CustomDataSavePath);
        }

        public static KeyCode GetKeyCode(string name)
        {
            Key key = m_InputData.GetKeyObject(name);
            if (key != null)
                return key.keyCode;
            return KeyCode.None;
        }
        public static KeyCode GetValueKeyCode(string name)
        {
            ValueKey valueKey = m_InputData.GetValueKeyObject(name);
            if (valueKey != null)
                return valueKey.keyCode;
            return KeyCode.None;
        }
        public static KeyCode GetAxisPosKeyCode(string name)
        {
            AxisKey axisKey = m_InputData.GetAxisKeyObject(name);
            if (axisKey != null)
                return axisKey.posKey;
            return KeyCode.None;
        }
        public static KeyCode GetAxisNegKeyCode(string name)
        {
            AxisKey axisKey = m_InputData.GetAxisKeyObject(name);
            if (axisKey != null)
                return axisKey.negKey;
            return KeyCode.None;
        }
    }
}
