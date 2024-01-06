using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public partial class ActionView : BaseView
{
    ActionViewParams actionViewParams;
    ObjPool actionPool;
    Dictionary<ActionData, ActionItem> actionItemDict = new Dictionary<ActionData, ActionItem>();
    //当前动作数据
    ActionData currActionData;
    ActionItem currActionItem;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        addAction_Button.SetButton(OnClickAddAction);
        removeAction_Button.SetButton(OnClickRemoveAction);
        close_Button.SetButton(OnClickClose);
        setVal_Button.SetButton(OnClickSetVal);
        actionPool = PoolMgr.Ins.CreatePool(actionItem);
        //onValueChange
        actionType_Dropdown.ClearOptions();
        name_InputField.onValueChanged.AddListener((str) => { currActionData.name = str; currActionItem.RefreshUI(); });
        actionType_Dropdown.onValueChanged.AddListener((val) =>
        {
            currActionData.actionType = (ActionType)val;
            RefreshUI();
        });
        isLoop_Toggle.onValueChanged.AddListener((isOn) => { currActionData.isLoop = isOn; intervalProperty.SetActive(isOn); });
        interval_InputField.onValueChanged.AddListener((val) => { currActionData.interval = int.Parse(val); });
        actionValInput_InputField.onEndEdit.AddListener(OnEndEditValInput);
        //actionType
        List<Dropdown.OptionData> OptionDatas = new List<Dropdown.OptionData>();
        foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
        {
            OptionDatas.Add(new Dropdown.OptionData(ActionMgr.Ins.GetActionName(action)));
        }
        actionType_Dropdown.AddOptions(OptionDatas);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        actionViewParams = _params[0] as ActionViewParams;
        frame.transform.position = UIMgr.Ins.windowPos;
        InitActionList();
    }

    public override void OnClose(Action _cb)
    {
        UIMgr.Ins.windowPos = frame.transform.position;
        SaveWindowData();
        base.OnClose(_cb);
    }

    void RefreshUI()
    {
        if (currActionData == null)
        {
            return;
        }
        name_InputField.text = currActionData.name;
        actionType_Dropdown.value = (int)currActionData.actionType;
        isLoop_Toggle.isOn = currActionData.isLoop;
        interval_InputField.text = currActionData.interval.ToString();
        //设置值按钮
        bool showTextVal = IsShowSetVal();
        setVal.SetActive(showTextVal);
        //值文本
        bool showActionValInput = currActionData.actionType == ActionType.Wait;
        if (showActionValInput)
        {
            int res = 0;
            try
            {
                res = int.Parse(currActionData.val);
            }
            catch (Exception)
            {
                throw;
            }
            actionValInput_InputField.text = res.ToString();
        }
        else
        {
            RefreshValText();
        }
        actionVal.SetActive(!showActionValInput);
        actionValInput.SetActive(showActionValInput);
    }

    /// <summary>
    /// 刷新动作值文本
    /// </summary>
    void RefreshValText()
    {
        string valStr = string.Empty;
        switch (currActionData.actionType)
        {
            case ActionType.MouseClick:
                {
                    try
                    {
                        ActionMouseClickData data = JsonConvert.DeserializeObject<ActionMouseClickData>(currActionData.val);
                        valStr = string.Format("{0},{1}", (int)data.mousePosX, (int)data.mousePosY);
                    }
                    catch (Exception)
                    {
                        valStr = string.Empty;
                    }
                    break;
                }
            case ActionType.ScreenCapture:
                break;
            case ActionType.ScreenCaptureAndIdentifyText:
                {
                    ActionIdentifyData data;
                    try
                    {
                        data = JsonConvert.DeserializeObject<ActionIdentifyData>(currActionData.val);
                        valStr = string.Format("{0}[{1}]-", data.expressCondition == ExpressCondition.contain ? "包含" : "不包含", data.text);
                        if (data.expressType == ExpressType.Exit)
                        {
                            valStr += "跳出";
                        }
                        else if (data.expressType == ExpressType.MoveTargetAction)
                        {
                            valStr += "跳转到";
                            valStr += ActionMgr.Ins.GetActionDataByUUID(data.targetActionUUID, actionViewParams.scriptData).name;
                        }
                    }
                    catch (System.Exception)
                    {
                        valStr = string.Empty;
                    }
                    break;
                }
        }
        actionVal_Text.text = valStr;
    }

    /// <summary>
    /// 是否显示设置值按钮
    /// </summary>
    /// <returns></returns>
    public bool IsShowSetVal()
    {
        if (currActionData == null)
            return false;
        switch (currActionData.actionType)
        {
            case ActionType.MouseClick:
            case ActionType.ScreenCaptureAndIdentifyText:
                return true;
        }
        return false;
    }

    /// <summary>
    /// 创建动作列表
    /// </summary>
    void InitActionList()
    {
        actionPool.CollectAll();
        actionItemDict.Clear();
        foreach (var actionData in actionViewParams.scriptData.actionList)
        {
            ActionItem item = actionPool.Get<ActionItem>(actionData);
            actionItemDict.Add(actionData, item);
        }
        if (actionViewParams.scriptData.actionList.Count == 0)
        {
            SetCurrActionData(null);
        }
        else
        {
            actionItemDict.First().Value.OnPointerClick(null);
        }
    }

    /// <summary>
    /// 添加动作
    /// </summary>
    void OnClickAddAction()
    {
        ActionData newActionData = ActionData.CreateNew(actionViewParams.scriptData);
        ActionItem item = actionPool.Get<ActionItem>(newActionData);
        actionItemDict.Add(newActionData, item);
        actionViewParams.scriptData.actionList.Add(newActionData);
        SelectActionItem(item);
        SetCurrActionData(newActionData);
        ActionMgr.Ins.RefreshActionIndex(actionViewParams.scriptData);
    }

    /// <summary>
    /// 移除动作
    /// </summary>
    void OnClickRemoveAction()
    {
        if (currActionItem == null)
        {
            return;
        }
        actionPool.CollectOne(currActionItem.gameObject);
        actionItemDict.Remove(currActionData);
        actionViewParams.scriptData.actionList.Remove(currActionData);
        currActionItem = null;
        SetCurrActionData(null);
        ActionMgr.Ins.RefreshActionIndex(actionViewParams.scriptData);
    }

    /// <summary>
    /// 设置当前动作数据
    /// </summary>
    /// <param name="_actionData"></param>
    public void SetCurrActionData(ActionData _actionData)
    {
        currActionData = _actionData;
        bool isNotEmpty = currActionData != null;
        propertyList.SetActive(isNotEmpty);
        emptyCurrActionDataTips.SetActive(!isNotEmpty);
        removeAction_Button.interactable = isNotEmpty;
        setVal_Button.interactable = isNotEmpty;
        //UI
        RefreshUI();
    }

    void OnClickClose()
    {
        SaveWindowData();
        Close();
        UIMgr.Ins.OpenView<MainView>();
    }

    /// <summary>
    /// 选择动作对象
    /// </summary>
    public void SelectActionItem(ActionItem _actionItem)
    {
        currActionItem = _actionItem;
        foreach (var item in actionItemDict)
        {
            var actionItem = item.Value;
            actionItem.SetSelect(actionItem == _actionItem);
        }
    }

    /// <summary>
    /// 设置值
    /// </summary>
    void OnClickSetVal()
    {
        if (currActionData == null)
        {
            return;
        }
        switch (currActionData.actionType)
        {
            case ActionType.MouseClick:
                {
                    ActionMouseClickViewParams viewParams = new ActionMouseClickViewParams();
                    viewParams.actionData = currActionData;
                    viewParams.closeCB = () =>
                    {
                        RefreshUI();
                        frame.SetActive(true);
                    };
                    UIMgr.Ins.OpenView<ActionMouseClickView>(viewParams);
                    frame.SetActive(false);
                    break;
                }
            case ActionType.ScreenCapture:
                break;
            case ActionType.ScreenCaptureAndIdentifyText:
                ActionScreenCaptureIdentifyViewParams _viewParmas = new ActionScreenCaptureIdentifyViewParams();
                _viewParmas.scriptData = actionViewParams.scriptData;
                _viewParmas.actionData = currActionData;
                _viewParmas.closeCB = () =>
                {
                    RefreshUI();
                    frame.SetActive(true);
                };
                UIMgr.Ins.OpenView<ActionScreenCaptureIdentifyView>(_viewParmas);
                frame.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 值输入框
    /// </summary>
    /// <param name="_val"></param>
    void OnEndEditValInput(string _val)
    {
        currActionData.val = _val;
    }
}

public class ActionViewParams
{
    /// <summary>
    /// 脚本数据
    /// </summary>
    public ScriptData scriptData;
}