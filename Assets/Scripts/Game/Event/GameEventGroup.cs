using System.Collections.Generic;

namespace RPG.EventSystem
{
    /// <summary>
    /// 游戏事件组
    /// </summary>
    public class GameEventGroup : GameBaseEvent
    {
        /// <summary>
        /// 该事件组管理的子事件/子事件组
        /// </summary>
        protected List<GameBaseEvent> m_Events;

        public GameEventGroup(string eventName)
        {
            // 设置事件名称
            name = eventName;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="gameEvent"></param>
        public void AddEvent(GameBaseEvent gameEvent)
        {
            if (m_Events == null)
                m_Events = new List<GameBaseEvent>();
            if (m_Events.Find(e => e.name == gameEvent.name) != null)
                return;
            m_Events.Add(gameEvent);
        }

        /// <summary>
        /// 获取事件对象
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public GameBaseEvent GetEvent(string eventName)
        {
            // 开辟队列内存
            Queue<GameBaseEvent> q = new Queue<GameBaseEvent>();
            // BFS查找
            q.Enqueue(this);
            while(q.Count > 0)
            {
                GameEventGroup temp = q.Dequeue() as GameEventGroup;
                if(temp != null && temp.m_Events != null && temp.m_Events.Count > 0)
                {
                    var children = temp.m_Events;
                    foreach (GameBaseEvent eventItem in children)
                    {
                        if (eventItem.name == eventName)
                        {
                            return eventItem;
                        }
                        q.Enqueue(eventItem);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 启用所有事件
        /// </summary>
        /// <param name="_enable"></param>
        public void EnableAllEvent(bool _enable)
        {
            enable = _enable;
            if (m_Events == null) return;
            foreach (GameBaseEvent eventItem in m_Events)
            {
                if(eventItem is GameEventGroup)
                {
                    // 如果是事件组，则启用其管理的所有事件
                    (eventItem as GameEventGroup).EnableAllEvent(_enable);
                }
                else
                {
                    // 如果是单个事件，则启用自身
                    eventItem.enable = _enable;
                }
            }
        }

        /// <summary>
        /// 移除某个事件
        /// </summary>
        /// <param name="eventName"></param>
        public void RemoveEvent(string eventName)
        {
            if (m_Events == null) return;
            foreach (GameBaseEvent eventItem in m_Events)
            {
                // 如果该层有为名eventName的事件，则移除
                if(eventItem.name == eventName)
                {
                    m_Events.Remove(eventItem);
                    return;
                }
                else
                {
                    // 递归查找
                    if(eventItem is GameEventGroup)
                    {
                        (eventItem as GameEventGroup).RemoveEvent(eventName);
                    }
                }
            }
        }

        /// <summary>
        /// 更新管理的所有事件
        /// </summary>
        public override void Update()
        {
            if (!enable || m_Events == null) return;
            foreach (GameBaseEvent eventItem in m_Events)
            {
                if(eventItem != null)
                    eventItem.Update();
            }
        }
    }
}
