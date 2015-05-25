using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit
{
    public GameObject Player { get; private set; }
    public PlayerScript Script { get; private set; }

    public PlayerUnit(GameObject p, PlayerScript ps)
    {
        Player = p;
        Script = ps;
    }
}


public class PlayerManager
{
    public static float InitSize { get; set; }
    public static float MaxSize { get; set; }

    private Dictionary<int, PlayerUnit> myPlayers = new Dictionary<int, PlayerUnit>();
    private Dictionary<int, PlayerUnit> otherPlayers = new Dictionary<int,PlayerUnit>();

    private static PlayerManager instance = null;
    private PlayerManager()
    {
    }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }

        return instance;
    }


    #region Called by Client itself
    public void Move(Vector3 target)
    {
        foreach (KeyValuePair<int, PlayerUnit> pair in myPlayers)
        {
            pair.Value.Script.SetTarget(target);
            ProtocolHandler.GetInstance().PlayerUpdate(
                pair.Key,
                pair.Value.Script.Size,
                pair.Value.Player.transform.position,
                pair.Value.Script.Towards
                );
        }
    }
    #endregion




    # region Called by Network
    public void AddPlayer(int type, int id, string name, float size, Vector3 pos, Color color)
    {
        PlayerUnit pu = GameManager.PlayerPoolScript.Get();
        pu.Player.transform.position = pos;
        pu.Script.Size = size;

        SpriteRenderer sp = pu.Player.GetComponent<SpriteRenderer>();
        sp.color = color;

        float scale = size / sp.sprite.bounds.size.x;
        pu.Player.transform.localScale = new Vector3(scale, scale, 1);
        
        if (type == 1)
        {
            pu.Player.tag = "MyPlayerMain";
            myPlayers.Add(id, pu);
        }
        else if(type == 2)
        {
            pu.Player.tag = "OtherPlayer";
            otherPlayers.Add(id, pu);
        }
        else
        {
            throw new System.Exception("AddPlayer unknown type: " + type);
        }
    }


    public void Report()
    {
        foreach(KeyValuePair<int, PlayerUnit>pair in myPlayers)
        {
            ProtocolHandler.GetInstance().PlayerUpdate(
                pair.Key,
                pair.Value.Script.Size,
                pair.Value.Player.transform.position,
                pair.Value.Script.Towards
                );
        }
    }

    public void Update(int id, float size, Vector3 currentPos, Vector3 targetPos)
    {
        otherPlayers[id].Player.transform.position = currentPos;
        otherPlayers[id].Script.SetTarget(targetPos);
        otherPlayers[id].Script.Size = size;
    }
    # endregion

    public void Die()
    {
        foreach(KeyValuePair<int, PlayerUnit>pair in myPlayers)
        {
            ProtocolHandler.GetInstance().Die(pair.Key);
        }
    }

    public void Die(int id)
    {
        if (!otherPlayers.ContainsKey(id))
        {
            return;
        }

        PlayerUnit pu = otherPlayers[id];
        otherPlayers.Remove(id);

        GameManager.PlayerPoolScript.Put(pu);
    }
}

