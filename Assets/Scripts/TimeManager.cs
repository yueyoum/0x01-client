using UnityEngine;
using System.Collections.Generic;


public class TimeManager
{
    private long serverUTCMilliSeconds;
    private System.DateTime date1970;

    private int lag;
    private int timeDiffWithServer;



    public long Timestamp
    {
        get
        {
            double seconds = (System.DateTime.UtcNow - date1970).TotalSeconds;
            return (long)seconds;
        }
    }

    public long TimestampInMilliSeconds
    {
        get
        {
            double milliseconds = (System.DateTime.UtcNow - date1970).TotalMilliseconds;
            return (long)milliseconds;
        }
    }


    private static TimeManager instance = null;
    private TimeManager()
    {
        date1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    }

    public static TimeManager GetInstance()
    {
        if (instance == null)
        {
            instance = new TimeManager();
        }

        return instance;
    }


    public void SyncTime(long serverTime, int roundTrip)
    {
        lag = roundTrip / 2;
        timeDiffWithServer = (int)(TimestampInMilliSeconds - serverTime - lag);
    }


    public long ServerTimeWithLag()
    {
        return TimestampInMilliSeconds - timeDiffWithServer + lag;
    }

    public long ServerTime()
    {
        return TimestampInMilliSeconds - timeDiffWithServer;
    }

}