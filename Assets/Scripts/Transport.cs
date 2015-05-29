using UnityEngine;
using System.Collections;
using System.Text;

using BestHTTP.WebSocket;

public class Transport
{
    private WebSocket webSocket = null;
    private static Transport instance = null;

    public static string uri;


    private Transport()
    {
        webSocket = new WebSocket(new System.Uri(uri));
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessage;
        webSocket.OnBinary += OnBinary;
        webSocket.OnClosed += OnClose;
        webSocket.OnError += OnError;
    }

    public static Transport GetInstance()
    {
        if (instance == null)
        {
            instance = new Transport();
        }

        return instance;
    }


    public void Connect()
    {
        webSocket.Open();
    }

    public void Send(string text)
    {
        webSocket.Send(text);
        Debug.Log("Send: " + text);
    }

    public void Send(byte[] data)
    {
        webSocket.Send(data);
    }


    private void OnOpen(WebSocket ws)
    {
        Debug.Log("WS OPEN");

        // time sync
        var timemsg = new Protocol.Define.TimeSync();
        timemsg.client = TimeManager.GetInstance().TimestampInMilliSeconds;
        timemsg.server = 0;

        Send(Protocol.ProtocolHandler.PackWithId(timemsg));




        // add self to map
        Vector2 point;
        if (!MapManager.GetInstance().FindEmptyArea(out point))
        {
            throw new System.Exception("No Empty Area...");
        }

        Vector2 towards = new Vector2(0, 0);
        int color = Utils.ColorToInt(Utils.RandomColor());

        var unit = new Protocol.Define.Unit();
        unit.id = System.Guid.NewGuid().ToString();

        unit.pos = new Protocol.Define.Vector2();
        unit.pos.x = point.x;
        unit.pos.y = point.y;

        unit.towards = new Protocol.Define.Vector2();
        unit.towards.x = towards.x;
        unit.towards.y = towards.y;

        unit.name = "";
        unit.size = PlayerManager.InitSize;
        unit.color = color;


        PlayerManager.GetInstance().UnitAdd(
            true,
            unit.id,
            unit.name,
            unit.size,
            color,
            point,
            towards
            );

        var msg = new Protocol.Define.UnitAdd();
        msg.units.Add(unit);

        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Send(data);
    }

    private void OnMessage(WebSocket ws, string message)
    {
        Debug.Log("WS OnMessage: " + message);
    }

    private void OnBinary(WebSocket ws, byte[] message)
    {
        Protocol.ProtocolHandler.Process(message);
    }

    private void OnClose(WebSocket ws, System.UInt16 code, string reason)
    {
        Debug.Log("WS Close");
        Debug.Log("code = " + code);
        Debug.Log("reason = " + reason);
    }

    private void OnError(WebSocket ws, System.Exception ex)
    {
        Debug.Log("WS Error");
        
    }
}
