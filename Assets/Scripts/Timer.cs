using UnityEngine;
using System.Collections;

public class Timer
{

    private static float timeSyncInterval = 2.0f;
    private float timeSyncPassedTime = 0f;

    private static Timer instance = null;
    public static Timer GetInstance()
    {
        if (instance == null)
        {
            instance = new Timer();
        }
        return instance;
    }

    private Timer()
    {

    }


    public void Update()
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
