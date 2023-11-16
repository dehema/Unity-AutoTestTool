using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class ActionItem : PoolItemBase, IPointerClickHandler
{
    ActionData actionData;
    bool isSelect = false;
    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        actionData = _params[0] as ActionData;
        RefreshUI();
        SetSelect(false);
    }

    public override void OnCollect()
    {
        base.OnCollect();
    }

    public void RefreshUI()
    {
        name_Text.text = actionData.name;
    }

    public void SetSelect(bool _isSelect)
    {
        isSelect = _isSelect;
        selBg.SetActive(isSelect);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ActionView editScript = UIMgr.Ins.GetView<ActionView>();
        editScript.SelectActionItem(this);
        editScript.SetCurrActionData(actionData);
    }
}
