using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ActionScreenCaptureIdentifyView : BaseView
{
    ActionScreenCaptureIdentifyViewParams viewParmas;
    List<ActionData> actionDataList = new List<ActionData>();

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        save_Button.SetButton(OnClickSave);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        if (UIMgr.Ins.windowPos != Vector3.zero)
        {
            frame.transform.position = UIMgr.Ins.windowPos;
        }
        viewParmas = _params[0] as ActionScreenCaptureIdentifyViewParams;
        //actionList
        targetAction_Dropdown.ClearOptions();
        actionDataList.Clear();
        foreach (var item in viewParmas.scriptData.actionList)
        {
            if (item != viewParmas.actionData)
            {
                actionDataList.Add(item);
            }
        }
        List<Dropdown.OptionData> OptionDatas = new List<Dropdown.OptionData>();
        foreach (ActionData action in actionDataList)
        {
            OptionDatas.Add(new Dropdown.OptionData(action.name));
        }
        targetAction_Dropdown.AddOptions(OptionDatas);
    }

    void OnClickSave()
    {
        ActionIdentifyData data = new ActionIdentifyData();
        data.text = textVal_InputField.text;
        data.expressType = (ExpressType)actionType_Dropdown.value;
        data.expressCondition = (ExpressCondition)conditionType_Dropdown.value;
        if (actionDataList.Count != 0)
        {
            data.targetActionUUID = actionDataList[targetAction_Dropdown.value].uuid;
        }
        string val = JsonConvert.SerializeObject(data);
        viewParmas.actionData.val = val;
        //
        viewParmas.closeCB.Invoke();
        Close();
    }
}

public class ActionScreenCaptureIdentifyViewParams
{
    public ScriptData scriptData;
    public ActionData actionData;
    public Action closeCB;
}
