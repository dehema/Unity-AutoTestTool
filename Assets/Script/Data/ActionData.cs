using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData
{
    public int index;
    public string uuid;
    /// <summary>
    /// 
    /// </summary>
    public string name;
    /// <summary>
    /// 类型
    /// </summary>
    public ActionType actionType = 0;
    /// <summary>
    /// 
    /// </summary>
    public string val;
    /// <summary>
    /// 是否循环
    /// </summary>
    public bool isLoop;
    /// <summary>
    /// 循环间隔（秒）
    /// </summary>
    public int interval;

    public static ActionData CreateNew(ScriptData scriptData = null)
    {
        ActionData actionData = new ActionData();
        actionData.uuid = DateTime.Now.ToString("yyyyMMddhhmmssff");
        string name = "new action";
        if (scriptData != null && scriptData.actionList.Count != 0)
        {
            name += (scriptData.actionList.Count + 1).ToString();
        }
        actionData.name = name;
        return actionData;
    }
}

public enum ActionType
{
    /// <summary>
    /// 等待一段时间
    /// </summary>
    Wait,
    /// <summary>
    /// 鼠标点击
    /// </summary>
    MouseClick,
    /// <summary>
    /// 截屏
    /// </summary>
    ScreenCapture,
    /// <summary>
    /// 截屏识别文字
    /// </summary>
    ScreenCaptureAndIdentifyText,
}
