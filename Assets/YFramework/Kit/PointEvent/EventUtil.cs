
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace YFramework.Event
{
    public class EventUtil
    {
        public static void CheckRaycaster(BaseButton.ObjType objType)
        {
            PhysicsRaycaster raycaster = null;
            switch (objType)
            {
                case BaseButton.ObjType.TwoD:
                    raycaster = Object.FindObjectOfType<Physics2DRaycaster>();
                    if (raycaster == null)
                    {
                        if (Camera.main != null)
                        {
                            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
                        }
                        else
                        {
                            Object.FindObjectOfType<Camera>().gameObject.AddComponent<Physics2DRaycaster>();
                        }
                    }
                    break;
                case BaseButton.ObjType.ThreeD:
                    raycaster = Object.FindObjectOfType<PhysicsRaycaster>();
                    if (raycaster == null || raycaster is Physics2DRaycaster)
                    {
                        if (Camera.main != null)
                        {
                            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
                        }
                        else
                        {
                            Object.FindObjectOfType<Camera>().gameObject.AddComponent<PhysicsRaycaster>();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objType), objType, null);
            }
        }
    }
}