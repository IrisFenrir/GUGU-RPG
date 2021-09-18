namespace RPG.EventSystem
{
    public class GameEvent : GameBaseEvent
    {
        /// <summary>
        /// 事件响应委托
        /// </summary>
        /// <param name="para"></param>
        public delegate void ResponseHandle(params object[] args);
        protected ResponseHandle m_Response;

        public delegate bool CheckHandle(out object[] args);
        protected CheckHandle m_CheckHandle;

        protected object[] m_Args;

        public GameEvent(string eventName)
        {
            // 设置事件名称
            name = eventName;
        }

        public override void Update()
        {
            if (!enable)
            {
                m_Args = null;
                return;
            }
            if(m_CheckHandle != null && m_CheckHandle(out m_Args))
            {
                if(m_Response != null)
                    m_Response(m_Args);
            }
        }

        public void AddCheckHandle(CheckHandle check)
        {
            m_CheckHandle += check;
        }
        public void RemoveCheckHandle(CheckHandle check)
        {
            m_CheckHandle -= check;
        }
        public void AddResponse(ResponseHandle response)
        {
            m_Response += response;
        }
        public void RemoveResponse(ResponseHandle response)
        {
            m_Response -= response;
        }
    }
}
