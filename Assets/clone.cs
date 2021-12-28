using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using LitJson;
using System.Linq;
using UnityEngine.UI;
using Spine.Unity;

public class clone : MonoBehaviour
{
    // 当前文字
    private Text text;
    // 获取文字背景
    public Transform bg;
    public GameObject dog;
    public int count = 100;
    // 获取弹幕组件 下面可调用该组件脚本里的方法
    //public Component danmuCs;
    // Start is called before the first frame update
    private int lastId = 0;
    // 储存所有的狗
    private List<GameObject> dogs = new List<GameObject>();
    void Start()
    {
        text = GameObject.Find("Canvas/danmu").GetComponent<Text>();
        // 查找相机
        // Camera cam = (Camera)FindObjectOfType(typeof(Camera));
        for (int i = 0; i < count; i++)
        {
            // 随机数
            float localx = Random.Range(-10f, 10f);
            float localz = Random.Range(-15f, 10f);
            // 克隆狗 赋值位置 并隐藏
            GameObject cloneDog = GameObject.Instantiate(dog);
            cloneDog.name = "" + i;
            cloneDog.transform.position = new Vector3(localx, 0, localz);
            //GameObject root = GameObject.Find("clone");
            //GameObject danmu = cloneDog.transform.Find("danmu").gameObject;
            //GameObject danmubg = cloneDog.transform.Find("danmubg").gameObject;
            //danmubg.SetActive(false);
            //danmu.SetActive(false);
            //FIXME: 修复引用错误的问题 不能直接向如下操作
            //Transform dm = cloneDog.transform.Find("danmu");
            //dm.GetComponent<Text>().text =""+ Random.Range(0f, 100f);

            GameObject.Find("Canvas/danmu").GetComponent<Text>().text = "" + Random.Range(0f, 100f);
            //Debug.Log("<<<<<<<<<<<<<<<<<" + GameObject.Find("Canvas/danmu").GetComponent<Text>().text);
            cloneDog.transform.Find("Canvas").gameObject.SetActive(false);
            cloneDog.transform.Find("dogspine").GetComponent<SkeletonAnimation>().state.AddAnimation(0, "dog" + Random.Range(1, 7), true, 0f);
            cloneDog.transform.LookAt(Camera.main.transform.position);
            cloneDog.SetActive(false);

            dogs.Add(cloneDog);
        }
        dog.SetActive(false);
        dogs.Add(dog);

        InvokeRepeating("getHttp", 0, 3F);
        //Debug.Log(dogs.Count);
    }
    public void getHttp()
    {
        // 必须在StartCoroutine 中传递网络请求 也是搞了半天
        StartCoroutine(GetRequest());
    }
    public static string MD5Encrypt(string strText)
    {
        MD5 md5 = MD5.Create();
        byte[] fromData = Encoding.Default.GetBytes(strText);
        byte[] targetData = md5.ComputeHash(fromData);
        StringBuilder byte2String = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
        {
            byte2String.AppendFormat("{0:x2}", targetData[i]);
        }
        return byte2String.ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GetRequest()
    {
        string key = "gaia-001";
        string secret = "b40e7bcd25c8ac18aa95bed84755ddfd5e317768";
        ///时间戳秒
        long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        string token = MD5Encrypt(key + secret + timestamp);

        string url = "https://help.yinjietd.com/spider/anchorapi/userBarrage?live_room_code=92&limiet=100&type=0&order=desc&key=" + key + "&secret=" + secret + "&timestamp=" + timestamp + "&token=" + token + "&id=" + lastId;
        UnityWebRequest www = UnityWebRequest.Get(url);
        //www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // 以文本形式显示结果
            //Debug.Log(www.downloadHandler.text);

            JsonInfo json = JsonMapper.ToObject<JsonInfo>(www.downloadHandler.text);
            //Debug.Log("UserData name:" + json.code);
            // 存在data

            responceDogs(json);
            // 或者获取二进制数据形式的结果
            //byte[] results = www.downloadHandler.data;
            //Debug.Log(results);
        }
    }
    void responceDogs(JsonInfo json)
    {
        //第一次请求 渲染全部的结果

        if (json.data.Count > 0)
        {
            if (lastId == 0)
            {

                //for (int i = 0; i < json.data.Count; i++)
                //{
                //    //随机数 随机位置
                //    float localx = Random.Range(0f, 100f);
                //    float localz = Random.Range(0f, 100f);
                //    //克隆对象并赋予名字
                //    GameObject cloneDog = GameObject.Instantiate(dog);
                //    cloneDog.name = json.data[i].nickname;
                //    //设置位置
                //    cloneDog.transform.position = new Vector3(localx, 0, localz);
                //    // 设置动画 0-6 float型，随机值涵盖： 最小和最大值 // int型，随机值涵盖：最小值，不涵盖最大值
                //    cloneDog.transform.Find("dogspine").GetComponent<SkeletonAnimation>().state.AddAnimation(0, "dog" + Random.Range(1, 7), true, 0f);
                //    // 有弹幕展示弹幕
                //    cloneDog.transform.Find("Canvas").gameObject.SetActive(false);
                //    if (json.data[i].ext != ""&&json.data[i].ext!="/img")
                //    {
                //        cloneDog.transform.Find("Canvas").gameObject.SetActive(true);
                //        //更新弹幕
                //        refreshText(json.data[i].ext, cloneDog);
                //        // 必须得已路径的形式查找，不能直接查找子级
                //        //cloneDog.transform.Find("Canvas/danmu").GetComponent<Text>().text = json.data[i].ext;
                //    }

                //    try
                //    {
                //        GameObject.Find("123")
                //    }
                //    catch
                //    {

                //    }
                //}
                for(int i = 0; i < json.data.Count; i++)
                {
                    
                    dogs[i].transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text = json.data[i].nickname;
                    dogs[i].SetActive(true);
                    if(json.data[i].ext != "" && json.data[i].ext != "/img")
                    {
                        
                        refreshText(json.data[i].ext, dogs[i]);

                    }

                }

            }
            else
            {

                lastId = json.data[0].id;
                // 获取昵称
                //Debug.Log("UserData name:" + json.data[0].nickname);

                List<Data> list = json.data;
                // 只包含弹幕的
                List<Data> onlyExt = list.Where(e => e.ext != "").ToList();
                // 打印list的内容
                displayData(onlyExt);


                //for (int i = 0; i < onlyExt.Count; i++)
                //{
                //    dogs[i].SetActive(true);
                //    // 更新弹幕
                //    refreshText(onlyExt[i].ext, dogs[i]);
                //    //更新名字
                //    dogs[i].transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text = onlyExt[i].nickname;
                //}
                //return;
                // 循环包含弹幕的狗
                for (int i = 0; i < onlyExt.Count; i++)
                {
                    // 查找当前狗的名字是否存在
                    // FindIndex  找不到会报错
                    int eq = -1;
                    try
                    {
                        eq = dogs.FindIndex(e => e.transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text == dogs[i].GetComponent<Text>().text);
                    }
                    catch
                    {
                        eq = -1;
                    }

                    //Debug.Log(eq);
                    // 狗已存在
                    if (eq != -1)
                    {
                        // 刷新弹幕
                        //dog[eq].
                        //GameObject.Find("danmu").GetComponent<danmu>().refreshText();
                        refreshText(onlyExt[i].ext, dogs[eq]);
                    }
                    else
                    {
                        // 找出还没显示的dogs; activeInHierarchy 表示当前对象setActive(false)
                        List<GameObject> enabledDogs = dogs.Where(e => e.activeInHierarchy && eq == -1).ToList();

                        if (enabledDogs.Count > 0 && i < enabledDogs.Count)
                        {
                            // 更新弹幕
                            refreshText(onlyExt[i].ext, dogs[i]);
                            //更新名字
                            dogs[i].transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text = onlyExt[i].nickname;
                        }
                        else
                        {
                            //狗已存在直接设置弹幕
                            //查看弹幕是否再展示中
                            if (dogs[i].transform.Find("Canvas").gameObject.activeInHierarchy)
                            {

                            }
                            else
                            {
                                int nameIndex = -1;
                                try
                                {

                                    nameIndex = dogs.FindIndex(e => e.transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text == dogs[i].GetComponent<Text>().text);
                                }
                                catch
                                {
                                    nameIndex = -1;
                                }

                                if (nameIndex != -1)
                                {
                                    // 更新弹幕
                                    refreshText(onlyExt[i].ext, dogs[nameIndex]);
                                    //更新名字
                                    dogs[nameIndex].transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text = onlyExt[i].nickname;

                                }
                                else
                                {
                                    //更新名字
                                    dogs[i].transform.Find("CanvasName/name").gameObject.GetComponent<Text>().text = onlyExt[i].nickname;
                                    // 更新弹幕
                                    refreshText(onlyExt[i].ext, dogs[i]);

                                }
                            }

                        }

                    }



                    //danmuCs.BroadcastMessage("addDog", onlyExt);

                    // 过滤出有弹幕的
                    //Debug.Log(typeof(list).ToString());
                    //List<Data> = list.Where(e=>e.ext!="").ToList();
                }


            }
            lastId = json.data[0].id;
            return;
        }
    }
    void displayData(List<Data> data)
    {
        string itemDisplay = "items:";
        string danmu = "";
        foreach (Data item in data)
        {
            itemDisplay += item.nickname + " ";
            danmu += item.ext + " ";
            //Debug.Log(item.ext);
            //Debug.Log("姓名" + item.nickname);
            //Debug.Log("弹幕>>>>" + item.ext);
        }
    }
    IEnumerator Timer(GameObject dog)
    {
        yield return new WaitForSeconds(Random.Range(5f, 7f));
        dog.transform.Find("Canvas/danmubg").gameObject.GetComponent<Image>().color = new Color32(91,226,104,225);
        dog.transform.Find("Canvas").gameObject.SetActive(false);

        dog.GetComponent<move>().BroadcastMessage("stopMove");
    }
    // 计算文字的宽度
    int CalculateLengthOfText(string message)
    {
        int totalLength = 0;

        int width = 0;
        
        //foreach (var item in message)
        //{

        //    //CharacterInfo info = new CharacterInfo();
        //    //text.font.GetCharacterInfo(item, out info, text.fontSize, text.fontStyle);// fontStyle不可忽略，
        //    //totalLength += info.advance;//总的字符宽度
        //}
        // 这种粗暴的方式更准确

        int len = Encoding.Default.GetByteCount(message);


        //Debug.Log("///message"+ message + totalLength+"//"+width);

        return 24*len/2;

    }
    void refreshText(string text, GameObject dog)
    {
        //Debug.Log("开始设置弹幕"+text);
        if (text == "" || text == "/img") return;

        dog.SetActive(true);
        

        string danmuText = text;
        try
        {
            GiftInfo gift = JsonMapper.ToObject<GiftInfo>(text);
            danmuText = gift.ext;
            //Debug.Log("礼物》》" + gift.ext+ danmuText);
            dog.transform.Find("Canvas/danmubg").gameObject.GetComponent<Image>().color = Color.yellow;

        }
        catch
        {

        }
        dog.transform.Find("Canvas/danmu").gameObject.GetComponent<Text>().text = danmuText;
        string danmu = dog.transform.Find("Canvas/danmu").gameObject.GetComponent<Text>().text;
        // 计算文字的长度
        int width = CalculateLengthOfText(danmu);
        if (width == 0)
        {

            Debug.Log("当前的弹幕//" + danmuText + "//计算的宽度//" + width);
        }
        if (width == 0) return;
        dog.transform.Find("Canvas").gameObject.SetActive(true);
        float localx = Random.Range(-10f, 10f);
         float localz = Random.Range(-15f, 10f);


    //dog.transform.position = Vector3.MoveTowards(dog.transform.position, new Vector3(localx,0,localz), Random.Range(5f, 10f));
    GameObject.Find(dog.name).transform.GetComponent<move>().SendMessage("startMove", new Vector3(localx, 0, localz));
        //dog.transform.GetComponent<move>().SendMessage("startMove");

        //MoveObject(dog,dog.transform.position, new Vector3(localx, 0, localz), Random.Range(5f, 10f));
        //使狗狗移动

        // 设置文字UI的宽度 避免文字被截取
        dog.transform.Find("Canvas/danmu").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30);
        // 设置背景的宽度
        dog.transform.Find("Canvas/danmubg").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, 60);

        StartCoroutine(Timer(dog));

    }
    
}

public class JsonInfo
{
    public int code;
    public List<Data> data;
}
public class GiftInfo
{
    public string type;
    public string ext;
}
public class Data
{
    public string nickname;
    // 弹幕信息
    public string ext;
    public int id;
}