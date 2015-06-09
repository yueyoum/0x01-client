using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit
{
    public GameObject Player { get; private set; }
    public PlayerScript Script { get; private set; }
    public GameObject UI { get; private set; }

    public PlayerUnit(GameObject p, PlayerScript ps, GameObject ui)
    {
        Player = p;
        Script = ps;
        UI = ui;
    }

}


public class PlayerManager
{
    private Dictionary<string, PlayerUnit> myUnits = new Dictionary<string, PlayerUnit>();
    private Dictionary<string, PlayerUnit> otherUnits = new Dictionary<string,PlayerUnit>();

    private static PlayerManager instance = null;
    private PlayerManager()
    {
        EventManger.OnSceneLoad += EventManger_OnSceneLoad;
    }


    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }

        return instance;
    }

    public Dictionary<string, PlayerUnit> GetMyUnits()
    {
        return myUnits;
    }


    #region Called by Client itself

    #endregion


    public void UnitAdd(bool isOwn, string id, string name, float score, Color color, Vector2 pos, Vector2 towards, Protocol.Define.Unit.UnitStatus status)
    {
        PlayerUnit pu = GameManager.PlayerPoolScript.Get();
        pu.Player.transform.position = pos;
        pu.Script.Id = id;
        pu.Script.Score = score;
        pu.Script.Towards = towards;
        pu.Script.Status = status;

        SpriteRenderer sp = pu.Player.GetComponent<SpriteRenderer>();
        sp.color = color;

        float scale = pu.Script.Size / sp.sprite.bounds.size.x;
        pu.Player.transform.localScale = new Vector3(scale, scale, 1);
        
        if (isOwn)
        {
            pu.Player.tag = "MyPlayerMain";
            pu.Player.layer = 9;
            myUnits.Add(id, pu);
        }
        else
        {
            pu.Player.tag = "OtherPlayer";
            pu.Player.layer = 10;
            otherUnits.Add(id, pu);
        }
    }

    # region Called by Server Data
    public void UnitUpdate(string id, float score, Vector2 pos, Vector2 towards, Protocol.Define.Unit.UnitStatus status, float timeSpan)
    {
        pos += towards * timeSpan;
        if (otherUnits.ContainsKey(id))
        {
            otherUnits[id].Player.transform.position = pos;
            otherUnits[id].Script.Towards = towards;
            otherUnits[id].Script.Score = score;
            otherUnits[id].Script.Status = status;
        }
    }


    public void UnitRemove(string id)
    {

        PlayerUnit pu;
        if (otherUnits.ContainsKey(id))
        {
            pu = otherUnits[id];
            otherUnits.Remove(id);
        }
        else if(myUnits.ContainsKey(id))
        {
            pu = myUnits[id];
            myUnits.Remove(id);
        }
        else
        {
            return;
        }

        GameManager.PlayerPoolScript.Put(pu);
    }
    # endregion


    private void EventManger_OnSceneLoad()
    {
        Debug.Log("PlayerManager: OnSceneLoad");


        // add self to map
        Vector2 point;
        if (!MapManager.GetInstance().FindEmptyArea(out point))
        {
            throw new System.Exception("No Empty Area...");
        }

        Vector2 towards = new Vector2(0, 0);
        Color color = Utils.RandomColor();
        int colorInt = Utils.ColorToInt(color);

        var unit = new Protocol.Define.Unit();
        unit.id = System.Guid.NewGuid().ToString();

        unit.pos = new Protocol.Define.Vector2();
        unit.pos.x = point.x;
        unit.pos.y = point.y;

        unit.towards = new Protocol.Define.Vector2();
        unit.towards.x = towards.x;
        unit.towards.y = towards.y;

        unit.name = "";
        unit.score = 0;
        unit.color = colorInt;
        unit.status = Protocol.Define.Unit.UnitStatus.Idle;

        var msg = new Protocol.Define.UnitAdd();
        msg.units.Add(unit);

        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);

        UnitAdd(
            true,
            unit.id,
            unit.name,
            unit.score,
            color,
            point,
            towards,
            unit.status
            );
    }
}

