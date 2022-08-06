using UnityEngine;

namespace ZPlugin {
    public static class CursorUtil {
        public static void SetLockCursor(bool lockCursor) {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    }
}