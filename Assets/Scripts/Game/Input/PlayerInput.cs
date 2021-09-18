using RPG.EventSystem;

namespace RPG.InputSystem
{
    public class PlayerInput
    {
        public float hor;
        public float ver;

        public PlayerInput()
        {
            GameEventManager.RegisterEvent("PlayerMove", InputPlayerMove);
            GameEventManager.RegisterEvent("PlayerDash", InputPlayerDash);
            GameEventManager.RegisterEvent("LockCamera", InputLockCamera);
            GameEventManager.RegisterEvent("CameraRotate", InputCameraRotate);
        }

        private bool InputPlayerMove(out object[] args)
        {
            float hor = InputManager.GetAxis("Horizontal");
            float ver = InputManager.GetAxis("Vertical");
            args = new object[] { hor, ver };
            return true;
        }

        private bool InputPlayerDash(out object[] args)
        {
            args = null;
            return InputManager.GetKeyDown("Dash");
        }

        private bool InputLockCamera(out object[] args)
        {
            args = null;
            return InputManager.GetKeyDown("LockCamera");
        }

        private bool InputCameraRotate(out object[] args)
        {
            float hor = InputManager.GetAxis("CameraHorizontal");
            float ver = InputManager.GetAxis("CameraVertical");
            args = new object[] { hor, ver };
            return hor != 0 || ver != 0;
        }
    }
}
