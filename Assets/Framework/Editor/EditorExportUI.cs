using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

using UnityEngine;

public class EditorExportUI : EditorWindow
{
    static GameObject uiPrefab;
    /// <summary>
    /// �����ĵ�����ͼ�ű�
    /// </summary>
    static bool isExportView;
    /// <summary>
    /// ������ͼ��UI
    /// </summary>
    public static void ExportViewUI(GameObject _uiPrefab = null)
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("����", "��������ʱ���ܵ���", "ok");
            return;
        }
        uiPrefab = _uiPrefab;
        UnityEditor.SceneManagement.PrefabStage stage = null;
        if (uiPrefab == null)
        {
            stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (!stage)
            {
                EditorUtility.DisplayDialog("����", "�ȴ�һ��View��Ԥ���壬����Ԥ����ģʽ", "ok");
                return;
            }
        }
        uiPrefab = uiPrefab ?? stage.prefabContentsRoot;
        BaseView baseView = uiPrefab.GetComponent<BaseView>();
        BaseUI baseUI = uiPrefab.GetComponent<BaseUI>();
        isExportView = baseView != null;
        string viewName = uiPrefab.name;
        //Debug.Log(viewName, root);
        List<Transform> allRoot = ForeachRoot(uiPrefab.transform);
        List<Transform> tfList = GetRegularRoot(allRoot);
        string scriptContent = GetUIScriptContent(tfList);
        string uiModelContent = GetUIModelContent(tfList);
        string folderPath = string.Empty;
        //��������
        string superClassName = isExportView ? "BaseView" : "BaseUI";
        //��ȡView�ű�·��
        if (baseView != null)
        {
            superClassName = baseView.GetType().BaseType.ToString();
        }
        else if (baseUI != null)
        {
            superClassName = baseUI.GetType().BaseType.ToString();
        }
        foreach (var item in AssetDatabase.FindAssets(uiPrefab.name + " t:Script"))
        {
            string scriptPath = AssetDatabase.GUIDToAssetPath(item);
            if (scriptPath.Contains("/" + viewName + ".cs"))
            {
                folderPath = scriptPath.Replace(viewName + ".cs", string.Empty);
                break;
            }
        }
        //else
        //{
        //    folderPath = $"{Application.dataPath}/Script/Item/";
        //}
        if (string.IsNullOrEmpty(folderPath))
        {
            EditorUtility.DisplayDialog("����", "�Ҳ�����ViewԤ����ͬ���ű�", "ok");
            return;
        }
        string tempViewUIContent = string.Empty;
        if (isExportView)
        {
            tempViewUIContent = File.ReadAllText(GetExportViewScriptTemplatePath());
        }
        else if (superClassName == "PoolItemBase")
        {
            tempViewUIContent = File.ReadAllText(GetExportPoolItemBaseTemplatePath());
        }
        else
        {
            tempViewUIContent = File.ReadAllText(GetExportUIWidgetTemplatePath());
        }
        tempViewUIContent = tempViewUIContent.Replace("#ScriptName#", viewName);
        scriptContent = tempViewUIContent.Replace("#Content#", scriptContent);
        scriptContent = scriptContent.Replace("#UIModelContent#", uiModelContent);
        scriptContent = scriptContent.Replace("#Superclass#", superClassName);
        string viewUIPath = folderPath + viewName + "_UI.cs";
        File.WriteAllText(viewUIPath, scriptContent);
        AssetDatabase.Refresh();
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(viewUIPath));
        Debug.Log($"����{viewUIPath}");
    }

    /// <summary>
    /// ��ȡ�Զ�����View������UIģ��ű�
    /// </summary>
    /// <returns></returns>
    public static string GetExportViewScriptTemplatePath()
    {
        return Application.dataPath + "/Framework/Script/View/ExportViewScriptTemplate.txt";
    }

    /// <summary>
    /// ��ȡ�Զ�����Prefab������UIģ��ű�
    /// </summary>
    /// <returns></returns>
    public static string GetExportUIWidgetTemplatePath()
    {
        return Application.dataPath + "/Framework/Script/View/ExportUIWidgetTemplate.txt";
    }

    /// <summary>
    /// ��ȡ�Զ����������Item������UIģ��ű�
    /// </summary>
    /// <returns></returns>
    public static string GetExportPoolItemBaseTemplatePath()
    {
        return Application.dataPath + "/Framework/Script/View/ExportPoolItemBaseTemplate.txt";
    }

    /// <summary>
    /// ��ȡ���з��Ϲ����UI
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetRegularRoot(List<Transform> _tfList)
    {
        List<Transform> tfList = new List<Transform>();
        foreach (var item in _tfList)
        {
            string name = item.name;
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }
            if (!name.StartsWith("$"))
            {
                continue;
            }
            tfList.Add(item);
        }
        return tfList;
    }

    /// <summary>
    /// ��ȡViewUI�ű�������
    /// </summary>
    /// <param name="_tfList"></param>
    /// <returns></returns>
    public static string GetUIScriptContent(List<Transform> _tfList)
    {
        StringBuilder scriptStr = new StringBuilder();
        foreach (var item in _tfList)
        {
            string name = item.name;
            string goName = name.Replace("$", string.Empty).Split('#')[0];
            //����ĳ���ڵ�
            string rootPath = GetRootFullPath(item.transform);
            scriptStr.Append($"        {goName} = transform.Find(\"{rootPath}\").gameObject;\n");
            if (name.Contains("#"))
            {
                string components = name.Replace("$", string.Empty).Split('#')[1];
                foreach (var componentName in components.Split(','))
                {
                    string componentFullName = GetComponentFullName(componentName);
                    if (item.GetComponent(componentFullName) == null)
                    {
                        EditorGUIUtility.PingObject(item);
                        EditorUtility.DisplayDialog("����", $"����{goName}�Ҳ������{componentFullName}", "ok");
                    }
                    else
                    {
                        scriptStr.Append($"        {goName + "_" + componentName} = {goName}.GetComponent<{componentFullName}>();\n");
                        //Debug.Log(component, componentRoot);
                    }
                }
            }
        }
        //Debug.Log(scriptStr.ToString());
        return scriptStr.ToString();
    }

    /// <summary>
    /// ��ȡ��ͼUI��ģ��������
    /// </summary>
    /// <param name="_tfList"></param>
    /// <returns></returns>
    public static string GetUIModelContent(List<Transform> _tfList)
    {
        StringBuilder scriptStr = new StringBuilder();
        foreach (var item in _tfList)
        {
            string name = item.name;
            string goName = name.Replace("$", string.Empty).Split('#')[0];
            //����ĳ���ڵ�
            scriptStr.Append($"    [HideInInspector]\n");
            scriptStr.Append($"    public GameObject {goName};\n");
            if (name.Contains("#"))
            {
                string components = name.Replace("$", string.Empty).Split('#')[1];
                foreach (var componentName in components.Split(','))
                {
                    string componentFullName = GetComponentFullName(componentName);
                    if (item.GetComponent(componentFullName) != null)
                    {
                        scriptStr.Append($"    [HideInInspector]\n");
                        scriptStr.Append($"    public {componentFullName} {goName + "_" + componentName};\n");
                        //Debug.Log(component, componentRoot);
                    }
                }
            }
        }
        return scriptStr.ToString();
    }

    /// <summary>
    /// ��ȡ������
    /// </summary>
    /// <returns></returns>
    public static string GetComponentFullName(string _name)
    {
        string name = _name;
        switch (name)
        {
            case "Rect":
                name = "RectTransform";
                break;
            case "Hor":
                name = "HorizontalLayoutGroup";
                break;
            case "Ver":
                name = "VerticalLayoutGroup";
                break;
        }
        return name;
    }

    /// <summary>
    /// ��ȡһ���ڵ��ȫ��·��
    /// </summary>
    /// <param name="_trans"></param>
    /// <returns></returns>
    public static string GetRootFullPath(Transform _trans)
    {
        string rootPath = _trans.gameObject.name;
        Transform rootParent = _trans.transform.parent;
        while (rootParent != null && rootParent.parent != null)
        {
            rootPath = rootParent.gameObject.name + "/" + rootPath;
            rootParent = rootParent.parent;
        }
        if (!isExportView)
        {
            int rootLength = uiPrefab.name.Length + 1;
            rootPath = rootPath.Substring(rootLength, rootPath.Length - rootLength);
        }
        return rootPath;
    }

    /// <summary>
    /// �ݹ����ҳ������нڵ�
    /// </summary>
    /// <param name="_root"></param>
    /// <param name="_tfList"></param>
    /// <returns></returns>
    public static List<Transform> ForeachRoot(Transform _root, List<Transform> _tfList = null)
    {
        List<Transform> tfList = _tfList ?? new List<Transform>();
        foreach (Transform child in _root)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(child.gameObject))
            {
                tfList.Add(child);
                continue;
            }
            tfList.Add(child);
            tfList = ForeachRoot(child, tfList);
        }
        return tfList;
    }
}
