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
        EasyTouch.On_SimpleTap += On_SimpleTap;
        EasyTouch.On_LongTap2Fingers += On_LongTag2Fingers;
        EasyTouch.On_DoubleTap += On_DoubleTap;
        EasyTouch.On_PinchIn += On_PinchIn;
        EasyTouch.On_PinchOut += On_PinchOut;
    }

    void OnDisable()
    {
        EasyTouch.On_SimpleTap -= On_SimpleTap;
        EasyTouch.On_LongTap2Fingers -= On_LongTag2Fingers;
    }

    void OnDestroy()
    {
        EasyTouch.On_SimpleTap -= On_SimpleTap;
        EasyTouch.On_LongTap2Fingers -= On_LongTag2Fingers;
    }

    void On_SimpleTap(Gesture gesture)
    {
        //Debug.Log(gesture.GetTouchToWorldPoint(10));

        //HTTPRequest req = new HTTPRequest(
        //    new Uri("http://192.168.1.109:8081/"),
        //    OnRequestFinished
        //    );

        //req.Send();

        Vector3 touchPosition = gesture.GetTouchToWorldPoint(10);
        EventManger.GetInstance().TrigSimpleTap(touchPosition);
        Debug.Log("On_SimpleTap");
    }

    void On_LongTag2Fingers(Gesture gesture)
    {
        EventManger.GetInstance().TrigLongTap2Fingers();
    }

    void On_DoubleTap(Gesture gesture)
    {
        Debug.Log("On_DoubleTap");
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
