namespace RPG.EventSystem
{
    /// <summary>
    /// 游戏事件基类
    /// </summary>
    public abstract class GameBaseEvent
    {
        #region 事件名称
        private string m_EventName;
        public string name
        {
            get { return m_EventName; }
            // 在构造函数中设置事件名称，名称作为事件的唯一标识
            protected set { m_EventName = value; }
        }
        #endregion

        #region 事件状态
        private bool m_Enable;
        public bool enable
        {
            get { return m_Enable; }
            set { m_Enable = value; }
        }
        #endregion

        /// <summary>
        /// 更新
        /// </summary>
        public virtual void Update() { }
    }
}
