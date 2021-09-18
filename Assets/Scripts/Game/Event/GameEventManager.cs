using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.EventSystem
{
    /// <summary>
    /// 事件处理器
    /// </summary>
    public static class GameEventManager
    {
        /// <summary>
        /// Root事件组，管理游戏内的所有事件
        /// </summary>
        private static GameEventGroup m_root;
        public static GameEventGroup RootGroup 
        {
            get
            {
                return m_root;
            }
            set
            {
                m_root = value;
            }
        }

        public struct NodeEvent
        {
            public SerializableNode node;
            public GameBaseEvent gameEvent;
        }

        static GameEventManager()
        {
            //HardInit();
            ImportDataFromGraph();

            RootGroup.enable = true;
            EnableEvent("PlayerInput", true);
        }
        private static void HardInit()
        {
            RootGroup = new GameEventGroup("Root");
            GameEventGroup playerInput = new GameEventGroup("PlayerInput");
            GameEventGroup camera = new GameEventGroup("Camera");
            GameEventGroup backage = new GameEventGroup("Backage");
            RootGroup.AddEvent(playerInput);
            RootGroup.AddEvent(camera);
            RootGroup.AddEvent(backage);

            // Player Input
            playerInput.AddEvent(new GameEvent("PlayerMove"));
            playerInput.AddEvent(new GameEvent("PlayerDash"));
            playerInput.AddEvent(new GameEvent("LockCamera"));
            playerInput.AddEvent(new GameEvent("PickUp"));
            playerInput.AddEvent(new GameEvent("OpenBackage"));
            playerInput.EnableAllEvent(true);

            // Camera
            camera.AddEvent(new GameEvent("CameraRotate"));
            camera.EnableAllEvent(true);

            RootGroup.enable = true;
        }
        private static void ImportDataFromGraph()
        {
            var graph = AssetDatabase.LoadAssetAtPath<EventGraph>("Assets/Resources/Data/Event Data/Core/EventCore.asset");

            var root = graph.nodes.Find(n => n.name == "Root");
            if(root == null)
            {
                Debug.LogError("Can not find the root node.");
                return;
            }
            RootGroup = new GameEventGroup("Root");
            Queue<NodeEvent> nodeQueue = new Queue<NodeEvent>();
            nodeQueue.Enqueue(new NodeEvent() { node = root, gameEvent = RootGroup });
            while(nodeQueue.Count > 0)
            {
                var node = nodeQueue.Dequeue();
                if(node.node != null && node.node.type == "Group" && node.node.children != null && node.node.children.Count > 0)
                {
                    for (int i = 0; i < node.node.children.Count; i++)
                    {
                        var child = graph.nodes.Find(n => n.id == node.node.children[i]);
                        if(child != null)
                        {
                            if(child.type == "Single")
                            {
                                var newEvent = new GameEvent(child.name);
                                ((GameEventGroup)node.gameEvent).AddEvent(newEvent);
                            }
                            else if(child.type == "Group")
                            {
                                var newEvent = new GameEventGroup(child.name);
                                ((GameEventGroup)node.gameEvent).AddEvent(newEvent);
                                nodeQueue.Enqueue(new NodeEvent() { node = child, gameEvent = newEvent });
                            }
                        }
                    }
                }
            }
        }
        

        public static void RegisterEvent(string eventName,GameEvent.CheckHandle check)
        {
            var target = RootGroup?.GetEvent(eventName);
            if(target != null && target is GameEvent temp)
            {
                temp.AddCheckHandle(check);
            }
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="response"></param>
        public static void SubscribeEvent(string eventName,GameEvent.ResponseHandle response)
        {
            var target = RootGroup?.GetEvent(eventName);
            if (target != null && target is GameEvent temp)
            {
                temp.AddResponse(response);
            }
        }

        /// <summary>
        /// 启用/禁用事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="_enable"></param>
        public static void EnableEvent(string eventName,bool _enable)
        {
            var target = RootGroup?.GetEvent(eventName);
            if(target != null)
            {
                target.enable = _enable;
            }
        }
        public static void EnableAllEvents(string eventName,bool _enable)
        {
            var target = RootGroup?.GetEvent(eventName) as GameEventGroup;
            if(target != null)
            {
                target.EnableAllEvent(_enable);
            }
        }

        public static void Update()
        {
            if (RootGroup == null)
                return;
            RootGroup.Update();
        }
    }
}
