using UnityEngine;
using RPG.MotionSystem;
using RPG.EventSystem;
using RPG.InputSystem;
using RPG.AnimationSystem;
using RPG.CameraSystem;
using RPG.UISystem;
using RPG.BackageSystem;
using RPG.PropertySystem;

namespace RPG.Global
{
    /// <summary>
    /// 游戏主循环
    /// </summary>
    public class GameLoop : MonoBehaviour
    {
        [Header("Animation System")]
        public int idleStateCount = 4;  // 待机状态数量
        public float idleMinWaitTime = 0f; // 待机切换动画最短时间
        public float idleMaxWaitTime = 5f;  // 待机切换动画最长时间
        [Header("Input System")]
        public InputData inputData = new InputData();  // 键位数据
        [Header("Motion System")]
        public float walkSpeed;  // 行走速度
        public float rotateSpeed;  // 旋转速度
        public float runSpeedMultiply;
        public float normalSpeedMultiply;
        public float runAnimMultiply;
        public float normalAnimMultiply;
        [Header("Camera System")]
        public float cameraMoveSmoothTime;  // 相机移动平滑时间
        public float cameraRotateSpeed;  // 相机旋转速度
        [Header("Backage System")]
        public Vector3 detectorCenter = new Vector3(0f, 1f, 0.91f);
        public Vector3 detectorSize = new Vector3(0.48f, 0.91f, 0.88f);
        public string targetLayer = "Prop";
        public int page;
        public int row;
        public int column;
        public Vector3 backageItemCenter;
        public Vector2 backageItemInterval;

        // 单例，GameLoop同时提供全局参数，需要的模块可以直接调取
        public static GameLoop Instance;

        // 模块
        private IdleChanger idleChanger;
        private InputManager m_InputManager;
        private MotionManager m_MotionManager;
        private CameraManager m_CameraManager;
        private UIManager m_UIManager;
        private BackageManager m_BackageManager;
        // 玩家
        private GameObject m_Player;
        private PlayerInput m_PlayerInput;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            // 获取场景中的玩家
            m_Player = HierarchyHelper.FindGameObject("Player");
            // 设置待机动画切换
            idleChanger = m_Player.GetComponent<Animator>().GetBehaviour<IdleChanger>();
            idleChanger.stateCount = idleStateCount;
            idleChanger.minWaitTime = idleMinWaitTime;
            idleChanger.maxWaitTime = idleMaxWaitTime;
            // 启动各模块
            

            m_InputManager = new InputManager(inputData);
            m_MotionManager = new MotionManager();
            //m_CharacterAnimation = new CharacterAnimation();
            m_CameraManager = new CameraManager(m_Player.transform, cameraMoveSmoothTime, cameraRotateSpeed);
            m_UIManager = new UIManager();
            PropertyFactory.ImportData();
            m_BackageManager = new BackageManager(detectorCenter, detectorSize, m_Player, targetLayer,
                page, row, column, backageItemCenter, backageItemInterval);

            m_PlayerInput = new PlayerInput();

            
        }

        private void Update()
        {
            // 更新各模块
            GameTimer.Update();
            m_InputManager.Update();
            
            m_MotionManager.Update();
            m_CameraManager.Update();
            m_BackageManager.Update();

            GameEventManager.Update();
        }

        private void FixedUpdate()
        {
            
        }
    }
}
