using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;

using BestHTTP;
using Newtonsoft.Json;

public class GlobalTouch : MonoBehaviour
{
    void OnEnable()
    {
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_PinchIn += On_PinchIn;
        EasyTouch.On_PinchOut += On_PinchOut;
    }

    void OnDisable()
    {
        EasyTouch.On_TouchStart -= On_TouchStart;
    }

    void OnDestroy()
    {
        EasyTouch.On_TouchStart -= On_TouchStart;
    }

    void On_TouchStart(Gesture gesture)
    {
        //Debug.Log(gesture.GetTouchToWorldPoint(10));

        //HTTPRequest req = new HTTPRequest(
        //    new Uri("http://192.168.1.109:8081/"),
        //    OnRequestFinished
        //    );

        //req.Send();

        Vector3 touchPosition = gesture.GetTouchToWorldPoint(10);
        PlayerManager.GetInstance().Move(touchPosition);
    }


    void On_PinchIn(Gesture gesture)
    {
        CameraManager cm = CameraManager.GetInstance();
        cm.ShrinkMainCameraView();
    }

    void On_PinchOut(Gesture gesture)
    {
        CameraManager cm = CameraManager.GetInstance();
        cm.ExpandMainCameraView();
    }


    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("HTTP Finished: " + response.DataAsText);
    }
}
