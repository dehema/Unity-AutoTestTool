using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ScriptItem : PoolItemBase
{
    [HideInInspector]
    public GameObject selBg;
    [HideInInspector]
    public GameObject name;
    [HideInInspector]
    public Text name_Text;

    override internal void _LoadUI()    
    {
        selBg = transform.Find("$selBg").gameObject;
        name = transform.Find("$name#Text").gameObject;
        name_Text = name.GetComponent<Text>();
    }
}