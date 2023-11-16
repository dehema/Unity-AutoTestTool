using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActionMouseClick
{

    //���ð�ťλ��
    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);
}

/// <summary>
/// ��������־λ����
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
    /// �����������Ϊ����λ�ã�dx,dy��,����Ϊ�������һ���¼����������λ��
    /// </summary>
    Absolute = 0x8000
}