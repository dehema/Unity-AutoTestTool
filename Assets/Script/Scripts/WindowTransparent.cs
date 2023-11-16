using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowTransparent : MonoSingleton<WindowTransparent>
{
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwnewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlages);
    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    const int GWL_EXSTYLE = -20;
    //�ô�����һ���ֲ㴰��
    const uint WS_EX_LAYERED = 0x00080000;
    //�ô�����һ��͸������
    const uint WS_EX_TRANSPARENT = 0x00000020;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint LWA_COLORKEY = 0x00000001;
    // ����ڵľ��
    private IntPtr hWnd;
    private void Start()
    {
        //MessageBox(new IntPtr(0), "text", "caption", 0);
#if !UNITY_EDITOR
        //��ȡ��ǰ���ھ��
        hWnd = GetActiveWindow();
        // ����һ���߾�ṹ������߿��С
        Margins margins = new Margins { cxLeftWidth = -1 };
        //��ܽ���չ���������Ĵ��ڵľ�� ������Ч����
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
        //��չ������ʽ
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        //SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        //���÷ֲ㴰��͸���� ������������ɫΪ0�ĵط����͸��
        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
        //���ô���ʼ���ö�
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif
        Application.runInBackground = true;
    }
}
