using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem
{
    [System.Serializable]
    public class SerializableNode
    {
        public string name;
        public string type;
        public int id;
        public Rect rect;
        public List<int> children;
    }
}