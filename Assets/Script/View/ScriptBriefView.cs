using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ScriptBriefView : BaseView
{
    //
    ScriptBriefViewParams viewParams;

    RunScript runScript = null;
    //当前动作数据
    ScriptData currScriptData;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        stop_Button.onClick.AddListener(OnClickStop);
        back_Button.onClick.AddListener(() => { Close(); });
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        viewParams = _params[0] as ScriptBriefViewParams;
        currScriptData = viewParams.scriptData;

        runing.SetActive(true);
        finish.SetActive(false);

        if (UIMgr.Ins.windowPos != Vector3.zero)
        {
            frame.transform.position = UIMgr.Ins.windowPos;
        }
        if (runScript == null)
        {
            runScript = gameObject.AddComponent<RunScript>();
            runScript.RefreshTips = RefreshUI;
            runScript.OnComplete = () =>
            {
                runing.SetActive(false);
                finish.SetActive(true);
            };
        }
        runScript.Launch(currScriptData);
    }


    public override void OnClose(Action _cb)
    {
        runScript.Stop();
        //
        UIMgr.Ins.windowPos = frame.transform.position;
        SaveWindowData();
        UIMgr.Ins.OpenView<MainView>();
        base.OnClose(_cb);
    }

    void OnClickStop()
    {
        runScript.Stop();
        Close();
        viewParams.closeCB?.Invoke();
    }

    void RefreshUI(string val)
    {
        scriptName_Text.text = currScriptData.name;
        loopProgress_Text.text = string.Format("{0}/{1}", runScript.loopCount + 1, currScriptData.loopCount);
        actionProgress_Text.text = string.Format("{0}/{1}", runScript.actionIndex + 1, currScriptData.actionList.Count);
        actionName_Text.text = val;
    }
}

public class ScriptBriefViewParams
{
    public ScriptData scriptData;
    public Action closeCB;

    public ScriptBriefViewParams(ScriptData _scriptData)
    {
        scriptData = _scriptData;
    }
}
