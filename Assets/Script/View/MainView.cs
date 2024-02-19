using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class MainView : BaseView
{
    ObjPool scriptPool;
    Dictionary<ScriptData, ScriptItem> scriptItemDict = new Dictionary<ScriptData, ScriptItem>();
    //当前动作数据
    ScriptData currScriptData;
    ScriptItem currScriptItem;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        //pool
        scriptPool = PoolMgr.Ins.CreatePool(scriptItem);
        //button
        addScript_Button.SetButton(OnclickAddScript);
        delAllScript_Button.SetButton(OnclickDelAllScript);
        close_Button.SetButton(OnClickClose);
        edit_Button.SetButton(OnClickEdit);
        launch_Button.SetButton(OnClickLaunch);
        save_Button.SetButton(() => { Utility.PopTips("保存成功"); DataMgr.Ins.SaveGameData(); });
        del_Button.SetButton(OnClickDelScript);
        //onchange
        name_InputField.onValueChanged.AddListener(OnNameCountChange);
        loopCount_InputField.onValueChanged.AddListener(OnLoopCountChange);
        loopInterval_InputField.onValueChanged.AddListener(OnLoopIntervalChange);
        //UI
        InitScriptList();
        version_Text.text = "1.2";
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        if (UIMgr.Ins.windowPos != Vector3.zero)
        {
            frame.transform.position = UIMgr.Ins.windowPos;
        }
    }

    public override void OnClose(Action _cb)
    {
        UIMgr.Ins.windowPos = frame.transform.position;
        SaveWindowData();
        base.OnClose(_cb);
    }

    /// <summary>
    /// 初始化脚本列表
    /// </summary>
    void InitScriptList()
    {
        scriptPool.CollectAll();
        scriptItemDict.Clear();
        foreach (var scriptData in DataMgr.Ins.gameData.scirptList)
        {
            ScriptItem item = scriptPool.Get<ScriptItem>(scriptData);
            scriptItemDict.Add(scriptData, item);
        }
        if (scriptItemDict.Count == 0)
        {
            SetCurrScriptData(null);
        }
        else
        {
            scriptItemDict.First().Value.OnPointerClick(null);
        }
    }

    /// <summary>
    /// 增加脚本名
    /// </summary>
    void OnclickAddScript()
    {
        ScriptData newScriptData = ScriptData.CreateNew();
        ScriptItem item = scriptPool.Get<ScriptItem>(newScriptData);
        scriptItemDict.Add(newScriptData, item);
        SelectScriptItem(item);
        SetCurrScriptData(newScriptData);
        DataMgr.Ins.gameData.scirptList.Add(newScriptData);
        DataMgr.Ins.SaveGameData();
    }

    /// <summary>
    /// 选择脚本对象
    /// </summary>
    public void SelectScriptItem(ScriptItem _scriptItem)
    {
        currScriptItem = _scriptItem;
        foreach (var item in scriptItemDict)
        {
            var scriptItem = item.Value;
            scriptItem.SetSelect(scriptItem == _scriptItem);
        }
    }

    /// <summary>
    /// 设置当前脚本数据
    /// </summary>
    /// <param name="_scriptData"></param>
    public void SetCurrScriptData(ScriptData _scriptData)
    {
        currScriptData = _scriptData;
        bool isNotEmpty = currScriptData != null;
        propertyList.SetActive(isNotEmpty);
        emptyCurrActionDataTips.SetActive(!isNotEmpty);
        edit_Button.interactable = isNotEmpty;
        launch_Button.interactable = isNotEmpty;
        del_Button.interactable = isNotEmpty;
        //UI
        RefreshUI();
    }

    void RefreshUI()
    {
        if (currScriptData == null)
        {
            return;
        }
        name_InputField.text = currScriptData.name;
        loopCount_InputField.text = currScriptData.loopCount.ToString();
        loopInterval_InputField.text = currScriptData.loopInterval.ToString();
        propertyLoopInterval.SetActive(currScriptData.loopCount > 1);
    }

    /// <summary>
    /// 删除所有脚本
    /// </summary>
    void OnclickDelAllScript()
    {
        SaveWindowData();
        PopConfirmViewParams viewParams = new PopConfirmViewParams("是否删除所有脚本");
        viewParams.yesCB = () =>
        {
            scriptPool.CollectAll();
            scriptItemDict.Clear();
            currScriptData = null;
            DataMgr.Ins.gameData.scirptList.Clear();
            DataMgr.Ins.SaveGameData();
            SetCurrScriptData(null);
        };
        UIMgr.Ins.OpenView<PopConfirmView>(viewParams);
    }

    /// <summary>
    /// 点击编辑
    /// </summary>
    void OnClickEdit()
    {
        if (currScriptData == null)
        {
            return;
        }
        Close();
        ActionViewParams viewParams = new ActionViewParams();
        viewParams.scriptData = currScriptData;
        UIMgr.Ins.OpenView<ActionView>(viewParams);
    }

    void OnClickClose()
    {
        SaveWindowData();
        DataMgr.Ins.SaveGameData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 点击运行
    /// </summary>
    void OnClickLaunch()
    {
        string tips = ActionMgr.Ins.CheckScipt(currScriptData);
        if (!string.IsNullOrEmpty(tips))
        {
            PopInfoViewParams viewPrams = new PopInfoViewParams(tips);
            UIMgr.Ins.OpenView<PopInfoView>(viewPrams);
            return;
        }
        ScriptBriefViewParams viewParams = new ScriptBriefViewParams(currScriptData);
        UIMgr.Ins.OpenView<ScriptBriefView>(viewParams);
        Close();
    }

    void OnNameCountChange(string str)
    {
        currScriptData.name = str;
        currScriptItem?.RefreshUI();
    }

    void OnLoopCountChange(string str)
    {
        currScriptData.loopCount = int.Parse(str);
        if (currScriptData.loopCount <= 1)
        {
            currScriptData.loopInterval = 0;
        }
        RefreshUI();
    }

    void OnLoopIntervalChange(string str)
    {
        currScriptData.loopInterval = int.Parse(str);
    }

    void OnClickDelScript()
    {
        if (currScriptItem == null)
        {
            return;
        }
        scriptPool.CollectOne(currScriptItem.gameObject);
        scriptItemDict.Remove(currScriptData);
        DataMgr.Ins.gameData.scirptList.Remove(currScriptData);
        currScriptItem = null;
        SetCurrScriptData(null);
    }
}
