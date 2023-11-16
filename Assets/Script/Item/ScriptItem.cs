using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class ScriptItem : PoolItemBase, IPointerClickHandler
{

    ScriptData scriptData;
    bool isSelect = false;
    public override void OnCreate(params object[] _params)
    {
        base.OnCreate(_params);
        scriptData = _params[0] as ScriptData;
        RefreshUI();
        SetSelect(false);
    }

    public override void OnCollect()
    {
        base.OnCollect();
    }

    public void RefreshUI()
    {
        name_Text.text = scriptData.name;
    }

    public void SetSelect(bool _isSelect)
    {
        isSelect = _isSelect;
        selBg.SetActive(isSelect);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MainView editScript = UIMgr.Ins.GetView<MainView>();
        editScript.SelectScriptItem(this);
        editScript.SetCurrScriptData(scriptData);
    }
}