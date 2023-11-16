using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MainView : BaseView
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
    public GameObject scriptItem;
    [HideInInspector]
    public GameObject addScript;
    [HideInInspector]
    public Button addScript_Button;
    [HideInInspector]
    public GameObject delAllScript;
    [HideInInspector]
    public Button delAllScript_Button;
    [HideInInspector]
    public GameObject close;
    [HideInInspector]
    public Button close_Button;
    [HideInInspector]
    public GameObject emptyCurrActionDataTips;
    [HideInInspector]
    public GameObject propertyList;
    [HideInInspector]
    public GameObject name;
    [HideInInspector]
    public InputField name_InputField;
    [HideInInspector]
    public GameObject loopCount;
    [HideInInspector]
    public InputField loopCount_InputField;
    [HideInInspector]
    public GameObject propertyLoopInterval;
    [HideInInspector]
    public GameObject loopInterval;
    [HideInInspector]
    public InputField loopInterval_InputField;
    [HideInInspector]
    public GameObject edit;
    [HideInInspector]
    public Button edit_Button;
    [HideInInspector]
    public GameObject launch;
    [HideInInspector]
    public Button launch_Button;
    [HideInInspector]
    public GameObject save;
    [HideInInspector]
    public Button save_Button;
    [HideInInspector]
    public GameObject del;
    [HideInInspector]
    public Button del_Button;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        frame = transform.Find("$content#Rect/$frame").gameObject;
        scriptItem = transform.Find("$content#Rect/$frame/left/border/Scroll View/Viewport/Content/$scriptItem").gameObject;
        addScript = transform.Find("$content#Rect/$frame/left/border/btList/$addScript#Button").gameObject;
        addScript_Button = addScript.GetComponent<Button>();
        delAllScript = transform.Find("$content#Rect/$frame/left/border/btList/$delAllScript#Button").gameObject;
        delAllScript_Button = delAllScript.GetComponent<Button>();
        close = transform.Find("$content#Rect/$frame/right/$close#Button").gameObject;
        close_Button = close.GetComponent<Button>();
        emptyCurrActionDataTips = transform.Find("$content#Rect/$frame/right/$emptyCurrActionDataTips").gameObject;
        propertyList = transform.Find("$content#Rect/$frame/right/$propertyList").gameObject;
        name = transform.Find("$content#Rect/$frame/right/$propertyList/name/$name#InputField").gameObject;
        name_InputField = name.GetComponent<InputField>();
        loopCount = transform.Find("$content#Rect/$frame/right/$propertyList/loopCount/$loopCount#InputField").gameObject;
        loopCount_InputField = loopCount.GetComponent<InputField>();
        propertyLoopInterval = transform.Find("$content#Rect/$frame/right/$propertyList/$propertyLoopInterval").gameObject;
        loopInterval = transform.Find("$content#Rect/$frame/right/$propertyList/$propertyLoopInterval/$loopInterval#InputField").gameObject;
        loopInterval_InputField = loopInterval.GetComponent<InputField>();
        edit = transform.Find("$content#Rect/$frame/right/$propertyList/btList/$edit#Button").gameObject;
        edit_Button = edit.GetComponent<Button>();
        launch = transform.Find("$content#Rect/$frame/right/$propertyList/btList/$launch#Button").gameObject;
        launch_Button = launch.GetComponent<Button>();
        save = transform.Find("$content#Rect/$frame/right/$propertyList/btList2/$save#Button").gameObject;
        save_Button = save.GetComponent<Button>();
        del = transform.Find("$content#Rect/$frame/right/$propertyList/btList2/$del#Button").gameObject;
        del_Button = del.GetComponent<Button>();
    }
}