using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PopInfoView : BaseView
{

    PopInfoViewParams viewParams;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        yes_Button.onClick.AddListener(() =>
        {
            viewParams.closeCB?.Invoke();
            Close();
        });
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        if (UIMgr.Ins.windowPos != Vector3.zero)
        {
            frame.transform.position = UIMgr.Ins.windowPos;
        }
        viewParams = _params[0] as PopInfoViewParams;
        tips_Text.text = viewParams.tips;
    }
}

public class PopInfoViewParams
{
    public string tips;
    public Action closeCB;

    public PopInfoViewParams(string _tips)
    {
        tips = _tips;
    }
}
