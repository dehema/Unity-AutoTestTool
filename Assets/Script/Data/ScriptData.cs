using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptData
{
    public string name;
    /// <summary>
    /// 循环次数
    /// </summary>
    public int loopCount = 1;
    /// <summary>
    /// 循环间隔（秒）
    /// </summary>
    public int loopInterval = 0;
    /// <summary>
    /// 创建时生成唯一ID 用作IO操作
    /// </summary>
    public string uuid;
    /// <summary>
    /// 动作列表
    /// </summary>
    public List<ActionData> actionList = new List<ActionData>();

    public static ScriptData CreateNew()
    {
        ScriptData scriptData = new ScriptData();
        scriptData.name = "新脚本";
        scriptData.uuid = DateTime.Now.ToString("yyyyMMddhhmmssff");
        return scriptData;
    }
}
