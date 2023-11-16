using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Baidu.Aip.Ocr;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

public class ActionIdentifyText : Singleton<ActionIdentifyText>
{
    const string apiKey = "g4s2xunbWw1EPLcRisA0iCRG";
    const string secretKey = "aNpbywQNUyl6mOl2CYUpvhu0Bttvd6Ao";

    private Ocr ocr;

    void InitOcr()
    {
        if (ocr == null)
        {
            ocr = new Ocr(apiKey, secretKey);
        }
    }

    public List<string> IdentifyImage(string imgPath)
    {
        InitOcr();
        byte[] bytes = File.ReadAllBytes(imgPath);
        var options = new Dictionary<string, object>
        {
            {"language_type", "CHN_ENG"},
            {"detect_direction", "true"},
            {"detect_language", "true"},
        };
        JObject result = ocr.GeneralBasic(bytes, options);
        List<string> wordList = new List<string>();
        if (result.ContainsKey("words_result"))
        {
            var wordsResult = result["words_result"] as JArray;
            foreach (var word in wordsResult)
            {
                wordList.Add(word["words"].ToString());
            }
        }
        Debug.Log(result);
        return wordList;
    }
}