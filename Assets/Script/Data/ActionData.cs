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
    /// ����
    /// </summary>
    public ActionType actionType = 0;
    /// <summary>
    /// 
    /// </summary>
    public string val;
    /// <summary>
    /// �Ƿ�ѭ��
    /// </summary>
    public bool isLoop;
    /// <summary>
    /// ѭ��������룩
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
    /// �ȴ�һ��ʱ��
    /// </summary>
    Wait,
    /// <summary>
    /// �����
    /// </summary>
    MouseClick,
    /// <summary>
    /// ����
    /// </summary>
    ScreenCapture,
    /// <summary>
    /// ����ʶ������
    /// </summary>
    ScreenCaptureAndIdentifyText,
}
