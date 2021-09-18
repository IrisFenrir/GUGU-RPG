using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem
{
    [CreateAssetMenu(fileName = "New Event Graph",menuName = "My Event Graph")]
    public class EventGraph : ScriptableObject
    {
        public List<SerializableNode> nodes = new List<SerializableNode>();
        public int currentID;
    }
}