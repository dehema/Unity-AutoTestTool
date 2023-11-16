using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ActionMouseClickView : BaseView
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
    public GameObject clickScreen;
    [HideInInspector]
    public Button clickScreen_Button;
    [HideInInspector]
    public GameObject img1;
    [HideInInspector]
    public RectTransform img1_Rect;
    [HideInInspector]
    public GameObject img2;
    [HideInInspector]
    public RectTransform img2_Rect;

    internal override void _LoadUI()    
    {
        base._LoadUI();
        bg = transform.Find("$bg#Image,Button").gameObject;
        bg_Image = bg.GetComponent<Image>();
        bg_Button = bg.GetComponent<Button>();
        content = transform.Find("$content#Rect").gameObject;
        content_Rect = content.GetComponent<RectTransform>();
        clickScreen = transform.Find("$content#Rect/$clickScreen#Button").gameObject;
        clickScreen_Button = clickScreen.GetComponent<Button>();
        img1 = transform.Find("$content#Rect/$img1#Rect").gameObject;
        img1_Rect = img1.GetComponent<RectTransform>();
        img2 = transform.Find("$content#Rect/$img2#Rect").gameObject;
        img2_Rect = img2.GetComponent<RectTransform>();
    }
}