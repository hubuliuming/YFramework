using UnityEngine;

namespace YFramework.Kit.Camera
{
    public class RotationCam : MonoBehaviour
    {
        public Transform myCam;

        private float x;
        private float y;

        private float touchSpeed = 0.2f;
        public float speed = 80;
        public float smoothTime = 3;
    
        bool canMouse = true;


        private void Update()
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    canMouse = false;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    canMouse = false;

                    x = Input.GetTouch(0).deltaPosition.x * touchSpeed;

                    y = Input.GetTouch(0).deltaPosition.y * touchSpeed;

                    if (x != 0 || y != 0)
                        RotateView(-x, -y);
                }
            }

            if (canMouse)
            {
                if (Input.GetMouseButton(0))
                {
                    x = Input.GetAxis("Mouse X");

                    y = Input.GetAxis("Mouse Y");

                    if (x != 0 || y != 0)
                        RotateView(x, y);
                }
            }

            Follow();
        }


        private void RotateView(float x, float y)
        {
            x *= speed * Time.deltaTime;
            //transform.Rotate(0, -x, 0, Space.World); 
            transform.Rotate(Vector3.up, -x, Space.World);

            y *= speed * Time.deltaTime;
            transform.Rotate(Vector3.right, y, Space.Self);

            //transform.Rotate(y, 0, 0);
        }

        public void Follow()
        {
            myCam.rotation = Quaternion.Slerp(myCam.rotation, transform.rotation, smoothTime * Time.deltaTime);
        }
    }
}