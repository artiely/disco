using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clone : MonoBehaviour
{
    public GameObject dog;
    public int count = 8;
    // Start is called before the first frame update
    void Start()
    {
        int offset = 5;
        for(int i = 0; i < count; i++)
        {
            GameObject cloneDog = GameObject.Instantiate(dog);
            cloneDog.transform.position = new Vector3(i*offset,0,3.0f);
            cloneDog.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
