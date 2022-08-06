using UnityEngine;

namespace ZPlugin {
    [RequireComponent(typeof(Camera))]
    public class ThirdPersonCamera : MonoBehaviour {
        public Transform characterTarget;
        public float pivotRotateSpeed = 60;
        public Vector3 pivotOffset = new Vector3(0, 2, 0);
        public Vector3 targetOffset = new Vector3(0, 0.2f, -3);
        public float maxAngleX = 60;
        public float minAngleX = -60;
        public bool lockCursor = true;

        Transform cameraPivot; //摄像机面朝的点,并且这个点随着目标移动
        Transform cameraTarget; //摄像机最终的位移坐标
        private float scrollWheel;
        private float mouseX;
        private float mouseY;

        private void Awake() {
            cameraPivot = new GameObject() {
                hideFlags = HideFlags.HideAndDontSave
            }.transform;

            cameraTarget = new GameObject().transform;
            cameraTarget.SetParent(cameraPivot);
        }

        void LateUpdate() {
            CursorUtil.SetLockCursor(lockCursor);

            if (characterTarget == null)
                return;

            ProcessInput();

            UpdateTarget();
            UpdatePivot();

            SyncCamera();
        }

        private void ProcessInput() {
            scrollWheel = -Input.GetAxis("Mouse ScrollWheel");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        private void UpdatePivot() {
            cameraPivot.position = characterTarget.position + pivotOffset;

            Quaternion rotateBefor = cameraPivot.rotation;
            cameraPivot.Rotate(-mouseY * pivotRotateSpeed * Time.deltaTime, 0, 0);

            //X = 90~0,360~270 
            //Check X no legal back to before
            Vector3 angle = cameraPivot.eulerAngles;
            if ((angle.x > maxAngleX && angle.x < 180) || (angle.x < 360 + minAngleX && angle.x > 180)) {
                cameraPivot.rotation = rotateBefor;
            }

            cameraPivot.Rotate(0, mouseX * pivotRotateSpeed * Time.deltaTime, 0);
            cameraPivot.eulerAngles = Vector3.Scale(cameraPivot.rotation.eulerAngles, new Vector3(1, 1, 0));
        }

        private void UpdateTarget() {
            targetOffset += targetOffset * scrollWheel;
            cameraTarget.localPosition = targetOffset;
        }

        private void SyncCamera() {
            transform.position = cameraTarget.position;
            transform.rotation = cameraTarget.rotation;
        }
    }
}