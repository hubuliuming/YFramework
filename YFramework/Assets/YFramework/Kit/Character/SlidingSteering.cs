/****************************************************
    文件：SlidingSteering.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：滑动一定距离移动
*****************************************************/

using System;
using UnityEngine;

namespace YFramework.Kit.Character
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public class SlidingSteering : MonoBehaviour
    {
        private int effectiveDistance = 70;
        private Vector3 starPos = Vector3.zero;
        private bool hasDir = false;

        private void UpdateCurDir()
        {
            Vector3 curPos = Vector3.zero;
            if (Input.GetMouseButtonDown(0) && !hasDir)
            {
                starPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                hasDir = false;
            }
            if (Input.GetMouseButton(0) && !hasDir)
            {
                curPos = Input.mousePosition;
                var offset = (Vector2) (starPos - curPos);
                // 左右移动
                if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                {
                    //left
                    if (offset.x > effectiveDistance)
                    {
                        Debug.Log("left");
                        PlayerMove(Direction.Left);
                        hasDir = true;
                    }
                    //right
                    else if (offset.x < -effectiveDistance)
                    {
                        Debug.Log("Right");
                        PlayerMove(Direction.Right);
                        hasDir = true;
                    }
                }
                //上下移动
                else
                {
                    //上面
                    if (offset.y < -effectiveDistance)
                    {
                        Debug.Log("Up");
                        PlayerMove(Direction.Up);
                        hasDir = true;
                    }
                    else if (offset.y > effectiveDistance)
                    {
                        Debug.Log("Down");
                        PlayerMove(Direction.Down);
                        hasDir = true;
                    }
                }
            }
        }

        private void PlayerMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                case Direction.Left:
                    break;
                case Direction.Right:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }
    }
}