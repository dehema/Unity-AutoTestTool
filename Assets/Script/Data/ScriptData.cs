using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptData
{
    public string name;
    /// <summary>
    /// ѭ������
    /// </summary>
    public int loopCount = 1;
    /// <summary>
    /// ѭ��������룩
    /// </summary>
    public int loopInterval = 0;
    /// <summary>
    /// ����ʱ����ΨһID ����IO����
    /// </summary>
    public string uuid;
    /// <summary>
    /// �����б�
    /// </summary>
    public List<ActionData> actionList = new List<ActionData>();

    public static ScriptData CreateNew()
    {
        ScriptData scriptData = new ScriptData();
        scriptData.name = "�½ű�";
        scriptData.uuid = DateTime.Now.ToString("yyyyMMddhhmmssff");
        return scriptData;
    }
}
