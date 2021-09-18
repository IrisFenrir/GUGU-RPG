using UnityEditor;
using UnityEngine;

namespace RPG.EventSystem
{
    public class EventGraphView
    {
        private EventGraphWindow m_Window;
        private static EventGroupNode m_Root;
        private int currentID;
        private EventGraph m_Graph;

        public EventGraphView(EventGraphWindow window,EventGraph graph)
        {
            m_Window = window;
            m_Graph = graph;
            LoadData(graph);
        }
        private void LoadData(EventGraph graph)
        {
            if(graph.nodes == null || graph.nodes.Count == 0)
            {
                m_Root = new EventGroupNode() { id = currentID++, eventName = "Root" };
            }
            else
            {
                m_Root = new EventGroupNode() { id = currentID++, eventName = "Root" };
                m_Root.LoadNode(graph.nodes, 0);
                currentID = m_Graph.currentID;
            }
        }
        public void SaveData()
        {
            if(m_Graph.nodes != null)
                m_Graph.nodes.Clear();
            m_Root.CopyNode(m_Graph.nodes);
            m_Graph.currentID = currentID;

            EditorUtility.SetDirty(m_Graph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Draw()
        {
            EditorGUI.DrawRect(new Rect(0, 0, m_Window.position.width, m_Window.position.height), new Color(0.1f, 0.1f, 0.1f, 0.8f));
            m_Root.Draw();
        }

        public bool ClickOnNode(Vector2 pos,out BaseEventNode node)
        {
            if(m_Root != null)
            {
                return m_Root.ClickOn(pos, out node);
            }
            node = null;
            return false;
        }

        public void AddNode<T>(object obj) where T:BaseEventNode,new()
        {
            EventGroupNode node = (EventGroupNode)obj;
            T newNode = new T() { id = currentID++ };
            newNode.nodeRect.x = node.nodeRect.x + 300;
            newNode.nodeRect.y = node.nodeRect.y;
            newNode.parent = node;
            node.AddNode(newNode);
        }

        public void DeleteNode(object obj)
        {
            BaseEventNode node = (BaseEventNode)obj;
            node.DeleteNode();
        }

        public static bool IsEventNameRepeated(string name,int id)
        {
            return m_Root.IsNameRepeated(name, id);
        }

        public void Move(Vector2 delta)
        {
            m_Root.MoveNode(delta);
            m_Window.Repaint();
        }
    }
}

