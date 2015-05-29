using UnityEngine;
using System.Collections.Generic;

public class EventManger
{

    private static EventManger instance = null;

    private EventManger()
    {
        PlayerManager.OnUnitUpdate += OnUnitUpdate;
    }

    public static EventManger GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManger();
        }

        return instance;
    }


    private void OnUnitUpdate(string id, PlayerUnit pu)
    {
        var unit = new Protocol.Define.Unit();
        unit.id = id;

        unit.pos = new Protocol.Define.Vector2();
        unit.pos.x = pu.Player.transform.position.x;
        unit.pos.y = pu.Player.transform.position.y;

        unit.towards = new Protocol.Define.Vector2();
        unit.towards.x = pu.Script.Towards.x;
        unit.towards.y = pu.Script.Towards.y;

        unit.size = pu.Script.Size;

        var msg = new Protocol.Define.UnitUpdate();
        msg.milliseconds = TimeManager.GetInstance().LocalToServerTime();
        msg.units.Add(unit);

        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

}