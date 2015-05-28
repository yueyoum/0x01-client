
using UnityEngine;

namespace Protocol.Implement
{
    public static class UnitUpdate
    {
        public static void Process(Protocol.Define.UnitUpdate msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();


            //long x = TimeManager.GetInstance().ServerToLocalTime(msg.milliseconds) - TimeManager.GetInstance().TimestampInMilliSeconds;
            long x = msg.milliseconds - TimeManager.GetInstance().LocalToServerTime();
            float y = x / 1000f;
            Debug.Log("x = " + x + ", timeSpan = " + y);


            foreach (Protocol.Define.Unit unit in msg.units)
            {
                pm.UnitUpdate(
                    unit.id,
                    unit.size,
                    new Vector2(unit.pos[0], unit.pos[1]),
                    new Vector2(unit.move_vector[0], unit.move_vector[1]),
                    -y
                    );
            }
        }
    }
}
