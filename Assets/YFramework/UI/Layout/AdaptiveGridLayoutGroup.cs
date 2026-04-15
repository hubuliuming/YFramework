using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI.Layout
{
    /// <summary>
    /// Grid-like layout group that can uniformly scale its children down to fit inside the parent rect.
    /// </summary>
    [AddComponentMenu("Layout/Adaptive Grid Layout Group")]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public sealed class AdaptiveGridLayoutGroup : LayoutGroup
    {
        private const float ScaleEpsilon = 0.0001f;

        private struct LayoutResult
        {
            public bool IsValid;
            public int ColumnCount;
            public int RowCount;
            public float Scale;
            public Vector2 ScaledCellSize;
            public Vector2 ScaledSpacing;
            public float RequiredWidth;
            public float RequiredHeight;
            public float TotalWidth;
            public float TotalHeight;
        }

        [SerializeField] private Vector2 m_CellSize = new Vector2(100f, 100f);
        [SerializeField] private Vector2 m_Spacing = Vector2.zero;
        [SerializeField] private GridLayoutGroup.Corner m_StartCorner = GridLayoutGroup.Corner.UpperLeft;
        [SerializeField] private GridLayoutGroup.Axis m_StartAxis = GridLayoutGroup.Axis.Horizontal;
        [SerializeField] private GridLayoutGroup.Constraint m_Constraint = GridLayoutGroup.Constraint.Flexible;
        [SerializeField] private int m_ConstraintCount = 2;

        [SerializeField] private bool m_AutoScaleToFit = true;
        [SerializeField] private float m_MinScale = 0.25f;
        [SerializeField] private float m_MaxScale = 1f;
        [SerializeField] private bool m_ScaleSpacing = true;

        private LayoutResult m_LayoutResult;

        public Vector2 cellSize
        {
            get { return m_CellSize; }
            set
            {
                Vector2 newValue = ClampVector2(value);
                if (m_CellSize == newValue)
                {
                    return;
                }

                m_CellSize = newValue;
                SetDirty();
            }
        }

        public Vector2 spacing
        {
            get { return m_Spacing; }
            set
            {
                Vector2 newValue = ClampVector2(value);
                if (m_Spacing == newValue)
                {
                    return;
                }

                m_Spacing = newValue;
                SetDirty();
            }
        }

        public GridLayoutGroup.Corner startCorner
        {
            get { return m_StartCorner; }
            set
            {
                if (m_StartCorner == value)
                {
                    return;
                }

                m_StartCorner = value;
                SetDirty();
            }
        }

        public GridLayoutGroup.Axis startAxis
        {
            get { return m_StartAxis; }
            set
            {
                if (m_StartAxis == value)
                {
                    return;
                }

                m_StartAxis = value;
                SetDirty();
            }
        }

        public GridLayoutGroup.Constraint constraint
        {
            get { return m_Constraint; }
            set
            {
                if (m_Constraint == value)
                {
                    return;
                }

                m_Constraint = value;
                SetDirty();
            }
        }

        public int constraintCount
        {
            get { return m_ConstraintCount; }
            set
            {
                int newValue = Mathf.Max(1, value);
                if (m_ConstraintCount == newValue)
                {
                    return;
                }

                m_ConstraintCount = newValue;
                SetDirty();
            }
        }

        public bool autoScaleToFit
        {
            get { return m_AutoScaleToFit; }
            set
            {
                if (m_AutoScaleToFit == value)
                {
                    return;
                }

                m_AutoScaleToFit = value;
                SetDirty();
            }
        }

        public float minScale
        {
            get { return m_MinScale; }
            set
            {
                float newValue = Mathf.Max(0.01f, value);
                if (Mathf.Approximately(m_MinScale, newValue))
                {
                    return;
                }

                m_MinScale = newValue;
                if (m_MaxScale < m_MinScale)
                {
                    m_MaxScale = m_MinScale;
                }

                SetDirty();
            }
        }

        public float maxScale
        {
            get { return m_MaxScale; }
            set
            {
                float newValue = Mathf.Max(m_MinScale, value);
                if (Mathf.Approximately(m_MaxScale, newValue))
                {
                    return;
                }

                m_MaxScale = newValue;
                SetDirty();
            }
        }

        public bool scaleSpacing
        {
            get { return m_ScaleSpacing; }
            set
            {
                if (m_ScaleSpacing == value)
                {
                    return;
                }

                m_ScaleSpacing = value;
                SetDirty();
            }
        }

        public void RefreshLayout()
        {
            if (!IsActive())
            {
                return;
            }

            SetDirty();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            m_LayoutResult = CalculateLayoutResult();
            SetLayoutInputForAxis(m_LayoutResult.TotalWidth, m_LayoutResult.TotalWidth, -1f, 0);
        }

        public override void CalculateLayoutInputVertical()
        {
            m_LayoutResult = CalculateLayoutResult();
            SetLayoutInputForAxis(m_LayoutResult.TotalHeight, m_LayoutResult.TotalHeight, -1f, 1);
        }

        public override void SetLayoutHorizontal()
        {
            ApplyLayout();
        }

        public override void SetLayoutVertical()
        {
            ApplyLayout();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RequestLayoutRefresh();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            ResetManagedChildScales();
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            RequestLayoutRefresh();
        }

        protected override void OnTransformChildrenChanged()
        {
            base.OnTransformChildrenChanged();
            RequestLayoutRefresh();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            NormalizeSerializedFields();
            RequestLayoutRefresh();
        }

        private void ApplyLayout()
        {
            if (!IsActive())
            {
                return;
            }

            m_LayoutResult = CalculateLayoutResult();
            m_Tracker.Clear();

            if (rectChildren.Count == 0 || !m_LayoutResult.IsValid)
            {
                return;
            }

            float startOffsetX = GetStartOffset(0, m_LayoutResult.RequiredWidth);
            float startOffsetY = GetStartOffset(1, m_LayoutResult.RequiredHeight);

            int cornerX = (int)m_StartCorner % 2;
            int cornerY = (int)m_StartCorner / 2;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                int positionX;
                int positionY;

                if (m_StartAxis == GridLayoutGroup.Axis.Horizontal)
                {
                    positionX = i % m_LayoutResult.ColumnCount;
                    positionY = i / m_LayoutResult.ColumnCount;
                }
                else
                {
                    positionX = i / m_LayoutResult.RowCount;
                    positionY = i % m_LayoutResult.RowCount;
                }

                if (cornerX == 1)
                {
                    positionX = m_LayoutResult.ColumnCount - 1 - positionX;
                }

                if (cornerY == 1)
                {
                    positionY = m_LayoutResult.RowCount - 1 - positionY;
                }

                float x = startOffsetX + (m_LayoutResult.ScaledCellSize.x + m_LayoutResult.ScaledSpacing.x) * positionX;
                float y = startOffsetY + (m_LayoutResult.ScaledCellSize.y + m_LayoutResult.ScaledSpacing.y) * positionY;
                ApplyChildLayout(child, x, y, m_CellSize, m_LayoutResult.Scale);
            }
        }

        private void ApplyChildLayout(RectTransform child, float x, float y, Vector2 size, float scale)
        {
            if (child == null)
            {
                return;
            }

            m_Tracker.Add(this, child, DrivenTransformProperties.Scale);
            child.localScale = new Vector3(scale, scale, scale);

            SetChildAlongAxisWithScale(child, 0, x, size.x, scale);
            SetChildAlongAxisWithScale(child, 1, y, size.y, scale);
        }

        private LayoutResult CalculateLayoutResult()
        {
            LayoutResult result = new LayoutResult();
            int childCount = rectChildren.Count;

            if (childCount == 0)
            {
                result.IsValid = false;
                result.Scale = Mathf.Clamp(1f, m_MinScale, m_MaxScale);
                result.TotalWidth = padding.horizontal;
                result.TotalHeight = padding.vertical;
                return result;
            }

            float availableWidth = Mathf.Max(0f, rectTransform.rect.width - padding.horizontal);
            float availableHeight = Mathf.Max(0f, rectTransform.rect.height - padding.vertical);

            switch (m_Constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                {
                    int columnCount = Mathf.Max(1, m_ConstraintCount);
                    int rowCount = Mathf.CeilToInt(childCount / (float)columnCount);
                    result = BuildLayoutResult(columnCount, rowCount, availableWidth, availableHeight);
                    break;
                }
                case GridLayoutGroup.Constraint.FixedRowCount:
                {
                    int rowCount = Mathf.Max(1, m_ConstraintCount);
                    int columnCount = Mathf.CeilToInt(childCount / (float)rowCount);
                    result = BuildLayoutResult(columnCount, rowCount, availableWidth, availableHeight);
                    break;
                }
                default:
                    result = BuildFlexibleLayoutResult(childCount, availableWidth, availableHeight);
                    break;
            }

            return result;
        }

        private LayoutResult BuildFlexibleLayoutResult(int childCount, float availableWidth, float availableHeight)
        {
            LayoutResult bestResult = new LayoutResult();

            if (m_StartAxis == GridLayoutGroup.Axis.Horizontal)
            {
                for (int columnCount = 1; columnCount <= childCount; columnCount++)
                {
                    int rowCount = Mathf.CeilToInt(childCount / (float)columnCount);
                    LayoutResult candidate = BuildLayoutResult(columnCount, rowCount, availableWidth, availableHeight);
                    if (IsBetterFlexibleResult(candidate, bestResult, availableWidth, availableHeight))
                    {
                        bestResult = candidate;
                    }
                }
            }
            else
            {
                for (int rowCount = 1; rowCount <= childCount; rowCount++)
                {
                    int columnCount = Mathf.CeilToInt(childCount / (float)rowCount);
                    LayoutResult candidate = BuildLayoutResult(columnCount, rowCount, availableWidth, availableHeight);
                    if (IsBetterFlexibleResult(candidate, bestResult, availableWidth, availableHeight))
                    {
                        bestResult = candidate;
                    }
                }
            }

            return bestResult;
        }

        private bool IsBetterFlexibleResult(LayoutResult candidate, LayoutResult currentBest, float availableWidth, float availableHeight)
        {
            if (!candidate.IsValid)
            {
                return false;
            }

            if (!currentBest.IsValid)
            {
                return true;
            }

            if (candidate.Scale > currentBest.Scale + 0.0001f)
            {
                return true;
            }

            if (candidate.Scale < currentBest.Scale - 0.0001f)
            {
                return false;
            }

            float candidateUnused = Mathf.Abs(availableWidth - candidate.RequiredWidth) + Mathf.Abs(availableHeight - candidate.RequiredHeight);
            float currentUnused = Mathf.Abs(availableWidth - currentBest.RequiredWidth) + Mathf.Abs(availableHeight - currentBest.RequiredHeight);
            if (candidateUnused < currentUnused - 0.0001f)
            {
                return true;
            }

            if (candidateUnused > currentUnused + 0.0001f)
            {
                return false;
            }

            if (m_StartAxis == GridLayoutGroup.Axis.Horizontal)
            {
                if (candidate.RowCount != currentBest.RowCount)
                {
                    return candidate.RowCount < currentBest.RowCount;
                }

                return candidate.ColumnCount > currentBest.ColumnCount;
            }

            if (candidate.ColumnCount != currentBest.ColumnCount)
            {
                return candidate.ColumnCount < currentBest.ColumnCount;
            }

            return candidate.RowCount > currentBest.RowCount;
        }

        private LayoutResult BuildLayoutResult(int columnCount, int rowCount, float availableWidth, float availableHeight)
        {
            LayoutResult result = new LayoutResult();
            columnCount = Mathf.Max(1, columnCount);
            rowCount = Mathf.Max(1, rowCount);

            float scale = Mathf.Clamp(1f, m_MinScale, m_MaxScale);
            if (m_AutoScaleToFit)
            {
                float widthScale = GetAxisFitScale(availableWidth, columnCount, m_CellSize.x, m_Spacing.x);
                float heightScale = GetAxisFitScale(availableHeight, rowCount, m_CellSize.y, m_Spacing.y);
                scale = Mathf.Min(widthScale, heightScale, m_MaxScale);
                scale = Mathf.Clamp(scale, m_MinScale, m_MaxScale);
            }

            float spacingScale = m_ScaleSpacing ? scale : 1f;
            Vector2 scaledCellSize = m_CellSize * scale;
            Vector2 scaledSpacing = m_Spacing * spacingScale;

            float requiredWidth = columnCount * scaledCellSize.x + Mathf.Max(0, columnCount - 1) * scaledSpacing.x;
            float requiredHeight = rowCount * scaledCellSize.y + Mathf.Max(0, rowCount - 1) * scaledSpacing.y;

            result.IsValid = true;
            result.ColumnCount = columnCount;
            result.RowCount = rowCount;
            result.Scale = scale;
            result.ScaledCellSize = scaledCellSize;
            result.ScaledSpacing = scaledSpacing;
            result.RequiredWidth = requiredWidth;
            result.RequiredHeight = requiredHeight;
            result.TotalWidth = requiredWidth + padding.horizontal;
            result.TotalHeight = requiredHeight + padding.vertical;
            return result;
        }

        private float GetAxisFitScale(float availableSize, int itemCount, float cellSize, float spacing)
        {
            if (itemCount <= 0)
            {
                return m_MaxScale;
            }

            float totalSpacing = Mathf.Max(0, itemCount - 1) * spacing;
            float totalCellSize = itemCount * cellSize;
            if (totalCellSize <= ScaleEpsilon)
            {
                return m_MaxScale;
            }

            float scaledSpace = m_ScaleSpacing ? totalSpacing : 0f;
            float fixedSpace = m_ScaleSpacing ? 0f : totalSpacing;
            return (availableSize - fixedSpace) / (totalCellSize + scaledSpace);
        }

        private void RequestLayoutRefresh()
        {
            SetDirty();

            if (!IsActive() || Application.isPlaying || CanvasUpdateRegistry.IsRebuildingLayout())
            {
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        private void ResetManagedChildScales()
        {
            if (rectTransform == null)
            {
                return;
            }

            for (int i = 0; i < rectTransform.childCount; i++)
            {
                RectTransform child = rectTransform.GetChild(i) as RectTransform;
                if (child == null)
                {
                    continue;
                }

                child.localScale = Vector3.one;
            }
        }

        private void NormalizeSerializedFields()
        {
            m_CellSize = ClampVector2(m_CellSize);
            m_Spacing = ClampVector2(m_Spacing);
            m_ConstraintCount = Mathf.Max(1, m_ConstraintCount);
            m_MinScale = Mathf.Max(0.01f, m_MinScale);
            m_MaxScale = Mathf.Max(m_MinScale, m_MaxScale);
        }

        private static Vector2 ClampVector2(Vector2 value)
        {
            return new Vector2(Mathf.Max(0f, value.x), Mathf.Max(0f, value.y));
        }
    }
}