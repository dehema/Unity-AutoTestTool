using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Drawing;
using static SettingField;
#if UNITY_EDITOR
using UnityEditor;
#endif
//using ScreenCaptureDemo;

public class RunScript : MonoBehaviour
{
    //data
    ScriptData currScriptData;
    string logFilePath;
    string imgDirPath;
    public Action<string> RefreshTips;
    public Action OnComplete;

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="flags">�¼�����</param>
    /// <param name="dx">x����ֵ(0~65535)</param>
    /// <param name="dy">y����ֵ(0~65535)</param>
    /// <param name="data">����ֵ(120һ����λ)</param>
    /// <param name="extraInfo">��֧��</param>
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    public void Launch(ScriptData _currScriptData)
    {
        currScriptData = _currScriptData;
        logFilePath = Utility.GetLogFilePath(currScriptData);
        imgDirPath = Utility.GetImgDirPath(currScriptData);
        Debug.Log("������־�ļ���" + logFilePath);
        StartCoroutine(ScriptCoroutine());
    }

    public void Stop()
    {
        StopCoroutine(ScriptCoroutine());
    }

    //ѭ������
    public int loopCount = 0;
    //��������
    public int actionIndex = 0;
    IEnumerator ScriptCoroutine()
    {
        yield return new WaitForEndOfFrame();
        loopCount = 0;
        bool forceExit = false;
        Log("�ű���ʼ����");
        while (loopCount < currScriptData.loopCount)
        {
            Log("��ʼ��{0}������", loopCount + 1);
            actionIndex = 0;
            while (actionIndex < currScriptData.actionList.Count)
            {
                Log("��ʼ{0}������", actionIndex + 1);
                ActionData currActionData = currScriptData.actionList[actionIndex];
                while (currActionData.isLoop)
                {
                    switch (currActionData.actionType)
                    {
                        case ActionType.Wait:
                            //�ȴ�һ��ʱ��
                            {
                                int time = int.Parse(currActionData.val);
                                yield return new WaitForSeconds(time);
                                break;
                            }
                        case ActionType.MouseClick:
                            //�����
                            {
                                ActionMouseClickData data = JsonConvert.DeserializeObject<ActionMouseClickData>(currActionData.val);
                                Log("�����({0},{1})", (int)data.mousePosX, (int)data.mousePosY);
                                //ActionMouseClick.SetCursorPos((int)data.mousePosX, (int)data.mousePosY);
                                int dx = (int)(data.mousePosX / Screen.width * 65535);
                                int dy = (int)((Screen.height - data.mousePosY) / Screen.height * 65536);
                                mouse_event(MouseEventFlag.Move | MouseEventFlag.LeftDown | MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0));
                                break;
                            }
                        case ActionType.ScreenCapture:
                            //����
                            {
                                string path = CaptureScreen();
                                ActionIdentifyText.Ins.IdentifyImage(path);
                                break;
                            }
                        case ActionType.ScreenCaptureAndIdentifyText:
                            //����ʶ������
                            {
                                string path = CaptureScreen();
                                List<string> words = ActionIdentifyText.Ins.IdentifyImage(path);
                                ActionIdentifyData data = JsonConvert.DeserializeObject<ActionIdentifyData>(currActionData.val);
                                bool isRight = false;
                                if (data.expressCondition == ExpressCondition.contain)
                                {
                                    Log("��ʼ�ж��Ƿ����{0}", data.text);
                                    foreach (var word in words)
                                    {
                                        if (word.Contains(data.text))
                                        {
                                            isRight = true;
                                            break;
                                        }
                                    }
                                }
                                else if (data.expressCondition == ExpressCondition.exclusive)
                                {
                                    Log("��ʼ�ж��Ƿ񲻰���{0}", data.text);
                                    isRight = true;
                                    foreach (var word in words)
                                    {
                                        if (word.Contains(data.text))
                                        {
                                            isRight = false;
                                            break;
                                        }
                                    }
                                }
                                if (isRight)
                                {
                                    Log("��������");
                                    if (data.expressType == ExpressType.Exit)
                                    {
                                        Log("�����ж���������ʼ����");
                                        forceExit = true;
                                        break;
                                    }
                                    else if (data.expressType == ExpressType.MoveTargetAction)
                                    {
                                        ActionData actionData = ActionMgr.Ins.GetActionDataByUUID(data.targetActionUUID, currScriptData);
                                        if (actionData != null)
                                        {
                                            Log("��ת��{0}", actionData.name);
                                            actionIndex = actionData.index;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Log("�������ж�������������һ������");
                                }
                                break;
                            }
                    }
                    if (forceExit)
                    {
                        break;
                    }
                    if (currActionData.isLoop)
                    {
                        yield return new WaitForSeconds(currActionData.interval);
                    }
                }
                actionIndex++;
                if (forceExit)
                {
                    Log("ǿ����������ѭ��");
                    break;
                }
            }
            if (actionIndex < currScriptData.actionList.Count)
            {
                yield return new WaitForSeconds(currScriptData.loopInterval);
            }
            Log("������{0}������", loopCount + 1);
            loopCount++;
        }
        Log("�ű����н���");
        Log("");
        OnComplete();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    void Log(string str, params object[] args)
    {
        string content = string.Format(str, args);
        RefreshTips(content);
        content = DateTime.Now.ToString("[hh:mm:ss]") + content + "\r\n";
        File.AppendAllText(logFilePath, content);
        Debug.Log(content);
    }

    /// <summary>
    /// ������Ļ
    /// </summary>
    string CaptureScreen()
    {
        string path = imgDirPath + "/" + DateTime.Now.ToString("hhmmss") + ".png";
        try
        {
            Log("�����ͼ��{0}", path);
            ScreenCapturnFramework.ScreenCapture.PrintScreen(path);
        }
        catch (Exception e)
        {
            Log("��ͼʧ��:{0}", e.ToString());
            throw;
        }
        return path;
    }
}