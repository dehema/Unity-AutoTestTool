using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图文识别数据
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
    /// 跳出
    /// </summary>
    Exit,
    /// <summary>
    /// 跳转到某个动作
    /// </summary>
    MoveTargetAction,
}

/// <summary>
/// 表达式条件
/// </summary>
public enum ExpressCondition
{
    /// <summary>
    /// 包含
    /// </summary>
    contain,
    /// <summary>
    /// 不包含
    /// </summary>
    exclusive
}