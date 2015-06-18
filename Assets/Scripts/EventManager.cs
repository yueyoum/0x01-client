using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class EventManger
{

    private static EventManger instance = null;

    public delegate void OnConnectionMadeHandler();
    public static event OnConnectionMadeHandler OnConnectionMade = null;

    public delegate void OnConnectionLostHandler();
    public static event OnConnectionLostHandler OnConnectionLost = null;

    public delegate void OnSceneLoadHandler();
    public static event OnSceneLoadHandler OnSceneLoad = null;

    private EventManger()
    {
        OnConnectionMade += HandleConnectionMade;
        OnConnectionLost += HandleConnectionLost;
    }

    public static EventManger GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManger();
        }

        return instance;
    }



    # region Event API
    public void TrigConnectionMade()
    {
        OnConnectionMade();
    }

    public void TrigConnectionLost()
    {
        OnConnectionLost();
    }


    public void TrigSceneLoad()
    {
        Debug.Log("EventManager: TrigSceneLoad");
        OnSceneLoad();
    }

    #endregion


    # region Event Handler. Some Handler Implemented in Other Classes.

    private void HandleConnectionMade()
    {
        Debug.Log("Connection Made...");
        // time sync
        var timemsg = new Protocol.Define.TimeSync();
        timemsg.client = TimeManager.GetInstance().TimestampInMilliSeconds;
        timemsg.server = 0;

        Transport.GetInstance().Send(Protocol.ProtocolHandler.PackWithId(timemsg));
    }

    private void HandleConnectionLost()
    {
        Debug.Log("Connection Lost...");
    }

    # endregion
}