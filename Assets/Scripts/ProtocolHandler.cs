using UnityEngine;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ProtocolConverter : JsonConverter
{
    public override bool CanConvert(System.Type objectType)
    {
        return typeof(Protocol.IProtocol).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObj = JObject.Load(reader);

        string cmd = (string)jsonObj["Cmd"];
        System.Object obj = Protocol.Protocol.GetObject(cmd);
        serializer.Populate(jsonObj.CreateReader(), obj);
        return obj;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new System.NotImplementedException();
    }
}


public class ProtocolHandler
{
    private static ProtocolHandler instance = null;
    private ProtocolHandler()
    {

    }

    public static ProtocolHandler GetInstance()
    {
        if (instance == null)
        {
            instance = new ProtocolHandler();
        }

        return instance;
    }


    public void StartGame()
    {
        Protocol.Client.GameStart p = new Protocol.Client.GameStart();
        string data = JsonConvert.SerializeObject(p);
        //Transport.GetInstance().Send(data);
        Debug.Log("StartGame");
        Debug.Log(data);
        PlayerInit();
    }

    public void PlayerInit()
    {
        Debug.Log("PlayerInit");
        Vector2 point;
        bool hasEmptyArea = MapManager.GetInstance().FindEmptyArea(out point);
        if (!hasEmptyArea)
        {
            throw new System.Exception("no empty area!!!");
        }

        Protocol.Client.PlayerInit p = new Protocol.Client.PlayerInit
        {
            Name = "",
            Size = PlayerManager.InitSize,
            Color = new float[] { Random.Range(0.6f, 0.95f), Random.Range(0.6f, 0.95f), Random.Range(0.6f, 0.95f) },
            Pos = new float[] { point.x, point.y }
        };

        string data = JsonConvert.SerializeObject(p);
        Debug.Log(data);
        //Transport.GetInstance().Send(data);
    }


    public void PlayerUpdate(int id, float size, Vector3 currentPos, Vector3 towards)
    {
        Protocol.Both.PlayerUpdate p = new Protocol.Both.PlayerUpdate()
        {
            Id = id,
            Size = size,
            Pos = new float[] { currentPos.x, currentPos.y, currentPos.z },
            Towards = new float[] { towards.x, towards.y, towards.z }
        };

        string data = JsonConvert.SerializeObject(p);
        Transport.GetInstance().Send(data);
    }


    public void Die(int id)
    {
        Protocol.Both.PlayerDie p = new Protocol.Both.PlayerDie()
        {
            Id = id,
        };

        string data = JsonConvert.SerializeObject(p);
        Transport.GetInstance().Send(data);
    }

    public void Process(string data)
    {
        Protocol.IProtocol obj = JsonConvert.DeserializeObject<Protocol.IProtocol>(data, new ProtocolConverter());
        switch (obj.Cmd)
        {
            case "GetStatus":
                Process((Protocol.Server.PlayerGetStatus)obj);
                break;
            case "Update":
                Process((Protocol.Both.PlayerUpdate)obj);
                break;
            case "Die":
                Process((Protocol.Both.PlayerDie)obj);
                break;
            default:
                throw new System.Exception("Bad Cmd: " + obj.Cmd);
        }
    }


    //private void Process(Protocol.Server.PlayerAdd data)
    //{
    //    PlayerManager.GetInstance().AddPlayer(
    //        data.Type,
    //        data.Id,
    //        data.Name,
    //        data.Size,
    //        new Vector3(data.Pos[0], data.Pos[1]),
    //        new Color(data.Color[0], data.Color[1], data.Color[2])
    //        );
    //}

    private void Process(Protocol.Server.PlayerGetStatus data)
    {

    }

    private void Process(Protocol.Both.PlayerUpdate data)
    {
        PlayerManager.GetInstance().Update(
            data.Id,
            data.Size,
            new Vector3(data.Pos[0], data.Pos[1], data.Pos[2]),
            new Vector3(data.Towards[0], data.Towards[1], data.Towards[2])
            );
    }


    private void Process(Protocol.Both.PlayerDie data)
    {

    }

}
