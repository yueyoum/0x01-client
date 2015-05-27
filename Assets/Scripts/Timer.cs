using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

    private static readonly float interval = 2f;
    private float passedTime;

    // Use this for initialization
    void Start()
    {
        passedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if(passedTime >= interval)
        {
            //System.DateTime now = System.DateTime.Now;
            //Debug.Log("INTERVAL..." + now.ToString("yyyy-MM-dd HH:mm:ss"));
            //PlayerManager.GetInstance().Update();
            passedTime = 0;
        }
    }
}
