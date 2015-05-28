using UnityEngine;
using System.Collections.Generic;


public class TimeManager
{
    public long ServerUTCMilliSeconds
    {
        get
        {
            return serverUTCMilliSeconds;
        }

        set
        {
            TimeDiffWithServer = (int)(TimestampInMilliSeconds - value);
            serverUTCMilliSeconds = value;
        }
    }

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

    // local time - server time
    public int TimeDiffWithServer { get; private set; }

    private long serverUTCMilliSeconds;
    private System.DateTime date1970;

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

    public long LocalToServerTime()
    {
        return TimestampInMilliSeconds - TimeDiffWithServer;
    }

    public long ServerToLocalTime(long ServerTime)
    {
        return ServerTime + TimeDiffWithServer;
    }

}