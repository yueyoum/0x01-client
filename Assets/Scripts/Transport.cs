using UnityEngine;
using System.Collections;
using System.Text;

using BestHTTP.WebSocket;
using Newtonsoft.Json;

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
        ProtocolHandler.GetInstance().StartGame();
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
        ProtocolHandler.GetInstance().StartGame();
    }

    private void OnMessage(WebSocket ws, string message)
    {
        Debug.Log("WS OnMessage: " + message);
        ProtocolHandler.GetInstance().Process(message);
    }

    private void OnBinary(WebSocket ws, byte[] message)
    {
        string text = Encoding.UTF8.GetString(message);
        Debug.Log("WS OnBinary: " + text);
        ProtocolHandler.GetInstance().Process(text);
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
