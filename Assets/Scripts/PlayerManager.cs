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
    public void AddPlayer(bool isOwn, int id, string name, float size,  Color color, Vector3 pos, Vector3 towards)
    {
        PlayerUnit pu = GameManager.PlayerPoolScript.Get();
        pu.Player.transform.position = pos;
        pu.Script.Size = size;
        pu.Script.Towards = towards;

        SpriteRenderer sp = pu.Player.GetComponent<SpriteRenderer>();
        sp.color = color;

        float scale = size / sp.sprite.bounds.size.x;
        pu.Player.transform.localScale = new Vector3(scale, scale, 1);
        
        if (isOwn)
        {
            pu.Player.tag = "MyPlayerMain";
            myPlayers.Add(id, pu);
        }
        else
        {
            pu.Player.tag = "OtherPlayer";
            otherPlayers.Add(id, pu);
        }
    }


    public void Update(int id, float size, Vector3 pos, Vector3 towards)
    {
        // only update others
        otherPlayers[id].Player.transform.position = pos;
        otherPlayers[id].Script.Towards = towards;
        otherPlayers[id].Script.Size = size;

    }
    # endregion



    public void Die(int id)
    {
        if (!otherPlayers.ContainsKey(id))
        {
            return;
        }

        PlayerUnit pu;
        if (otherPlayers.ContainsKey(id))
        {
            pu = otherPlayers[id];
            otherPlayers.Remove(id);
        }
        else if(myPlayers.ContainsKey(id))
        {
            pu = myPlayers[id];
            myPlayers.Remove(id);
        }
        else
        {
            return;
        }

        GameManager.PlayerPoolScript.Put(pu);
    }
}

