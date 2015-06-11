using UnityEngine;
using System.Collections;
using System.Text;

using BestHTTP.WebSocket;

public class Transport
{
    private WebSocket webSocket = null;
    private static Transport instance = null;

    public static string uri;

    public bool IsOpen
    {
        get
        {
            return webSocket.IsOpen;
        }
    }


    private Transport()
    {
        BestHTTP.HTTPManager.ConnectTimeout = new System.TimeSpan(0, 0, 3);
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
        webSocket.Close();
        webSocket.Open();
    }

    public void Send(string text)
    {
        webSocket.Send(text);
    }

    public void Send(byte[] data)
    {
        webSocket.Send(data);
    }


    private void OnOpen(WebSocket ws)
    {
        EventManger.GetInstance().TrigConnectionMade();
    }

    private void OnMessage(WebSocket ws, string message)
    {
        throw new System.Exception("Invalid string data. Should be byte[]");
    }

    private void OnBinary(WebSocket ws, byte[] message)
    {
        Protocol.ProtocolHandler.Process(message);
    }

    private void OnClose(WebSocket ws, System.UInt16 code, string reason)
    {
        EventManger.GetInstance().TrigConnectionLost();
    }

    private void OnError(WebSocket ws, System.Exception ex)
    {
        EventManger.GetInstance().TrigConnectionLost();
    }
}
