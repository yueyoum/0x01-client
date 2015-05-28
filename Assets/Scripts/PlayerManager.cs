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

    public delegate void OnUnitUpdateHandler(string id, PlayerUnit pu);
    public static event OnUnitUpdateHandler OnUnitUpdate = null;

    private Dictionary<string, PlayerUnit> myPlayers = new Dictionary<string, PlayerUnit>();
    private Dictionary<string, PlayerUnit> otherPlayers = new Dictionary<string,PlayerUnit>();

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
        foreach (KeyValuePair<string, PlayerUnit> pair in myPlayers)
        {
            pair.Value.Script.SetTarget(target);
            if (OnUnitUpdate != null)
            {
                OnUnitUpdate(pair.Key, pair.Value);
            }
        }
    }

    public void Update()
    {
        foreach (KeyValuePair<string, PlayerUnit> pair in myPlayers)
        {
            if (OnUnitUpdate != null)
            {
                OnUnitUpdate(pair.Key, pair.Value);
            }
        }
    }

    #endregion




    # region Called by Network
    public void UnitAdd(bool isOwn, string id, string name, float size,  int color, Vector2 pos, Vector2 moveVector)
    {
        PlayerUnit pu = GameManager.PlayerPoolScript.Get();
        pu.Player.transform.position = pos;
        pu.Script.Size = size;
        pu.Script.Towards = moveVector;

        SpriteRenderer sp = pu.Player.GetComponent<SpriteRenderer>();
        sp.color = Utils.IntToColor(color);

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


    public void UnitUpdate(string id, float size, Vector2 pos, Vector2 moveVector, float timeSpan)
    {
        pos += moveVector * timeSpan * 10;
        // only update others
        if (otherPlayers.ContainsKey(id))
        {
            otherPlayers[id].Player.transform.position = pos;
            otherPlayers[id].Script.Towards = moveVector;
            otherPlayers[id].Script.Size = size;
        }
        else
        {
            myPlayers[id].Player.transform.position = pos;
            myPlayers[id].Script.Towards = moveVector;
            myPlayers[id].Script.Size = size;

        }
    }


    public void UnitRemove(string id)
    {

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
    # endregion

}

