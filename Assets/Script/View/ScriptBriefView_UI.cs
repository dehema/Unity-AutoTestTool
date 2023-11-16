using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ScriptBriefView : BaseView
{
    [HideInInspector]
    public GameObject bg;
    [HideInInspector]
    public Image bg_Image;
    [HideInInspector]
    public Button bg_Button;
    [HideInInspector]
    public GameObject content;
    [HideInInspector]
    public RectTransform content_Rect;
    [HideInInspector]
    public GameObject frame;
    [HideInInspector]
    public GameObject runing;
    [HideInInspector]
    public GameObject scriptName;
    [HideInInspector]
    public Text scriptName_Text;
    [HideInInspector]
    public GameObject loopProgress;
    [HideInInspector]
    public Text loopProgress_Text;
    [HideInInspector]
    public GameObject actionProgress;
    [HideInInspector]
    public Text actionProgress_Text;
    [HideInInspector]
    public GameObject actionName;
    [HideInInspector]
    public Text actionName_Text;
    [HideInInspector]
    public GameObject stop;
    [HideInInspector]
    public Button stop_Button;
    [HideInInspector]
    public GameObject finish;
    [HideInInspector]
    public GameObject back;
    [HideInInspector]
    public Button back_Button;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        frame = transform.Find("$content#Rect/$frame").gameObject;
        runing = transform.Find("$content#Rect/$frame/$runing").gameObject;
        scriptName = transform.Find("$content#Rect/$frame/$runing/scriptName/$scriptName#Text").gameObject;
        scriptName_Text = scriptName.GetComponent<Text>();
        loopProgress = transform.Find("$content#Rect/$frame/$runing/loopProgress/$loopProgress#Text").gameObject;
        loopProgress_Text = loopProgress.GetComponent<Text>();
        actionProgress = transform.Find("$content#Rect/$frame/$runing/actionProgress/$actionProgress#Text").gameObject;
        actionProgress_Text = actionProgress.GetComponent<Text>();
        actionName = transform.Find("$content#Rect/$frame/$runing/actionName/$actionName#Text").gameObject;
        actionName_Text = actionName.GetComponent<Text>();
        stop = transform.Find("$content#Rect/$frame/$runing/bt/$stop#Button").gameObject;
        stop_Button = stop.GetComponent<Button>();
        finish = transform.Find("$content#Rect/$frame/$finish").gameObject;
        back = transform.Find("$content#Rect/$frame/$finish/$back#Button").gameObject;
        back_Button = back.GetComponent<Button>();
    }
}