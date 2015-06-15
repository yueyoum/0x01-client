using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

    private static float timeSyncInterval = 2.0f;
    private float timeSyncPassedTime = 0f;

    // Use this for initialization
    void Start()
    {
        timeSyncPassedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Transport.GetInstance().IsOpen)
        {
            timeSyncPassedTime += Time.deltaTime;
            if (timeSyncPassedTime >= timeSyncInterval)
            {
                Protocol.Define.TimeSync msg = new Protocol.Define.TimeSync();
                msg.client = TimeManager.GetInstance().TimestampInMilliSeconds;
                msg.server = 0;
                Transport.GetInstance().Send(Protocol.ProtocolHandler.PackWithId(msg));

                timeSyncPassedTime = 0;
            }
        }

    }
}
