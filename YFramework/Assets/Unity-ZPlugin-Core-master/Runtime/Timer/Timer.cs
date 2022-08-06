using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZPlugin {
    public class EmptyBehaviour : MonoBehaviour {
        public static EmptyBehaviour Craete() {
            GameObject go = new GameObject("Empty") {hideFlags = HideFlags.HideAndDontSave};
            EmptyBehaviour emptyBehaviour = go.AddComponent<EmptyBehaviour>();
            return emptyBehaviour;
        }
    }

    /// <summary>
    /// 计时器及延时执行
    /// </summary>
    public class Timer {
        public EmptyBehaviour EmptyBehaviour;
        public bool Running = true;
        public float Time;

        public Timer() {
            GameObject timerGO = new GameObject("Timer") {hideFlags = HideFlags.HideAndDontSave};
            EmptyBehaviour = timerGO.AddComponent<EmptyBehaviour>();
            EmptyBehaviour.StartCoroutine(UpdateTimerCoro());
        }

        public static implicit operator float(Timer timer) {
            return timer.Time;
        }

        public override string ToString() {
            return ((float) this).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Reset() => Time = 0;

        private IEnumerator UpdateTimerCoro() {
            while (true) {
                if (Running) {
                    Time += UnityEngine.Time.deltaTime;
                }

                yield return null;
            }
        }
    }

    public static class TimerUtil {
        /// <summary>
        /// 延时执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void InvokeDelay(Action action, float delay) {
            EmptyBehaviour empty = EmptyBehaviour.Craete();
            empty.StartCoroutine(InvokeDelayCoro(action, delay, empty));
        }

        private static IEnumerator InvokeDelayCoro(Action action, float delay, Component comp) {
            yield return new WaitForSeconds(delay);
            action();
            Object.Destroy(comp.gameObject);
        }

        /// <summary>
        /// 时间插值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="action"></param>
        public static void InverseLerp(float time, Action<float> action) {
            MonoBehaviour mono = EmptyBehaviour.Craete();
            mono.StartCoroutine(ToCoro(time, action, mono));
        }

        private static IEnumerator ToCoro(float time, Action<float> action, Component comp) {
            float timer = 0;
            while (timer < time) {
                action(timer / time);
                yield return null;
                timer += Time.deltaTime;
            }

            action(1);
            Object.Destroy(comp.gameObject);
        }
    }
}