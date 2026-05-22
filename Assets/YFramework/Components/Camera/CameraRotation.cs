using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Camera targetCamera;
    public float rotationSpeed = 15f; // 旋转速度
    public RectTransform controlArea; // 指定控制区域

    private Vector2 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // 检查鼠标是否在指定区域内
        if (RectTransformUtility.RectangleContainsScreenPoint(controlArea, Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0)) // 鼠标左键按下
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }

            if (Input.GetMouseButton(0) && isDragging) // 鼠标左键按住并拖动
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
                RotateCamera(delta);
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0)) // 鼠标左键释放
            {
                isDragging = false;
            }
        }
        else
        {
            // 如果鼠标移出区域，停止拖动
            if (isDragging)
            {
                isDragging = false;
            }
        }
    }

    void RotateCamera(Vector2 delta)
    {
        // 根据鼠标移动量旋转相机
        float rotationX = delta.y * rotationSpeed * Time.deltaTime;
        float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

        targetCamera.transform.Rotate(rotationX, rotationY, 0);
    }
}