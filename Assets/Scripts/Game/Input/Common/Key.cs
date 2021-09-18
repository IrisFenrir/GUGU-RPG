using System;
using UnityEngine;

namespace RPG.InputSystem
{
    public enum KeyTrigger
    {
        Once,        // 单击
        Double,      // 双击
        Continuity   // 持续
    }

    [Serializable]
    public class Key
    {
        public string name; //按键名字
        public KeyTrigger trigger; //触发类型

        [HideInInspector]
        public bool isDown; //是否按下

        [HideInInspector]
        public bool isDoubleDown; //是否双击
        [HideInInspector]
        public bool acceptDoubleDown; //开始检测双击
        public float pressInterval = 1f;  //双击间隔时间
        [HideInInspector]
        public float realInterval; //真实间隔

        public KeyCode keyCode; //对应的KeyCode
        [HideInInspector]
        public bool enable = true; //是否启用

        public void SetKey(KeyCode key)
        {
            keyCode = key;
        }
        public void SetEnable(bool enable)
        {
            this.enable = enable;
            isDown = false;
            isDoubleDown = false;
        }
    }

    [Serializable]
    public class ValueKey
    {
        public string name;
        public Vector2 range = new Vector2(0, 1);
        [HideInInspector]
        public float value = 0f;
        public float addSpeed = 3f;
        public KeyCode keyCode;
        [HideInInspector]
        public bool enable = true;

        public void SetKey(KeyCode key)
        {
            keyCode = key;
        }
        public void SetEnable(bool enable)
        {
            this.enable = enable;
            value = 0f;
        }
    }

    [Serializable]
    public class AxisKey
    {
        public string name;
        public Vector2 range = new Vector2(-1, 1);
        [HideInInspector]
        public float value = 0f;
        public float addSpeed = 5f;
        public KeyCode posKey;
        public KeyCode negKey;
        [HideInInspector]
        public bool enable = true;

        public void SetKey(KeyCode pos,KeyCode neg)
        {
            posKey = pos;
            negKey = neg;
        }
        public void SetPosKey(KeyCode pos)
        {
            posKey = pos;
        }
        public void SetNegKey(KeyCode neg)
        {
            negKey = neg;
        }

        public void SetEnable(bool enable)
        {
            this.enable = enable;
            value = 0f;
        }
    }
}
