using UnityEngine;

namespace Protocol.Implement
{
    public static class TimeSync
    {
        public static void Process(Protocol.Define.TimeSync msg)
        {
            // Logic here
            int roundTrip = (int)(TimeManager.GetInstance().TimestampInMilliSeconds - msg.client);

            TimeManager tm = TimeManager.GetInstance();
            tm.SyncTime(msg.server, roundTrip);
        }
    }
}
