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

    public delegate void OnLongTap2FingersHandler();
    public static event OnLongTap2FingersHandler OnLongTap2Finger = null;

    public delegate void OnUnitScoreChangeHandler(PlayerScript ps);
    public static event OnUnitScoreChangeHandler OnUnitScoreChange = null;

    private EventManger()
    {
        OnConnectionMade += HandleConnectionMade;
        OnConnectionLost += HandleConnectionLost;

        OnSimpleTap += HandlerSimpleTap;
        OnLongTap2Finger += HandlerLongTap2Fingers;

        OnUnitScoreChange += HandlerUnitScoreChange;
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

    public void TrigLongTap2Fingers()
    {
        OnLongTap2Finger();
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
        Protocol.Define.UnitUpdate msg = new Protocol.Define.UnitUpdate();
        msg.milliseconds = TimeManager.GetInstance().LocalToServerTime();

        PlayerManager pm = PlayerManager.GetInstance();
        foreach(KeyValuePair<string, PlayerUnit>pair in pm.GetMyUnits())
        {
            if(pair.Value.Script.Status != Protocol.Define.Unit.UnitStatus.Jump)
            {
                pair.Value.Script.SetTarget(touchPosition);

                Protocol.Define.Unit unit = new Protocol.Define.Unit();

                unit.id = pair.Key;

                unit.pos = new Protocol.Define.Vector2();
                unit.pos.x = pair.Value.Player.transform.position.x;
                unit.pos.y = pair.Value.Player.transform.position.y;

                unit.towards = new Protocol.Define.Vector2();
                unit.towards.x = pair.Value.Script.Towards.x;
                unit.towards.y = pair.Value.Script.Towards.y;
                unit.status = pair.Value.Script.Status;
                unit.score = pair.Value.Script.Score;

                msg.units.Add(unit);
            }
        }

        if (msg.units.Count > 0)
        {
            byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
            Transport.GetInstance().Send(data);
        }

    }

    private void HandlerLongTap2Fingers()
    {
        Protocol.Define.UnitUpdate msg = new Protocol.Define.UnitUpdate();
        msg.milliseconds = TimeManager.GetInstance().LocalToServerTime();

        PlayerManager pm = PlayerManager.GetInstance();
        foreach(KeyValuePair<string, PlayerUnit>pair in pm.GetMyUnits())
        {
            if (pair.Value.Script.Status == Protocol.Define.Unit.UnitStatus.Move)
            {
                pair.Value.Script.Stop();

                Protocol.Define.Unit unit = new Protocol.Define.Unit();
                unit.id = pair.Key;
                unit.pos = new Protocol.Define.Vector2();
                unit.pos.x = pair.Value.Player.transform.position.x;
                unit.pos.y = pair.Value.Player.transform.position.y;

                unit.towards = new Protocol.Define.Vector2();
                unit.towards.x = 0;
                unit.towards.y = 0;
                unit.status = Protocol.Define.Unit.UnitStatus.Idle;

                unit.score = pair.Value.Script.Score;

                msg.units.Add(unit);
            }
        }

        if (msg.units.Count > 0)
        {
            byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
            Transport.GetInstance().Send(data);
        }
    }

    private void HandlerUnitScoreChange(PlayerScript ps)
    {
        Debug.Log("HandlerUnitScoreChange");
        Protocol.Define.UnitUpdate msg = new Protocol.Define.UnitUpdate();
        msg.milliseconds = TimeManager.GetInstance().LocalToServerTime();

        Protocol.Define.Unit unit = new Protocol.Define.Unit();
        unit.id = ps.Id;
        unit.pos = new Protocol.Define.Vector2();
        unit.pos.x = ps.gameObject.transform.position.x;
        unit.pos.y = ps.gameObject.transform.position.y;

        unit.towards = new Protocol.Define.Vector2();
        unit.towards.x = ps.Towards.x;
        unit.towards.y = ps.Towards.y;

        unit.status = ps.Status;
        unit.score = ps.Score;

        msg.units.Add(unit);

        byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

    # endregion
}