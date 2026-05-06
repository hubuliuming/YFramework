using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.UI
{
    public struct UILayerRoot
    {
        public int Layer;
        public Transform Root;

        public UILayerRoot(int layer, Transform root)
        {
            Layer = layer;
            Root = root;
        }
    }

    public struct UIRuntimeCallbacks
    {
        public Action<GameObject, int, object> NotifyOpen;
        public Action<GameObject> NotifyClose;
        public Func<GameObject, int, object, IEnumerator> NotifyOpenAsync;
        public Func<GameObject, IEnumerator> NotifyCloseAsync;
        public Action<GameObject> CacheRuntimeObject;
        public Action<GameObject> InitializeObject;
        public Func<GameObject, bool> ShouldDeferSceneObject;
    }

    public sealed class UIKitRuntime
    {
        private sealed class UIRecord
        {
            public int Layer;
            public GameObject Source;
            public GameObject Instance;
            public bool IsRuntimeInstance;
            public bool IsOpen;
        }

        private readonly MonoBehaviour m_owner;
        private readonly Dictionary<int, UIRecord> m_recordsBySourceId = new Dictionary<int, UIRecord>();
        private readonly Dictionary<int, UIRecord> m_recordsByInstanceId = new Dictionary<int, UIRecord>();
        private readonly Dictionary<int, List<UIRecord>> m_openedByLayer = new Dictionary<int, List<UIRecord>>();

        public UIKitRuntime(MonoBehaviour owner)
        {
            m_owner = owner;
        }

        public GameObject Open(int layer, Transform layerRoot, GameObject uiObject, object userData, UIRuntimeCallbacks callbacks)
        {
            if (uiObject == null)
            {
                Debug.LogError("OpenUI failed because uiObject is null.");
                return null;
            }

            if (layerRoot == null)
            {
                Debug.LogErrorFormat(m_owner, "OpenUI failed because layer root '{0}' is missing.", layer);
                return null;
            }

            UIRecord record = GetOrCreateRecord(layer, uiObject);
            if (record == null || record.Instance == null)
            {
                Debug.LogError("OpenUI failed because the UI record could not be created.");
                return null;
            }

            if (record.IsOpen && record.Instance.activeSelf)
            {
                record.Instance.transform.SetParent(layerRoot, false);
                record.Instance.transform.SetAsLastSibling();
                return record.Instance;
            }

            ChangeLayer(record, layer);
            record.Instance.transform.SetParent(layerRoot, false);
            record.Instance.transform.SetAsLastSibling();
            record.Instance.SetActive(true);
            record.IsOpen = true;

            if (record.IsRuntimeInstance && callbacks.CacheRuntimeObject != null)
            {
                callbacks.CacheRuntimeObject(record.Instance);
            }

            if (callbacks.InitializeObject != null)
            {
                callbacks.InitializeObject(record.Instance);
            }

            MarkOpened(record);
            if (callbacks.NotifyOpen != null)
            {
                callbacks.NotifyOpen(record.Instance, record.Layer, userData);
            }

            if (callbacks.NotifyOpenAsync != null)
            {
                m_owner.StartCoroutine(callbacks.NotifyOpenAsync(record.Instance, record.Layer, userData));
            }

            return record.Instance;
        }

        public bool Close(GameObject uiObject, UIRuntimeCallbacks callbacks)
        {
            UIRecord record;
            if (!TryGetRecord(uiObject, out record))
            {
                return false;
            }

            if (!record.IsOpen || record.Instance == null)
            {
                return false;
            }

            if (callbacks.NotifyCloseAsync != null)
            {
                m_owner.StartCoroutine(CloseRoutine(record, callbacks));
                return true;
            }

            if (callbacks.NotifyClose != null)
            {
                callbacks.NotifyClose(record.Instance);
            }

            record.Instance.SetActive(false);
            record.IsOpen = false;
            RemoveOpened(record);
            return true;
        }

        public bool CloseTop(int layer, UIRuntimeCallbacks callbacks)
        {
            List<UIRecord> records = GetOpenedRecords(layer);
            if (records.Count == 0)
            {
                return false;
            }

            return Close(records[records.Count - 1].Instance, callbacks);
        }

        public void CloseLayer(int layer, UIRuntimeCallbacks callbacks)
        {
            List<UIRecord> records = GetOpenedRecords(layer);
            for (int i = records.Count - 1; i >= 0; i--)
            {
                Close(records[i].Instance, callbacks);
            }
        }

        public bool IsOpen(GameObject uiObject)
        {
            UIRecord record;
            if (!TryGetRecord(uiObject, out record))
            {
                return false;
            }

            return record.IsOpen && record.Instance != null && record.Instance.activeSelf;
        }

        public bool Release(GameObject uiObject, UIRuntimeCallbacks callbacks)
        {
            UIRecord record;
            if (!TryGetRecord(uiObject, out record))
            {
                return false;
            }

            if (record.Instance != null && record.IsOpen && callbacks.NotifyCloseAsync != null)
            {
                m_owner.StartCoroutine(ReleaseRoutine(record, callbacks));
                return true;
            }

            if (record.Instance != null && record.IsOpen && callbacks.NotifyClose != null)
            {
                callbacks.NotifyClose(record.Instance);
            }

            if (record.Instance != null)
            {
                record.Instance.SetActive(false);
            }

            UnregisterRecord(record);

            if (record.IsRuntimeInstance && record.Instance != null)
            {
                UnityEngine.Object.Destroy(record.Instance);
            }

            return true;
        }

        public GameObject GetOpened(GameObject uiObject)
        {
            UIRecord record;
            if (!TryGetRecord(uiObject, out record))
            {
                return null;
            }

            return record.IsOpen ? record.Instance : null;
        }

        public void RegisterSceneUI(IEnumerable<UILayerRoot> layerRoots, UIRuntimeCallbacks callbacks)
        {
            ReleaseAllOpenUIs(callbacks);
            ClearRecords();

            foreach (UILayerRoot pair in layerRoots)
            {
                Transform layerRoot = pair.Root;
                if (layerRoot == null)
                {
                    continue;
                }

                for (int i = 0; i < layerRoot.childCount; i++)
                {
                    GameObject uiObject = layerRoot.GetChild(i).gameObject;
                    bool isOpen = uiObject.activeSelf;

                    if (callbacks.ShouldDeferSceneObject != null && callbacks.ShouldDeferSceneObject(uiObject))
                    {
                        isOpen = false;
                        uiObject.SetActive(false);
                    }

                    UIRecord record = new UIRecord
                    {
                        Layer = pair.Layer,
                        Source = uiObject,
                        Instance = uiObject,
                        IsRuntimeInstance = false,
                        IsOpen = isOpen
                    };

                    m_recordsBySourceId[uiObject.GetInstanceID()] = record;
                    m_recordsByInstanceId[uiObject.GetInstanceID()] = record;

                    if (record.IsOpen)
                    {
                        MarkOpened(record);
                    }
                }
            }
        }

        private UIRecord GetOrCreateRecord(int layer, GameObject uiObject)
        {
            UIRecord record;
            if (TryGetRecord(uiObject, out record))
            {
                if (record.Instance == null)
                {
                    UnregisterRecord(record);
                }
                else
                {
                    return record;
                }
            }

            bool isSceneObject = uiObject.scene.IsValid();
            GameObject instance = isSceneObject ? uiObject : UnityEngine.Object.Instantiate(uiObject);

            record = new UIRecord
            {
                Layer = layer,
                Source = uiObject,
                Instance = instance,
                IsRuntimeInstance = !isSceneObject,
                IsOpen = false
            };

            m_recordsBySourceId[uiObject.GetInstanceID()] = record;
            m_recordsByInstanceId[instance.GetInstanceID()] = record;
            return record;
        }

        private bool TryGetRecord(GameObject uiObject, out UIRecord record)
        {
            record = null;
            if (uiObject == null)
            {
                return false;
            }

            int instanceId = uiObject.GetInstanceID();

            if (m_recordsByInstanceId.TryGetValue(instanceId, out record))
            {
                return record != null;
            }

            if (m_recordsBySourceId.TryGetValue(instanceId, out record))
            {
                return record != null;
            }

            return false;
        }

        private void ChangeLayer(UIRecord record, int layer)
        {
            if (record.Layer == layer)
            {
                return;
            }

            RemoveOpened(record);
            record.Layer = layer;
        }

        private void MarkOpened(UIRecord record)
        {
            List<UIRecord> records = GetOpenedRecords(record.Layer);
            records.Remove(record);
            records.Add(record);
        }

        private void RemoveOpened(UIRecord record)
        {
            GetOpenedRecords(record.Layer).Remove(record);
        }

        private List<UIRecord> GetOpenedRecords(int layer)
        {
            List<UIRecord> records;
            if (!m_openedByLayer.TryGetValue(layer, out records))
            {
                records = new List<UIRecord>();
                m_openedByLayer.Add(layer, records);
            }

            return records;
        }

        private void UnregisterRecord(UIRecord record)
        {
            if (record == null)
            {
                return;
            }

            RemoveOpened(record);

            if (record.Source != null)
            {
                m_recordsBySourceId.Remove(record.Source.GetInstanceID());
            }

            if (record.Instance != null)
            {
                m_recordsByInstanceId.Remove(record.Instance.GetInstanceID());
            }
        }

        private IEnumerator CloseRoutine(UIRecord record, UIRuntimeCallbacks callbacks)
        {
            yield return callbacks.NotifyCloseAsync(record.Instance);

            if (callbacks.NotifyClose != null)
            {
                callbacks.NotifyClose(record.Instance);
            }

            if (record.Instance != null)
            {
                record.Instance.SetActive(false);
            }

            record.IsOpen = false;
            RemoveOpened(record);
        }

        private IEnumerator ReleaseRoutine(UIRecord record, UIRuntimeCallbacks callbacks)
        {
            yield return callbacks.NotifyCloseAsync(record.Instance);

            if (callbacks.NotifyClose != null)
            {
                callbacks.NotifyClose(record.Instance);
            }

            if (record.Instance != null)
            {
                record.Instance.SetActive(false);
            }

            UnregisterRecord(record);

            if (record.IsRuntimeInstance && record.Instance != null)
            {
                UnityEngine.Object.Destroy(record.Instance);
            }
        }

        private void ReleaseAllOpenUIs(UIRuntimeCallbacks callbacks)
        {
            var openedRecords = new List<UIRecord>();
            foreach (var kvp in m_openedByLayer)
            {
                openedRecords.AddRange(kvp.Value);
            }

            for (int i = openedRecords.Count - 1; i >= 0; i--)
            {
                UIRecord record = openedRecords[i];
                if (record.Instance == null)
                {
                    continue;
                }

                if (record.IsOpen && callbacks.NotifyClose != null)
                {
                    callbacks.NotifyClose(record.Instance);
                }

                record.Instance.SetActive(false);
                record.IsOpen = false;

                if (record.IsRuntimeInstance)
                {
                    UnityEngine.Object.Destroy(record.Instance);
                }
            }
        }

        private void ClearRecords()
        {
            m_recordsBySourceId.Clear();
            m_recordsByInstanceId.Clear();
            m_openedByLayer.Clear();
        }
    }
}
