using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͼ��ʶ������
/// </summary>
public class ActionIdentifyData
{
    public string text;
    public ExpressCondition expressCondition = ExpressCondition.contain;
    public ExpressType expressType = ExpressType.Exit;
    public string targetActionUUID;
    [NonSerialized]
    public ActionData targetActionData;
}

public enum ExpressType
{
    /// <summary>
    /// ����
    /// </summary>
    Exit,
    /// <summary>
    /// ��ת��ĳ������
    /// </summary>
    MoveTargetAction,
}

/// <summary>
/// ���ʽ����
/// </summary>
public enum ExpressCondition
{
    /// <summary>
    /// ����
    /// </summary>
    contain,
    /// <summary>
    /// ������
    /// </summary>
    exclusive
}