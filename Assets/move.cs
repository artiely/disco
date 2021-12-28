using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMove = false;
    private Vector3 _pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
        if (isMove)
        {
            
            //MoveObject( transform.position, new Vector3(localx, 0, localz), Random.Range(5f, 10f));

            //transform.Translate(new Vector3(localx, 0, localz) * Time.deltaTime, Space.World);

            transform.position = Vector3.MoveTowards(transform.position, _pos, 0.05f);
            transform.LookAt(Camera.main.transform.position);
        }
    }
    void startMove(Vector3 pos)
    {
        Debug.Log("收到了移动的信息");
        isMove = true;
        _pos = pos;

        //float localx = Random.Range(-10f, 10f);
        //float localz = Random.Range(-15f, 10f);
        //MoveObject(transform.position, new Vector3(localx, 0, localz), Random.Range(5f, 10f));
    }
    void stopMove()
    {
        isMove = false;
    }
    //在time时间内移动物体
    private IEnumerator MoveObject( Vector3 startPos, Vector3 endPos, float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
    }
}
