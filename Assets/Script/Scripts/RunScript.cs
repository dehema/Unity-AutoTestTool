using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
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
    /// 鼠标事件
    /// </summary>
    /// <param name="flags">事件类型</param>
    /// <param name="dx">x坐标值(0~65535)</param>
    /// <param name="dy">y坐标值(0~65535)</param>
    /// <param name="data">滚动值(120一个单位)</param>
    /// <param name="extraInfo">不支持</param>
    [DllImport("user32.dll")]
    static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

    public void Launch(ScriptData _currScriptData)
    {
        currScriptData = _currScriptData;
        logFilePath = Utility.GetLogFilePath(currScriptData);
        imgDirPath = Utility.GetImgDirPath(currScriptData);
        Debug.Log("创建日志文件：" + logFilePath);
        StartCoroutine(ScriptCoroutine());
    }

    public void Stop()
    {
        StopCoroutine(ScriptCoroutine());
    }

    //循环次数
    public int loopCount = 0;
    //动作索引
    public int actionIndex = 0;
    IEnumerator ScriptCoroutine()
    {
        loopCount = 0;
        actionIndex = 0;
        bool forceExit = false;
        bool isExpressionRight = false;
        Log("脚本开始运行");
        yield return new WaitForSeconds(1);
        while (loopCount < currScriptData.loopCount)
        {
            Log("开始第{0}次运行", loopCount + 1);
            actionIndex = 0;
            while (actionIndex < currScriptData.actionList.Count)
            {
                Log("开始{0}个动作", actionIndex + 1);
                ActionData currActionData = currScriptData.actionList[actionIndex];
                do
                {
                    isExpressionRight = false;
                    switch (currActionData.actionType)
                    {
                        case ActionType.Wait:
                            //等待一段时间
                            {
                                int time = int.Parse(currActionData.val);
                                yield return new WaitForSeconds(time);
                                break;
                            }
                        case ActionType.MouseClick:
                            //鼠标点击
                            {
                                ActionMouseClickData data = JsonConvert.DeserializeObject<ActionMouseClickData>(currActionData.val);
                                Log("鼠标点击({0},{1})", (int)data.mousePosX, (int)data.mousePosY);
                                //ActionMouseClick.SetCursorPos((int)data.mousePosX, (int)data.mousePosY);
                                int dx = (int)(data.mousePosX / Screen.width * 65535);
                                int dy = (int)((Screen.height - data.mousePosY) / Screen.height * 65536);
                                yield return new WaitForSeconds(0.1f);
                                //mouse_event(MouseEventFlag.Move | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0));
                                //yield return new WaitForSeconds(0.3f);
                                //mouse_event(MouseEventFlag.Move | MouseEventFlag.XDown, 2, 0, 0, new UIntPtr(0));
                                //yield return new WaitForSeconds(0.3f);
                                //mouse_event(MouseEventFlag.Move | MouseEventFlag.XDown, 0, 2, 0, new UIntPtr(0));
                                //yield return new WaitForSeconds(0.5f);
                                mouse_event(MouseEventFlag.Move | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0));
                                mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0));
                                yield return new WaitForSeconds(0.1f);
                                mouse_event(MouseEventFlag.LeftUp | MouseEventFlag.Absolute, dx, dy, 0, new UIntPtr(0));
                                break;
                            }
                        case ActionType.ScreenCapture:
                            //截屏
                            {
                                string path = CaptureScreen();
                                ActionIdentifyText.Ins.IdentifyImage(path);
                                break;
                            }
                        case ActionType.ScreenCaptureAndIdentifyText:
                            //截屏识别文字
                            {
                                string path = CaptureScreen();
                                List<string> words = ActionIdentifyText.Ins.IdentifyImage(path);
                                ActionIdentifyData data = JsonConvert.DeserializeObject<ActionIdentifyData>(currActionData.val);
                                if (data.expressCondition == ExpressCondition.contain)
                                {
                                    Log("开始判断是否包含{0}", data.text);
                                    foreach (var word in words)
                                    {
                                        if (word.Contains(data.text))
                                        {
                                            isExpressionRight = true;
                                            break;
                                        }
                                    }
                                }
                                else if (data.expressCondition == ExpressCondition.exclusive)
                                {
                                    Log("开始判断是否不包含{0}", data.text);
                                    isExpressionRight = true;
                                    foreach (var word in words)
                                    {
                                        if (word.Contains(data.text))
                                        {
                                            isExpressionRight = false;
                                            break;
                                        }
                                    }
                                }
                                if (isExpressionRight)
                                {
                                    Log("符合条件");
                                    if (data.expressType == ExpressType.Exit)
                                    {
                                        Log("满足判断条件，开始跳出");
                                        forceExit = true;
                                        break;
                                    }
                                    else if (data.expressType == ExpressType.MoveTargetAction)
                                    {
                                        ActionData actionData = ActionMgr.Ins.GetActionDataByUUID(data.targetActionUUID, currScriptData);
                                        if (actionData != null)
                                        {
                                            Log("跳转到{0}", actionData.name);
                                            actionIndex = actionData.index;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Log("不满足判断条件，继续下一个动作");
                                }
                                break;
                            }
                    }
                    if (actionIndex < currScriptData.actionList.Count && currActionData.isLoop && !forceExit && !isExpressionRight)
                    {
                        //动作的等待时间
                        yield return new WaitForSeconds(currActionData.interval);
                    }
                } while (currActionData.isLoop && !forceExit && !isExpressionRight);
                //完成动作
                if (forceExit)
                {
                    Log("强制跳出本次循环");
                    break;
                }
                if (isExpressionRight)
                {
                    //表达式成立 跳到指定动作
                    continue;
                }
                actionIndex++;
                if (actionIndex < currScriptData.loopCount)
                {
                    yield return new WaitForSeconds(currScriptData.loopInterval);
                }
            }
            //完成一次循环
            loopCount++;
            Log("结束第{0}次运行", loopCount + 1);
            Log("脚本运行结束");
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
        /// 捕获屏幕
        /// </summary>
        string CaptureScreen()
        {
            string path = imgDirPath + "/" + DateTime.Now.ToString("hhmmss") + ".png";
            try
            {
                Log("保存截图至{0}", path);
                ScreenCapturnFramework.ScreenCapture.PrintScreen(path);
            }
            catch (Exception e)
            {
                Log("截图失败:{0}", e.ToString());
                throw;
            }
            return path;
        }
    }
}