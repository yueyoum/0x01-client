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


    public void UnitAdd(bool isOwn, string id, string name, float size, Color color, Vector2 pos)
    {
        PlayerUnit pu = GameManager.PlayerPoolScript.Get();
        pu.Script.InitPosition(new Vector3(pos.x, pos.y));
        pu.Script.Id = id;
        pu.Script.Name = name;
        pu.Script.Size = size;

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
    public void UnitUpdate(string id, float size, Vector2 pos, float lag)
    {
        if (otherUnits.ContainsKey(id))
        {
            //pos += towards * timeSpan * otherUnits[id].Script.Speed;
            otherUnits[id].Script.Size = size;
            otherUnits[id].Script.Pos = new Vector3(pos.x, pos.y, 0);
        }
        else
        {
            //pos += towards * timeSpan * myUnits[id].Script.Speed;
            myUnits[id].Script.Lag = lag;
            myUnits[id].Script.Size = size;
            myUnits[id].Script.Pos = new Vector3(pos.x, pos.y, 0);
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

        Protocol.Define.UnitCreate msg = new Protocol.Define.UnitCreate();
        msg.name = "";
        msg.pos = new Protocol.Define.Vector2();
        msg.pos.x = point.x;
        msg.pos.y = point.y;

        byte[] data = Protocol.ProtocolHandler.PackWithId(msg);

        Transport.GetInstance().Send(data);

    }
}

