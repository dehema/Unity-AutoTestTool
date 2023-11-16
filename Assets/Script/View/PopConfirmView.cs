using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PopConfirmView : BaseView
{
    PopConfirmViewParams viewParams;
    public override void Init(params object[] _params)
    {
        base.Init(_params);
        yes_Button.SetButton(OnClickYes);
        no_Button.SetButton(OnClickNo);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        if (UIMgr.Ins.windowPos != Vector3.zero)
        {
            frame.transform.position = UIMgr.Ins.windowPos;
        }

        viewParams = _params[0] as PopConfirmViewParams;
        title_Text.text = viewParams.title;
    }

    public void OnClickYes()
    {
        viewParams.yesCB?.Invoke();
        Close();
    }

    public void OnClickNo()
    {
        viewParams.noCB?.Invoke();
        Close();
    }
}

public class PopConfirmViewParams
{
    public string title;
    public Action yesCB;
    public Action noCB;

    public PopConfirmViewParams(string _title)
    {
        title = _title;
    }
}