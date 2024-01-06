using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PopConfirmView : BaseView
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
    public GameObject yes;
    [HideInInspector]
    public Button yes_Button;
    [HideInInspector]
    public GameObject no;
    [HideInInspector]
    public Button no_Button;

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
        yes = transform.Find("$content#Rect/$frame/buttonList/$yes#Button").gameObject;
        yes_Button = yes.GetComponent<Button>();
        no = transform.Find("$content#Rect/$frame/buttonList/$no#Button").gameObject;
        no_Button = no.GetComponent<Button>();
    }
}