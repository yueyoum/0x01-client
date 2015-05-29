
using UnityEngine;

namespace Protocol.Implement
{
    public static class UnitUpdate
    {
        public static void Process(Protocol.Define.UnitUpdate msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();

            long lag = msg.milliseconds - TimeManager.GetInstance().LocalToServerTime();
            float lagSeconds = lag / 1000f;

            foreach (Protocol.Define.Unit unit in msg.units)
            {
                pm.UnitUpdate(
                    unit.id,
                    unit.size,
                    new Vector2(unit.pos.x, unit.pos.y),
                    new Vector2(unit.towards.x, unit.towards.y),
                    -lagSeconds
                    );
            }
        }
    }
}
