using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataMgr : Singleton<DataMgr>
{
    /// <summary>
    /// ��Ҵ浵
    /// </summary>
    public PlayerData playerData;
    bool isLoaded = false;
    public GameData gameData;
    public SettingData settingData;
    /// <summary>
    /// �����ϵ
    /// </summary>
    public const int playerFactionID = 11;

    public void Load()
    {
        if (isLoaded)
        {
            return;
        }
        //�������
        playerData = new PlayerData();
        string playerDataStr = GetData(SaveField.playerData);
        if (string.IsNullOrEmpty(playerDataStr))
        {
            playerData.playerName.Value = "dehema";
            SavePlayerData();
        }
        else
        {
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(playerDataStr);
            Utility.Dump("-------------------------------�������--------------------------------");
            playerData.SetVal(dict);
        }
        InitGameData();
        //����
        settingData = JsonConvert.DeserializeObject<SettingData>(GetData(SaveField.settingData));
        if (settingData == null)
        {
            settingData = new SettingData();
            if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                settingData.language = SystemLanguage.Chinese;
            }
            else
            {
                settingData.language = SystemLanguage.English;
            }
            SaveSettingData();
        }
        AudioMgr.Ins.soundVolume = settingData.soundVolume;
        AudioMgr.Ins.musicVolume = settingData.musicVolume;
        isLoaded = true;
        //login
        Login();
    }
    public string isgameData = "1234";
    /// <summary>
    /// ��ʼ����Ϸ����
    /// </summary>
    void InitGameData()
    {
        gameData = JsonConvert.DeserializeObject<GameData>(GetData(SaveField.gameData));
        if (gameData == null)
        {
            gameData = new GameData();
        }
        else
        {
            UIMgr.Ins.windowPos = new Vector3(gameData.windowPosX, gameData.windowPosY, 0);
        }
    }

    /// <summary>
    /// ������Ϸ���� �����á�ǩ���ȣ�
    /// </summary>
    public void SaveGameData()
    {
        string data = JsonConvert.SerializeObject(gameData);
        SaveDataToJson(SaveField.gameData, data);
    }

    /// <summary>
    /// ����������ݣ�������ֵ��
    /// </summary>
    public void SavePlayerData()
    {
        string str = playerData.ToJson();
        SaveDataToJson(SaveField.playerData, str);
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void SaveSettingData()
    {
        settingData.musicVolume = AudioMgr.Ins.musicVolume;
        settingData.soundVolume = AudioMgr.Ins.soundVolume;
        SaveDataToJson(SaveField.settingData, JsonConvert.SerializeObject(settingData));
    }

    public void SaveDataToJson(string name, string data)
    {
        Utility.Log(string.Format("����{0}��{1}", name, GetSaveFilePath(name)));
        PlayerPrefs.SetString(name, data);
        //File.WriteAllText(GetSaveFilePath(name), data);
    }

    public string GetData(string name)
    {
        string path = GetSaveFilePath(name);
        //if (File.Exists(path))
        {
            if (name == SaveField.gameData)
                //isgameData = File.ReadAllText(path).Length.ToString();
                isgameData = PlayerPrefs.GetString(name);
            return PlayerPrefs.GetString(name);
            //return File.ReadAllText(path);
        }
        return string.Empty;
    }

    /// <summary>
    /// ��ҵ�½
    /// </summary>
    public void Login()
    {
    }

    /// <summary>
    /// �µĵ�¼��
    /// </summary>
    public void NewDay()
    {

    }

    /// <summary>
    /// ������������
    /// </summary>
    public void SaveAllData()
    {
        DataMgr.Ins.SaveGameData();
        DataMgr.Ins.SavePlayerData();
        DataMgr.Ins.SaveSettingData();
    }

    public string GetSaveFilePath(string _name)
    {
        string pathDir = string.Format("{0}/SaveFile/{1}", Application.dataPath, _name);
        if (!Directory.Exists(pathDir))
        {
            Debug.Log("�����ļ���" + pathDir);
            Directory.CreateDirectory(pathDir);
        }
        return pathDir + ".json";
    }
}

public class SaveField
{
    public const string playerData = "playerData";
    public const string gameData = "gameData";
    public const string settingData = "settingData";
}