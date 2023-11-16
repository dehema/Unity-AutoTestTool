using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ActionScreenCaptureIdentifyView : BaseView
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
    public GameObject title;
    [HideInInspector]
    public Text title_Text;
    [HideInInspector]
    public GameObject save;
    [HideInInspector]
    public Button save_Button;
    [HideInInspector]
    public GameObject conditionType;
    [HideInInspector]
    public Dropdown conditionType_Dropdown;
    [HideInInspector]
    public GameObject textVal;
    [HideInInspector]
    public InputField textVal_InputField;
    [HideInInspector]
    public GameObject targetAction;
    [HideInInspector]
    public Dropdown targetAction_Dropdown;
    [HideInInspector]
    public GameObject actionType;
    [HideInInspector]
    public Dropdown actionType_Dropdown;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        frame = transform.Find("$content#Rect/$frame").gameObject;
        title = transform.Find("$content#Rect/$frame/$title#Text").gameObject;
        title_Text = title.GetComponent<Text>();
        save = transform.Find("$content#Rect/$frame/$save#Button").gameObject;
        save_Button = save.GetComponent<Button>();
        conditionType = transform.Find("$content#Rect/$frame/express/$conditionType#Dropdown").gameObject;
        conditionType_Dropdown = conditionType.GetComponent<Dropdown>();
        textVal = transform.Find("$content#Rect/$frame/express/$textVal#InputField").gameObject;
        textVal_InputField = textVal.GetComponent<InputField>();
        targetAction = transform.Find("$content#Rect/$frame/express/$targetAction#Dropdown").gameObject;
        targetAction_Dropdown = targetAction.GetComponent<Dropdown>();
        actionType = transform.Find("$content#Rect/$frame/express/$actionType#Dropdown").gameObject;
        actionType_Dropdown = actionType.GetComponent<Dropdown>();
    }
}