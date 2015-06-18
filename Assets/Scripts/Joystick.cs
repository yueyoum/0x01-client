using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnJoystickMoveStart()
    {
    }

    public void OnJoystickMoveEnd()
    {

        Protocol.Define.UnitMove msg = new Protocol.Define.UnitMove();
        msg.towards = new Protocol.Define.Vector2();
        msg.towards.x = 0;
        msg.towards.y = 0;

        byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

    public void OnJoystickMove(Vector2 towards)
    {
        towards.Normalize();

        Protocol.Define.UnitMove msg = new Protocol.Define.UnitMove();
        msg.towards = new Protocol.Define.Vector2();
        msg.towards.x = towards.x;
        msg.towards.y = towards.y;

        byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);

    }


    public void OnSpeedUpButtonDown()
    {
        var msg = new Protocol.Define.UnitSpeedUp();
        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }

    public void OnSpeedUpButtonUp()
    {
        var msg = new Protocol.Define.UnitSpeedNormal();
        var data = Protocol.ProtocolHandler.PackWithId(msg);
        Transport.GetInstance().Send(data);
    }
}
