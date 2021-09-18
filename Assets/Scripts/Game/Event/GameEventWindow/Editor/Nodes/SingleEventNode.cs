using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystem
{
    public class SingleEventNode : BaseEventNode
    {
        public SingleEventNode()
        {
            nodeTitle = "Event";
            m_TextColor = Color.cyan;
        }

        public override void CopyNode(List<SerializableNode> container)
        {
            if (!legal)
                return;
            SerializableNode snode = new SerializableNode();
            snode.type = "Single";
            snode.name = eventName;
            snode.id = id;
            snode.rect = nodeRect;
            container.Add(snode);
        }
        public override void LoadNode(List<SerializableNode> container, int id)
        {
            var data = container.Find(n => n.id == id);
            if(data != null)
            {
                eventName = data.name;
                this.id = data.id;
                nodeRect = data.rect;
            }
        }
    }
}

