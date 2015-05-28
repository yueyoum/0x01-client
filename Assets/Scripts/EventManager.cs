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
        unit.pos.Add(pu.Player.transform.position.x);
        unit.pos.Add(pu.Player.transform.position.y);

        unit.move_vector.Add(pu.Script.Towards.x);
        unit.move_vector.Add(pu.Script.Towards.y);

        unit.size = pu.Script.Size;

        var msg = new Protocol.Define.UnitUpdate();
        msg.milliseconds = TimeManager.GetInstance().LocalToServerTime();
        msg.units.Add(unit);

        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

}