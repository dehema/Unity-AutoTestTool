using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ActionView : BaseView
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
    public GameObject listContent;
    [HideInInspector]
    public GameObject actionItem;
    [HideInInspector]
    public GameObject addAction;
    [HideInInspector]
    public Button addAction_Button;
    [HideInInspector]
    public GameObject removeAction;
    [HideInInspector]
    public Button removeAction_Button;
    [HideInInspector]
    public GameObject close;
    [HideInInspector]
    public Button close_Button;
    [HideInInspector]
    public GameObject propertyList;
    [HideInInspector]
    public GameObject name;
    [HideInInspector]
    public InputField name_InputField;
    [HideInInspector]
    public GameObject actionType;
    [HideInInspector]
    public Dropdown actionType_Dropdown;
    [HideInInspector]
    public GameObject actionValInput;
    [HideInInspector]
    public InputField actionValInput_InputField;
    [HideInInspector]
    public GameObject actionVal;
    [HideInInspector]
    public Text actionVal_Text;
    [HideInInspector]
    public GameObject isLoop;
    [HideInInspector]
    public Toggle isLoop_Toggle;
    [HideInInspector]
    public GameObject intervalProperty;
    [HideInInspector]
    public GameObject interval;
    [HideInInspector]
    public InputField interval_InputField;
    [HideInInspector]
    public GameObject expression;
    [HideInInspector]
    public Text expression_Text;
    [HideInInspector]
    public GameObject setVal;
    [HideInInspector]
    public Button setVal_Button;
    [HideInInspector]
    public GameObject emptyCurrActionDataTips;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        frame = transform.Find("$content#Rect/$frame").gameObject;
        listContent = transform.Find("$content#Rect/$frame/left/border/Scroll View/Viewport/$listContent").gameObject;
        actionItem = transform.Find("$content#Rect/$frame/left/border/Scroll View/Viewport/$listContent/$actionItem").gameObject;
        addAction = transform.Find("$content#Rect/$frame/left/border/btList/$addAction#Button").gameObject;
        addAction_Button = addAction.GetComponent<Button>();
        removeAction = transform.Find("$content#Rect/$frame/left/border/btList/$removeAction#Button").gameObject;
        removeAction_Button = removeAction.GetComponent<Button>();
        close = transform.Find("$content#Rect/$frame/right/$close#Button").gameObject;
        close_Button = close.GetComponent<Button>();
        propertyList = transform.Find("$content#Rect/$frame/right/$propertyList").gameObject;
        name = transform.Find("$content#Rect/$frame/right/$propertyList/name/$name#InputField").gameObject;
        name_InputField = name.GetComponent<InputField>();
        actionType = transform.Find("$content#Rect/$frame/right/$propertyList/actionType/$actionType#Dropdown").gameObject;
        actionType_Dropdown = actionType.GetComponent<Dropdown>();
        actionValInput = transform.Find("$content#Rect/$frame/right/$propertyList/val/$actionValInput#InputField").gameObject;
        actionValInput_InputField = actionValInput.GetComponent<InputField>();
        actionVal = transform.Find("$content#Rect/$frame/right/$propertyList/val/$actionVal#Text").gameObject;
        actionVal_Text = actionVal.GetComponent<Text>();
        isLoop = transform.Find("$content#Rect/$frame/right/$propertyList/isLoop/isLoop/$isLoop#Toggle").gameObject;
        isLoop_Toggle = isLoop.GetComponent<Toggle>();
        intervalProperty = transform.Find("$content#Rect/$frame/right/$propertyList/$intervalProperty").gameObject;
        interval = transform.Find("$content#Rect/$frame/right/$propertyList/$intervalProperty/$interval#InputField").gameObject;
        interval_InputField = interval.GetComponent<InputField>();
        expression = transform.Find("$content#Rect/$frame/right/$propertyList/expression/$expression#Text").gameObject;
        expression_Text = expression.GetComponent<Text>();
        setVal = transform.Find("$content#Rect/$frame/right/$propertyList/buttonList1/$setVal#Button").gameObject;
        setVal_Button = setVal.GetComponent<Button>();
        emptyCurrActionDataTips = transform.Find("$content#Rect/$frame/right/$emptyCurrActionDataTips").gameObject;
    }
}