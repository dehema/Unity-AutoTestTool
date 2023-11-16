using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ActionMouseClickView : BaseView
{
    ActionMouseClickViewParams viewParams;

    public override void Init(params object[] _params)
    {
        base.Init(_params);
        clickScreen_Button.SetButton(OnClickScreen);
    }

    public override void OnOpen(params object[] _params)
    {
        base.OnOpen(_params);
        viewParams = _params[0] as ActionMouseClickViewParams;
    }

    public override void OnClose(Action _cb)
    {
        base.OnClose(_cb);
    }

    private void Update()
    {
        img1_Rect.anchoredPosition = new Vector3(img1_Rect.anchoredPosition.x, Input.mousePosition.y - Screen.height / 2, 0);
        img2_Rect.anchoredPosition = new Vector3(Input.mousePosition.x - Screen.width / 2, img2_Rect.anchoredPosition.y, 0);
    }

    void OnClickScreen()
    {
        ActionMouseClickData data = new ActionMouseClickData();
        data.mousePosX = Input.mousePosition.x;
        data.mousePosY = Input.mousePosition.y;
        viewParams.actionData.val = JsonConvert.SerializeObject(data);
        viewParams.closeCB?.Invoke();
        Close();
    }
}

public class ActionMouseClickViewParams
{
    public ActionData actionData;
    public Action closeCB;
}
