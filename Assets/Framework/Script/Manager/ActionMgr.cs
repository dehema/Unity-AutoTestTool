using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMgr : Singleton<ActionMgr>
{
    /// <summary>
    /// 获取动作名称
    /// </summary>
    /// <param name="actionType"></param>
    /// <returns></returns>
    public string GetActionName(ActionType actionType)
    {
        string name = string.Empty;
        switch (actionType)
        {
            case ActionType.Wait:
                name = "等待";
                break;
            case ActionType.MouseClick:
                name = "点击鼠标";
                break;
            case ActionType.ScreenCapture:
                name = "截屏";
                break;
            case ActionType.ScreenCaptureAndIdentifyText:
                name = "截屏&检测文字";
                break;
        }
        return name;
    }

    /// <summary>
    /// 根据uuid获取动作数据
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
    /// 刷新动作索引
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
    /// 检查脚本
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
                            return string.Format("动作[{0}]值错误", actionData.name);
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
                            return string.Format("动作[{0}]没有设置鼠标点击位置", actionData.name);
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
                            return string.Format("动作[{0}]没有设置跳转条件或者跳转动作", actionData.name);
                        }
                        if (actionIdentifyData == null ||
                            string.IsNullOrEmpty(actionIdentifyData.text) ||
                            (actionIdentifyData.expressType == ExpressType.MoveTargetAction && string.IsNullOrEmpty(actionIdentifyData.targetActionUUID)))
                        {
                            return string.Format("动作[{0}]没有设置跳转条件或者跳转动作", actionData.name);
                        }
                        break;
                    }
            }
        }
        return string.Empty;
    }
}
