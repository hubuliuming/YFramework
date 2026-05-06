/****************************************************
    文件：TMPTextRollEffect.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：TMP文字滚动切换效果
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YFramework.UI
{
    public enum TMPTextRollDirection
    {
        Up,
        Down,
    }

    public static class TMPTextRollEffect
    {
        private const string RunnerName = "__TMPTextRollEffectRunner";

        private static TMPTextRollEffectRunner s_runner;

        public static Handle Play(
            TextMeshProUGUI target,
            string newText,
            TMPTextRollDirection direction,
            float distance = 18f,
            float duration = 0.22f)
        {
            return PlayChangedPart(target, target != null ? target.text : string.Empty, newText, direction, null, distance, duration);
        }

        public static Handle PlayChangedPart(
            TextMeshProUGUI target,
            string oldText,
            string newText,
            TMPTextRollDirection direction,
            string delimiter,
            float distance = 18f,
            float duration = 0.22f)
        {
            if (target == null)
            {
                return null;
            }

            RectTransform targetRect = target.rectTransform;
            if (targetRect == null || targetRect.parent == null || duration <= 0f || distance <= 0f)
            {
                target.text = newText ?? string.Empty;
                return null;
            }

            oldText = oldText ?? string.Empty;
            newText = newText ?? string.Empty;
            if (oldText == newText)
            {
                target.text = newText;
                return null;
            }

            TextSections sections = BuildTextSections(oldText, newText, delimiter);
            if (sections.OldRollingText == sections.NewRollingText)
            {
                target.text = newText;
                return null;
            }

            EnsureRunner();

            Vector2 originPosition = targetRect.anchoredPosition;
            float originAlpha = target.alpha;
            SegmentViews segmentViews = CreateSegmentViews(target, sections, originPosition);
            if (segmentViews == null || segmentViews.RollingOldText == null || segmentViews.RollingNewText == null)
            {
                segmentViews?.DestroyAll();
                target.text = newText;
                target.alpha = originAlpha;
                return null;
            }

            float sign = direction == TMPTextRollDirection.Up ? 1f : -1f;
            target.alpha = 0f;

            RectTransform newRollingRect = segmentViews.RollingNewText.rectTransform;
            newRollingRect.anchoredPosition = segmentViews.RollingPosition - Vector2.up * distance * sign;
            segmentViews.RollingNewText.alpha = 0f;

            Handle handle = new Handle(target, segmentViews, newText, originAlpha);
            handle.Coroutine = s_runner.StartCoroutine(PlayRoutine(handle, sign, distance, duration));
            return handle;
        }

        public static Handle PlayNumberChangedPart(
            TextMeshProUGUI target,
            int oldValue,
            int newValue,
            string suffixText,
            float duration = 0.3f)
        {
            if (target == null)
            {
                return null;
            }

            string finalText = newValue.ToString() + (suffixText ?? string.Empty);
            RectTransform targetRect = target.rectTransform;
            if (targetRect == null || targetRect.parent == null || duration <= 0f || oldValue == newValue)
            {
                target.text = finalText;
                return null;
            }

            EnsureRunner();

            Vector2 originPosition = targetRect.anchoredPosition;
            float originAlpha = target.alpha;
            SegmentViews segmentViews = CreateNumberViews(target, oldValue, newValue, suffixText, originPosition);
            if (segmentViews == null || segmentViews.RollingNewText == null)
            {
                segmentViews?.DestroyAll();
                target.text = finalText;
                target.alpha = originAlpha;
                return null;
            }

            target.alpha = 0f;

            Handle handle = new Handle(target, segmentViews, finalText, originAlpha);
            handle.Coroutine = s_runner.StartCoroutine(PlayNumberRoutine(handle, oldValue, newValue, duration));
            return handle;
        }

        private static IEnumerator PlayNumberRoutine(Handle handle, int oldValue, int newValue, float duration)
        {
            TextMeshProUGUI target = handle.Target;
            TextMeshProUGUI numberTextView = handle.SegmentViews.RollingNewText;

            float elapsed = 0f;
            int lastValue = oldValue;
            if (numberTextView != null)
            {
                numberTextView.text = oldValue.ToString();
            }

            while (elapsed < duration)
            {
                if (target == null)
                {
                    handle.Stop(false);
                    yield break;
                }

                elapsed += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(oldValue, newValue, EaseOutCubic(progress)));
                if (currentValue != lastValue && numberTextView != null)
                {
                    numberTextView.text = currentValue.ToString();
                    lastValue = currentValue;
                }

                yield return null;
            }

            if (numberTextView != null)
            {
                numberTextView.text = newValue.ToString();
            }

            handle.Complete();
        }

        private static IEnumerator PlayRoutine(Handle handle, float sign, float distance, float duration)
        {
            TextMeshProUGUI target = handle.Target;
            TextMeshProUGUI oldTextView = handle.SegmentViews.RollingOldText;
            TextMeshProUGUI newTextView = handle.SegmentViews.RollingNewText;
            RectTransform oldTextRect = oldTextView != null ? oldTextView.rectTransform : null;
            RectTransform newTextRect = newTextView != null ? newTextView.rectTransform : null;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (target == null)
                {
                    handle.Stop(false);
                    yield break;
                }

                elapsed += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                float eased = EaseOutCubic(progress);

                if (newTextView != null && newTextRect != null)
                {
                    newTextRect.anchoredPosition = Vector2.Lerp(
                        handle.SegmentViews.RollingPosition - Vector2.up * distance * sign,
                        handle.SegmentViews.RollingPosition,
                        eased);
                    newTextView.alpha = Mathf.Lerp(0f, handle.OriginAlpha, eased);
                }

                if (oldTextView != null && oldTextRect != null)
                {
                    oldTextRect.anchoredPosition = Vector2.Lerp(
                        handle.SegmentViews.RollingPosition,
                        handle.SegmentViews.RollingPosition + Vector2.up * distance * sign,
                        eased);
                    oldTextView.alpha = Mathf.Lerp(handle.OriginAlpha, 0f, eased);
                }

                yield return null;
            }

            handle.Complete();
        }

        private static SegmentViews CreateSegmentViews(TextMeshProUGUI target, TextSections sections, Vector2 originPosition)
        {
            RectTransform targetRect = target.rectTransform;
            float height = Mathf.Max(targetRect.rect.height, target.GetPreferredValues(target.text).y);
            float prefixWidth = ResolveTextWidth(target, sections.PrefixText);
            float oldRollingWidth = ResolveTextWidth(target, sections.OldRollingText);
            float newRollingWidth = ResolveTextWidth(target, sections.NewRollingText);
            float rollingWidth = Mathf.Max(oldRollingWidth, newRollingWidth);
            float suffixWidth = ResolveTextWidth(target, sections.SuffixText);
            float totalWidth = prefixWidth + rollingWidth + suffixWidth;
            float startX = ResolveStartX(target, originPosition, totalWidth);
            int siblingIndex = targetRect.GetSiblingIndex();

            SegmentViews views = new SegmentViews();
            float cursorX = startX;
            if (!string.IsNullOrEmpty(sections.PrefixText))
            {
                views.AddStatic(CreateSegment(target, sections.PrefixText, cursorX, prefixWidth, height, originPosition.y, siblingIndex, target.alpha));
                cursorX += prefixWidth;
            }

            Vector2 rollingPosition = new Vector2(cursorX + rollingWidth * 0.5f, originPosition.y);
            views.RollingPosition = rollingPosition;
            views.RollingOldText = CreateSegment(target, sections.OldRollingText, cursorX, rollingWidth, height, originPosition.y, siblingIndex, target.alpha);
            views.RollingNewText = CreateSegment(target, sections.NewRollingText, cursorX, rollingWidth, height, originPosition.y, siblingIndex, target.alpha);
            views.AddStatic(views.RollingOldText);
            views.AddStatic(views.RollingNewText);
            cursorX += rollingWidth;

            if (!string.IsNullOrEmpty(sections.SuffixText))
            {
                views.AddStatic(CreateSegment(target, sections.SuffixText, cursorX, suffixWidth, height, originPosition.y, siblingIndex, target.alpha));
            }

            return views;
        }

        private static SegmentViews CreateNumberViews(
            TextMeshProUGUI target,
            int oldValue,
            int newValue,
            string suffixText,
            Vector2 originPosition)
        {
            RectTransform targetRect = target.rectTransform;
            float height = Mathf.Max(targetRect.rect.height, target.GetPreferredValues(target.text).y);
            string oldNumberText = oldValue.ToString();
            string newNumberText = newValue.ToString();
            suffixText = suffixText ?? string.Empty;
            float numberWidth = Mathf.Max(ResolveTextWidth(target, oldNumberText), ResolveTextWidth(target, newNumberText));
            float suffixWidth = ResolveTextWidth(target, suffixText);
            float totalWidth = numberWidth + suffixWidth;
            float startX = ResolveStartX(target, originPosition, totalWidth);
            int siblingIndex = targetRect.GetSiblingIndex();

            SegmentViews views = new SegmentViews();
            views.RollingPosition = new Vector2(startX + numberWidth * 0.5f, originPosition.y);
            views.RollingNewText = CreateSegment(
                target,
                oldNumberText,
                startX,
                numberWidth,
                height,
                originPosition.y,
                siblingIndex,
                target.alpha);
            views.AddStatic(views.RollingNewText);

            if (!string.IsNullOrEmpty(suffixText))
            {
                views.AddStatic(CreateSegment(
                    target,
                    suffixText,
                    startX + numberWidth,
                    suffixWidth,
                    height,
                    originPosition.y,
                    siblingIndex,
                    target.alpha));
            }

            return views;
        }

        private static TextMeshProUGUI CreateSegment(
            TextMeshProUGUI source,
            string text,
            float startX,
            float width,
            float height,
            float y,
            int siblingIndex,
            float alpha)
        {
            TextMeshProUGUI segment = Object.Instantiate(source, source.rectTransform.parent);
            segment.name = source.name + "_RollPart";
            segment.text = text ?? string.Empty;
            segment.raycastTarget = false;
            segment.alpha = alpha;
            segment.alignment = TextAlignmentOptions.Center;
            segment.enableWordWrapping = false;
            segment.overflowMode = TextOverflowModes.Overflow;
            segment.transform.SetSiblingIndex(siblingIndex);

            RectTransform segmentRect = segment.rectTransform;
            RectTransform sourceRect = source.rectTransform;
            segmentRect.anchorMin = sourceRect.anchorMin;
            segmentRect.anchorMax = sourceRect.anchorMax;
            segmentRect.pivot = new Vector2(0.5f, sourceRect.pivot.y);
            segmentRect.sizeDelta = new Vector2(Mathf.Max(0f, width), height);
            segmentRect.anchoredPosition = new Vector2(startX + width * 0.5f, y);
            segmentRect.localScale = sourceRect.localScale;
            segmentRect.localRotation = sourceRect.localRotation;
            return segment;
        }

        private static float ResolveTextWidth(TextMeshProUGUI target, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0f;
            }

            return Mathf.Max(0f, target.GetPreferredValues(text).x);
        }

        private static float ResolveStartX(TextMeshProUGUI target, Vector2 originPosition, float totalWidth)
        {
            TextAlignmentOptions alignment = target.alignment;
            string alignmentText = alignment.ToString();
            Rect rect = target.rectTransform.rect;
            if (alignmentText.Contains("Left"))
            {
                return originPosition.x + rect.xMin;
            }

            if (alignmentText.Contains("Right"))
            {
                return originPosition.x + rect.xMax - totalWidth;
            }

            return originPosition.x - totalWidth * 0.5f;
        }

        private static TextSections BuildTextSections(string oldText, string newText, string delimiter)
        {
            if (!string.IsNullOrEmpty(delimiter))
            {
                int oldDelimiterIndex = oldText.IndexOf(delimiter);
                int newDelimiterIndex = newText.IndexOf(delimiter);
                if (oldDelimiterIndex >= 0 && newDelimiterIndex >= 0)
                {
                    return new TextSections(
                        string.Empty,
                        oldText.Substring(0, oldDelimiterIndex),
                        newText.Substring(0, newDelimiterIndex),
                        newText.Substring(newDelimiterIndex));
                }
            }

            int prefixLength = ResolveCommonPrefixLength(oldText, newText);
            int suffixLength = ResolveCommonSuffixLength(oldText, newText, prefixLength);
            return new TextSections(
                newText.Substring(0, prefixLength),
                oldText.Substring(prefixLength, oldText.Length - prefixLength - suffixLength),
                newText.Substring(prefixLength, newText.Length - prefixLength - suffixLength),
                newText.Substring(newText.Length - suffixLength));
        }

        private static int ResolveCommonPrefixLength(string oldText, string newText)
        {
            int maxLength = Mathf.Min(oldText.Length, newText.Length);
            int length = 0;
            while (length < maxLength && oldText[length] == newText[length])
            {
                length++;
            }

            return length;
        }

        private static int ResolveCommonSuffixLength(string oldText, string newText, int prefixLength)
        {
            int oldIndex = oldText.Length - 1;
            int newIndex = newText.Length - 1;
            int length = 0;
            while (oldIndex >= prefixLength && newIndex >= prefixLength && oldText[oldIndex] == newText[newIndex])
            {
                length++;
                oldIndex--;
                newIndex--;
            }

            return length;
        }

        private static float EaseOutCubic(float value)
        {
            float inverse = 1f - Mathf.Clamp01(value);
            return 1f - inverse * inverse * inverse;
        }

        private static void EnsureRunner()
        {
            if (s_runner != null)
            {
                return;
            }

            GameObject runnerObject = new GameObject(RunnerName);
            Object.DontDestroyOnLoad(runnerObject);
            runnerObject.hideFlags = HideFlags.HideAndDontSave;
            s_runner = runnerObject.AddComponent<TMPTextRollEffectRunner>();
        }

        public sealed class Handle
        {
            internal Handle(TextMeshProUGUI target, SegmentViews segmentViews, string finalText, float originAlpha)
            {
                Target = target;
                SegmentViews = segmentViews;
                FinalText = finalText ?? string.Empty;
                OriginAlpha = originAlpha;
            }

            internal TextMeshProUGUI Target { get; }
            internal SegmentViews SegmentViews { get; }
            internal string FinalText { get; }
            internal float OriginAlpha { get; }
            internal Coroutine Coroutine { get; set; }

            public void Stop(bool complete = false)
            {
                if (Coroutine != null && s_runner != null)
                {
                    s_runner.StopCoroutine(Coroutine);
                    Coroutine = null;
                }

                if (complete)
                {
                    Complete();
                    return;
                }

                Complete();
            }

            internal void Complete()
            {
                Coroutine = null;
                if (Target != null)
                {
                    Target.text = FinalText;
                    Target.alpha = OriginAlpha;
                }

                Cleanup();
            }

            private void Cleanup()
            {
                SegmentViews?.DestroyAll();
            }
        }

        internal sealed class SegmentViews
        {
            private readonly List<TextMeshProUGUI> m_views = new List<TextMeshProUGUI>();

            public TextMeshProUGUI RollingOldText { get; set; }
            public TextMeshProUGUI RollingNewText { get; set; }
            public Vector2 RollingPosition { get; set; }

            public void AddStatic(TextMeshProUGUI view)
            {
                if (view != null)
                {
                    m_views.Add(view);
                }
            }

            public void DestroyAll()
            {
                for (int i = 0; i < m_views.Count; i++)
                {
                    TextMeshProUGUI view = m_views[i];
                    if (view != null)
                    {
                        Object.Destroy(view.gameObject);
                    }
                }

                m_views.Clear();
                RollingOldText = null;
                RollingNewText = null;
            }
        }

        private readonly struct TextSections
        {
            public TextSections(string prefixText, string oldRollingText, string newRollingText, string suffixText)
            {
                PrefixText = prefixText ?? string.Empty;
                OldRollingText = oldRollingText ?? string.Empty;
                NewRollingText = newRollingText ?? string.Empty;
                SuffixText = suffixText ?? string.Empty;
            }

            public string PrefixText { get; }
            public string OldRollingText { get; }
            public string NewRollingText { get; }
            public string SuffixText { get; }
        }
    }

    internal sealed class TMPTextRollEffectRunner : MonoBehaviour
    {
    }
}
