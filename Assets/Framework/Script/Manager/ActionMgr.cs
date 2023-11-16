using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMgr : Singleton<ActionMgr>
{
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="actionType"></param>
    /// <returns></returns>
    public string GetActionName(ActionType actionType)
    {
        string name = string.Empty;
        switch (actionType)
        {
            case ActionType.Wait:
                name = "�ȴ�";
                break;
            case ActionType.MouseClick:
                name = "������";
                break;
            case ActionType.ScreenCapture:
                name = "����";
                break;
            case ActionType.ScreenCaptureAndIdentifyText:
                name = "����&�������";
                break;
        }
        return name;
    }

    /// <summary>
    /// ����uuid��ȡ��������
    /// </summary>
    /// <param name="_uuid"></param>
    /// <param name="_scriptData"></param>
    /// <returns></returns>
    public ActionData GetActionDataByUUID(string _uuid, ScriptData _scriptData)
    {
        foreach (var action in _scriptData.actionList)
        {
            if (action.uuid == _uuid)
            {
                return action;
            }
        }
        return null;
    }

    /// <summary>
    /// ˢ�¶�������
    /// </summary>
    /// <param name="_scriptData"></param>
    public void RefreshActionIndex(ScriptData _scriptData)
    {
        int index = 0;
        foreach (ActionData action in _scriptData.actionList)
        {
            action.index = index;
            index++;
        }
    }

    /// <summary>
    /// ���ű�
    /// </summary>
    /// <param name="_scriptData"></param>
    /// <returns></returns>
    public string CheckScipt(ScriptData _scriptData)
    {
        foreach (var actionData in _scriptData.actionList)
        {
            switch (actionData.actionType)
            {
                case ActionType.Wait:
                    {
                        int res = 0;
                        if (!int.TryParse(actionData.val, out res))
                        {
                            return string.Format("����[{0}]ֵ����", actionData.name);
                        }
                        break;
                    }
                case ActionType.MouseClick:
                    {
                        try
                        {
                            JsonConvert.DeserializeObject<ActionMouseClickData>(actionData.val);
                        }
                        catch (System.Exception)
                        {
                            return string.Format("����[{0}]û�����������λ��", actionData.name);
                        }
                        break;
                    }
                case ActionType.ScreenCapture:
                    break;
                case ActionType.ScreenCaptureAndIdentifyText:
                    {
                        ActionIdentifyData actionIdentifyData;
                        try
                        {
                            actionIdentifyData = JsonConvert.DeserializeObject<ActionIdentifyData>(actionData.val);
                        }
                        catch (System.Exception)
                        {
                            return string.Format("����[{0}]û��������ת����������ת����", actionData.name);
                        }
                        if (actionIdentifyData == null ||
                            string.IsNullOrEmpty(actionIdentifyData.text) ||
                            (actionIdentifyData.expressType == ExpressType.MoveTargetAction && string.IsNullOrEmpty(actionIdentifyData.targetActionUUID)))
                        {
                            return string.Format("����[{0}]û��������ת����������ת����", actionData.name);
                        }
                        break;
                    }
            }
        }
        return string.Empty;
    }
}
