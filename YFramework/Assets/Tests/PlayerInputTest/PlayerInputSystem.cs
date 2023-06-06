// /****************************************************
//     文件：PlayerInputSystem.cs
//     作者：Y
//     邮箱: 916111418@qq.com
//     日期：#CreateTime#
//     功能：Nothing
// *****************************************************/
//
// using System.Collections.Generic;
// using UnityEngine;
//
// public class PlayerInputSystem : MonoBehaviour
// {
//     public static PlayerInputSystem System;
//     public static PlayerInput MyInput;
//
//     private static string[] names =
//     {
//         "Left", "Right", "Attack", "Jump", "Rush", "Select", "Esc"
//     };
//
//     public Dictionary<string, int> buttons = new Dictionary<string, int>();
//     public static string preAction;
//
//     public static bool IsJoys;
//
//     //战斗界面输入
//     public static bool IsJumpDown;
//     public static bool IsJumpAir;
//     public static bool IsAttackDown;
//     public static bool IsLeft;
//     public static bool IsRight;
//     public static bool IsEsc;
//     public static bool IsToDo;
//
//     private void Awake()
//     {
//         MyInput = GetComponent<PlayerInput>();
//         System = this;
//         for (int i = 0; i < names.Length; i++)
//         {
//             buttons.Add(names[i], 0);
//         }
//     }
//
//     public void Update()
//     {
//         IsEsc = MyInput.actions["Esc"].WasPressedThisFrame();
//         IsRight = MyInput.actions["Right"].IsPressed();
//         IsLeft = MyInput.actions["Left"].IsPressed();
//
//         // if (IsRight)
//         // {
//         //     Debug.Log("Right");
//         // }
//         //
//         // if (IsLeft)
//         // {
//         //     Debug.Log("Left");
//         // }
//         //
//         // if (IsEsc)
//         // {
//         //     Debug.Log("Esc");
//         // }
//     }
//
//     public void Buttons(InputAction.CallbackContext context)
//     {
//         preAction = context.action.name;
//
//         if (context.control.device.displayName == "Keyboard" ||
//             context.control.device.displayName == "Mouse")
//         {
//             IsJoys = false;
//         }
//         else
//         {
//             IsJoys = true;
//         }
//     }
// }