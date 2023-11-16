using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActionMouseClick
{

    //设置按钮位置
    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);
}

/// <summary>
/// 鼠标操作标志位集合
/// </summary>
[Flags]
enum MouseEventFlag : uint
{
    Move = 0x0001,
    LeftDown = 0x0002,
    LeftUp = 0x0004,
    RightDown = 0x0008,
    RightUp = 0x0010,
    MiddleDown = 0x0020,
    MiddleUp = 0x0040,
    XDown = 0x0080,
    XUp = 0x0100,
    Wheel = 0x0800,
    VirtualDesk = 0x4000,
    /// <summary>
    /// 设置鼠标坐标为绝对位置（dx,dy）,否则为距离最后一次事件触发的相对位置
    /// </summary>
    Absolute = 0x8000
}