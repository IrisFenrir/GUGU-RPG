using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG.InputSystem
{
    [Serializable]
    public class InputData
    {
        public List<Key> keys = new List<Key>() { new Key() };
        public List<ValueKey> valueKeys = new List<ValueKey>() { new ValueKey() };
        public List<AxisKey> axisKeys = new List<AxisKey>() { new AxisKey() };


        /// <summary>
        /// 获取Key对象
        /// </summary>
        public Key GetKeyObject(string name)
        {
            return keys.Find(key => key.name == name);
        }
        /// <summary>
        /// 获取ValueKey对象
        /// </summary>
        public ValueKey GetValueKeyObject(string name)
        {
            return valueKeys.Find(valueKey => valueKey.name == name);
        }
        /// <summary>
        /// 获取AxisKey对象
        /// </summary>
        public AxisKey GetAxisKeyObject(string name)
        {
            return axisKeys.Find(axisKey => axisKey.name == name);
        }

        // 设置普通按键
        public void SetKey(string name,KeyCode keyCode)
        {
            Key key = GetKeyObject(name);
            if(key != null)
                key.SetKey(keyCode);
        }
        // 设置数值按键
        public void SetValueKey(string name,KeyCode keyCode)
        {
            ValueKey valueKey = GetValueKeyObject(name);
            if(valueKey != null)
                valueKey.SetKey(keyCode);
        }
        // 设置轴按键
        public void SetAxisKey(string name,KeyCode posKey,KeyCode negKey)
        {
            AxisKey axisKey = GetAxisKeyObject(name);
            if(axisKey != null)
                axisKey.SetKey(posKey, negKey);
        }
        // 设置轴按键正轴
        public void SetAxisPosKey(string name,KeyCode posKey)
        {
            AxisKey axisKey = GetAxisKeyObject(name);
            if (axisKey != null)
                axisKey.SetPosKey(posKey);
        }
        // 设置轴按键负轴
        public void SetAxisNegKey(string name, KeyCode negKey)
        {
            AxisKey axisKey = GetAxisKeyObject(name);
            if (axisKey != null)
                axisKey.SetNegKey(negKey);
        }

        // 按键是否按下
        public bool GetKeyDown(string name)
        {
            Key key = GetKeyObject(name);
            if(key != null)
            {
                return key.isDown;
            }
            return false;
        }
        // 按键是否双击
        public bool GetKeyDownTwice(string name)
        {
            Key key = GetKeyObject(name);
            if(key != null)
            {
                return key.isDoubleDown;
            }
            return false;
        }
        // 获取值
        public float GetValue(string name)
        {
            ValueKey valueKey = GetValueKeyObject(name);
            if(valueKey != null)
            {
                return valueKey.value;
            }
            return 0f;
        }
        // 获取轴值
        public float GetAxis(string name)
        {
            AxisKey axisKey = GetAxisKeyObject(name);
            if(axisKey != null)
            {
                return axisKey.value;
            }
            return 0f;
        }

        public void SetKeyEnable(string name,bool enable)
        {
            Key key = GetKeyObject(name);
            if(key != null)
            {
                key.SetEnable(enable);
            }
        }
        public void SetValueKeyEnable(string name,bool enable)
        {
            ValueKey valueKey = GetValueKeyObject(name);
            if(valueKey != null)
            {
                valueKey.SetEnable(enable);
            }
        }
        public void SetAxisKeyEnable(string name,bool enable)
        {
            AxisKey axisKey = GetAxisKeyObject(name);
            if(axisKey != null)
            {
                axisKey.SetEnable(enable);
            }
        }

        // 接受输入
        public void AcceptInput()
        {
            UpdateKeys();
            UpdateValueKeys();
            UpdateAxisKeys();
        }
        // 更新普通按键
        private void UpdateKeys()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if(keys[i].enable == true)
                {
                    keys[i].isDown = false;
                    keys[i].isDoubleDown = false;
                    switch(keys[i].trigger)
                    {
                        case KeyTrigger.Once:
                            // 封装Input.GetKeyDown方法
                            if(Input.GetKeyDown(keys[i].keyCode))
                            {
                                keys[i].isDown = true;
                            }
                            break;
                        case KeyTrigger.Double:
                            if(keys[i].acceptDoubleDown)
                            {
                                keys[i].realInterval += Time.fixedDeltaTime;
                                if (keys[i].realInterval > keys[i].pressInterval)
                                {
                                    keys[i].acceptDoubleDown = false;
                                    keys[i].realInterval = 0f;
                                }
                                else if(Input.GetKeyDown(keys[i].keyCode))
                                {
                                    keys[i].isDoubleDown = true;
                                    keys[i].realInterval = 0f;
                                }
                                else if(Input.GetKeyUp(keys[i].keyCode))
                                {
                                    keys[i].acceptDoubleDown = false;
                                }
                            }
                            else
                            {
                                if(Input.GetKeyUp(keys[i].keyCode))
                                {
                                    keys[i].acceptDoubleDown = true;
                                    keys[i].realInterval = 0f;
                                }
                            }

                            break;
                        case KeyTrigger.Continuity:
                            if(Input.GetKey(keys[i].keyCode))
                            {
                                keys[i].isDown = true;
                            }
                            break;
                    }
                }
            }
        }
        // 更新数值按键
        private void UpdateValueKeys()
        {
            for (int i = 0; i < valueKeys.Count; i++)
            {
                if(valueKeys[i].enable)
                {
                    if(Input.GetKey(valueKeys[i].keyCode))
                    {
                        valueKeys[i].value = Mathf.Clamp(valueKeys[i].value + valueKeys[i].addSpeed * Time.fixedDeltaTime, 
                            valueKeys[i].range.x, valueKeys[i].range.y);
                    }
                    else
                    {
                        valueKeys[i].value = Mathf.Clamp(valueKeys[i].value - valueKeys[i].addSpeed * Time.fixedDeltaTime,
                            valueKeys[i].range.x, valueKeys[i].range.y);
                    }
                }
            }
        }
        // 更新轴按键
        private void UpdateAxisKeys()
        {
            for (int i = 0; i < axisKeys.Count; i++)
            {
                if (axisKeys[i].enable)
                {
                    if(Input.GetKey(axisKeys[i].posKey))
                    {
                        axisKeys[i].value = Mathf.Clamp(axisKeys[i].value + axisKeys[i].addSpeed * Time.fixedDeltaTime,
                            axisKeys[i].range.x, axisKeys[i].range.y);
                    }
                    else if(Input.GetKey(axisKeys[i].negKey))
                    {
                        axisKeys[i].value = Mathf.Clamp(axisKeys[i].value - axisKeys[i].addSpeed * Time.fixedDeltaTime,
                            axisKeys[i].range.x, axisKeys[i].range.y);
                    }
                    else
                    {
                        axisKeys[i].value = Mathf.Lerp(axisKeys[i].value, 0f, axisKeys[i].addSpeed * Time.fixedDeltaTime);
                        if (Mathf.Abs(axisKeys[i].value) < 0.01f)
                            axisKeys[i].value = 0f;
                    }
                }
            }
        }

        // 保存当前键位设置
        public void SaveInputSetting(string path)
        {
            JsonData json = new JsonData();
            json["Keys"] = new JsonData();
            foreach (Key key in keys)
            {
                json["Keys"][key.name] = key.keyCode.ToString();
            }
            json["ValueKeys"] = new JsonData();
            foreach (ValueKey valueKey in valueKeys)
            {
                json["ValueKeys"][valueKey.name] = valueKey.keyCode.ToString();
            }
            json["AxisKeys"] = new JsonData();
            foreach (AxisKey axisKey in axisKeys)
            {
                json["AxisKeys"][axisKey.name] = new JsonData();
                json["AxisKeys"][axisKey.name]["Pos"] = axisKey.posKey.ToString();
                json["AxisKeys"][axisKey.name]["Neg"] = axisKey.negKey.ToString();
            }

            string filePath = Application.dataPath + path;
            FileInfo file = new FileInfo(filePath);
            StreamWriter sw = file.CreateText();
            sw.WriteLine(json.ToJson());
            sw.Close();
            sw.Dispose();

        }
        // 从指定文件加载键位设置
        public void LoadInputSetting(string path)
        {
            string filePath = Application.dataPath + path;
            if (!File.Exists(filePath))
                return;
            
            string data = File.ReadAllText(filePath);
            JsonData json = JsonMapper.ToObject(data);
            foreach (Key key in keys)
            {
                key.keyCode = String2Enum_KeyCode(json["Keys"][key.name].ToString());
            }
            foreach (ValueKey valueKey in valueKeys)
            {
                valueKey.keyCode = String2Enum_KeyCode(json["ValueKeys"][valueKey.name].ToString());
            }
            foreach (AxisKey axisKey in axisKeys)
            {
                axisKey.posKey = String2Enum_KeyCode(json["AxisKeys"][axisKey.name]["Pos"].ToString());
                axisKey.negKey = String2Enum_KeyCode(json["AxisKeys"][axisKey.name]["Neg"].ToString());
            }
        }
        // 将string转换为KeyCode
        private KeyCode String2Enum_KeyCode(string key)
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), key);
        }
    }
}
