using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZPlugin
{
    [RequireComponent(typeof(RectTransform))]
    public class Resizable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float dragZoneSize = 3;
        [SerializeField] private Vector2 minSize;
        [SerializeField] private Vector2 maxSize;
        [SerializeField] private ResizeDirection resizeDirection;

        private bool isCursorEnter;
        private bool isResizing;
        private RectTransform rectTransform;
        private Camera uiCamera;

        private Vector2 lastMousePointInParent;
        private Vector2 relativeMousePoint;
        private Vector2 mousePointInParent;
        private Vector2 borderDistance = Vector2.zero;

        private int horizontalDirection;
        private int verticalDirection;

        private void Start()
        {
            if (minSize.x > maxSize.x || minSize.y > maxSize.y)
                throw new ArgumentException("Invalid Size");

            Canvas canvas = GetComponentInParent<Canvas>();
            uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isCursorEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isCursorEnter = false;
        }

        private void Update()
        {
            if (isCursorEnter || isResizing)
            {
                CalculateLocalPoint();
                if (isResizing)
                    DoResize();
                else if (IsMouseInDragZone())
                    CheckResize();
                lastMousePointInParent = mousePointInParent;
            }
        }

        private void CheckResize()
        {
            Rect rect = rectTransform.rect;
            horizontalDirection = GetDirection(relativeMousePoint.x, rect.width);
            verticalDirection = GetDirection(relativeMousePoint.y, rect.height);
            CursorDirection cursorDirection = GetCursorDirection();
            // TODO
            isResizing = Input.GetMouseButtonDown(0);
        }

        private void DoResize()
        {
            Rect rect = rectTransform.rect;
            Vector2 delta = mousePointInParent - lastMousePointInParent - borderDistance;
            Vector2 startDelta = delta;
            delta.x = horizontalDirection * Mathf.Max(minSize.x - rect.width, horizontalDirection * delta.x);
            delta.y = verticalDirection * Mathf.Max(minSize.y - rect.height, verticalDirection * delta.y);
            if (maxSize.magnitude > 0.1f)
            {
                delta.x = horizontalDirection * Mathf.Min(maxSize.x - rect.width, horizontalDirection * delta.x);
                delta.y = verticalDirection * Mathf.Min(maxSize.y - rect.height, verticalDirection * delta.y);
            }

            borderDistance = delta - startDelta;

            if (horizontalDirection > 0 &&
                (resizeDirection.right || resizeDirection.bottomRight || resizeDirection.topRight))
            {
                rectTransform.sizeDelta += new Vector2(delta.x, 0);
                rectTransform.anchoredPosition += new Vector2(delta.x * rectTransform.pivot.x, 0);
            }
            else if (horizontalDirection < 0 &&
                     (resizeDirection.left || resizeDirection.bottomLeft || resizeDirection.topLeft))
            {
                rectTransform.sizeDelta -= new Vector2(delta.x, 0);
                rectTransform.anchoredPosition += new Vector2(delta.x * (1 - rectTransform.pivot.x), 0);
            }

            if (verticalDirection > 0 && (resizeDirection.top || resizeDirection.topLeft || resizeDirection.topRight))
            {
                rectTransform.sizeDelta += new Vector2(0, delta.y);
                rectTransform.anchoredPosition += new Vector2(0, delta.y * rectTransform.pivot.y);
            }
            else if (verticalDirection < 0 &&
                     (resizeDirection.bottom || resizeDirection.bottomLeft || resizeDirection.bottomRight))
            {
                rectTransform.sizeDelta -= new Vector2(0, delta.y);
                rectTransform.anchoredPosition += new Vector2(0, delta.y * (1 - rectTransform.pivot.y));
            }

            if (Input.GetMouseButtonUp(0))
            {
                isResizing = false;
                borderDistance = Vector2.zero;
            }
        }

        private void CalculateLocalPoint()
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                mousePosition, uiCamera, out Vector2 localPoint);
            relativeMousePoint = localPoint + rectTransform.rect.size * rectTransform.pivot;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform,
                mousePosition, uiCamera, out mousePointInParent);
        }

        private int GetDirection(float position, float size)
        {
            if (position < dragZoneSize)
                return -1;
            if (position > size - dragZoneSize)
                return 1;

            return 0;
        }

        private CursorDirection GetCursorDirection()
        {
            if (horizontalDirection < 0 && verticalDirection == 0)
                return CursorDirection.Left;

            if (horizontalDirection > 0 && verticalDirection == 0)
                return CursorDirection.Right;

            if (verticalDirection < 0 && horizontalDirection == 0)
                return CursorDirection.Bottom;

            if (verticalDirection > 0 && horizontalDirection == 0)
                return CursorDirection.Top;

            if (horizontalDirection < 0 && verticalDirection < 0)
                return CursorDirection.BottomLeft;

            if (horizontalDirection > 0 && verticalDirection > 0)
                return CursorDirection.TopRight;

            if (horizontalDirection < 0 && verticalDirection > 0)
                return CursorDirection.TopLeft;

            if (horizontalDirection > 0 && verticalDirection < 0)
                return CursorDirection.BottomRight;

            return CursorDirection.None;
        }

        private bool IsMouseInDragZone()
        {
            Rect rect = rectTransform.rect;
            float x = relativeMousePoint.x;
            float y = relativeMousePoint.y;
            return x >= 0 && y >= 0 && x <= rect.width && y <= rect.height
                   && (x < dragZoneSize || y < dragZoneSize || x > rect.width - dragZoneSize ||
                       y > rect.height - dragZoneSize);
        }

        [Serializable]
        private struct ResizeDirection
        {
            public bool top;
            public bool bottom;
            public bool left;
            public bool right;
            public bool topLeft;
            public bool topRight;
            public bool bottomLeft;
            public bool bottomRight;
        }

        private enum CursorDirection
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
    }
}