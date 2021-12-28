using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class danmu : MonoBehaviour
{
    // 当前文字
    public Text text;
    // 获取文字背景
    public Transform bg;
    void Start()
    {
        // 获取到当前的弹幕文字
       
        //string danmu = GetComponent<Text>().text;
        //// 计算文字的长度
        //int width = CalculateLengthOfText(danmu);
        //// 设置文字UI的宽度 避免文字被截取
        //gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
        //// 设置背景的宽度
        //bg.GetComponent<RectTransform>().sizeDelta = new Vector2(width+40, 60);
        refreshText(GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 计算文字的宽度
    int CalculateLengthOfText(string message)
    {
        int totalLength = 0;
        foreach (var item in message)
        {

            CharacterInfo info = new CharacterInfo();
            text.font.GetCharacterInfo(item, out info, text.fontSize,text.fontStyle);// fontStyle不可忽略，网上的博客害人
            totalLength += info.advance;//总的字符宽度
        }
        return totalLength;

    }
    void refreshText(string text)
    {
        GetComponent<Text>().text = text;
        string danmu = GetComponent<Text>().text;
        // 计算文字的长度
        int width = CalculateLengthOfText(danmu);
        // 设置文字UI的宽度 避免文字被截取
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
        // 设置背景的宽度
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, 60);

    }
    void addDog(List<Data> list)
    {
        Debug.Log("已运行");
    }
}
