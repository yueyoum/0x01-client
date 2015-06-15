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

    public delegate void OnSimpleTapHandler(Vector3 touchPosition);
    public static event OnSimpleTapHandler OnSimpleTap = null;

    public delegate void OnUnitScoreChangeHandler(PlayerScript ps);
    public static event OnUnitScoreChangeHandler OnUnitScoreChange = null;

    private EventManger()
    {
        OnConnectionMade += HandleConnectionMade;
        OnConnectionLost += HandleConnectionLost;

        OnSimpleTap += HandlerSimpleTap;
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

    public void TrigSimpleTap(Vector3 touchPosition)
    {
        OnSimpleTap(touchPosition);
    }


    public void TrigUnitScoreChange(PlayerScript ps)
    {
        OnUnitScoreChange(ps);
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

    private void HandlerSimpleTap(Vector3 touchPosition)
    {
        Protocol.Define.UnitMove msg = new Protocol.Define.UnitMove();
        msg.target = new Protocol.Define.Vector2();
        msg.target.x = touchPosition.x;
        msg.target.y = touchPosition.y;

        byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

    # endregion
}