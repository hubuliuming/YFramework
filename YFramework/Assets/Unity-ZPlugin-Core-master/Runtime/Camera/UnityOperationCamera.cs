using UnityEngine;

namespace ZPlugin {
    /// <summary>
    /// 仿制 Unity 摄像机操作的自由摄像机。
    /// TODO 可以将 Update 的相机Transform 位移调整到 FixedUpdate
    /// 实现 Update 之计算位置, 物理位移在 FixedUpdate
    /// </summary>
    public class UnityOperationCamera : MonoBehaviour {
        void Update() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float ms = +Input.GetAxis("Mouse ScrollWheel");

            float m_x = Input.GetAxis("Mouse X");
            float m_y = Input.GetAxis("Mouse Y");

            float q = Input.GetKey(KeyCode.E) ? -1 : 0;
            q += Input.GetKey(KeyCode.Q) ? 1 : 0;

            float z = Input.GetKey(KeyCode.C) ? -1 : 0;
            z += Input.GetKey(KeyCode.Z) ? 1 : 0;

            if (Input.GetMouseButton(1)) {
                transform.Rotate(Vector3.up * m_x * 3, Space.World);
                transform.Rotate(Vector3.left * m_y * 3);
            }

            if (Input.GetMouseButton(2)) {
                transform.Translate(Vector3.left * m_x * 5 + Vector3.down * m_y * 5);
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }


            Vector3 forward_w = transform.TransformDirection(Vector3.forward);
            forward_w.y = 0;
            forward_w = Vector3.Normalize(forward_w);
            Vector3 right_w = transform.TransformDirection(Vector3.right);
            right_w.y = 0;
            right_w = Vector3.Normalize(right_w);

            transform.Translate(forward_w * v + Vector3.up * q + right_w * h, Space.World);
            transform.Translate(Vector3.forward * ms * 100);
            transform.Rotate(Vector3.forward * z);
        }
    }
}