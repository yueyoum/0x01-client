using UnityEngine;

namespace Protocol.Implement
{
    public static class TimeSync
    {
        public static void Process(Protocol.Define.TimeSync msg)
        {
            // Logic here
            int lag = (int)(TimeManager.GetInstance().TimestampInMilliSeconds - msg.client);

            TimeManager tm = TimeManager.GetInstance();
            tm.ServerUTCMilliSeconds = msg.server + lag;
        }
    }
}
