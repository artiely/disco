using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class danmu : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    void Start()
    {
        // 获取到当前的弹幕文字
        string danmu = GetComponent<Text>().text;
        // 计算文字的长度
        int width = CalculateLengthOfText(danmu);
        // 设置文字UI的宽度 避免文字被截取
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
        // TODO 设置背景的宽度
        
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
            Debug.Log("advance:" + info.advance);
        }
        return totalLength;

    }
}
