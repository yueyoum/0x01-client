
using UnityEngine;
using System.Collections.Generic;

using BestHTTP;

public static class GlobalConfig
{
    public static float SyncInterval { get; set; }

    public static class Unit
    {
        public static float InitSize { get; set; }
    }

    public static class Map
    {
        public static int BoundX { get; set; }
        public static int BoundY { get; set; }
    }


    private static bool getConfigDone = false;
    public static void GetConfig()
    {
        new HTTPRequest(new System.Uri("http://192.168.1.109:9001/config/"), GetConfigCallBack).Send();
    }

    public static bool IsGetConfigDone()
    {
        return getConfigDone;
    }

    private static void GetConfigCallBack(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("request.State = " + request.State);
        if (request.State != HTTPRequestStates.Finished)
        {
            Debug.Log("Error...");
        }
        else
        {
            Debug.Log("Status Code = " + response.StatusCode);
            Debug.Log("Success = " + response.IsSuccess);
            Protocol.ProtocolHandler.Process(response.Data);
            getConfigDone = true;
        }
    }
}
