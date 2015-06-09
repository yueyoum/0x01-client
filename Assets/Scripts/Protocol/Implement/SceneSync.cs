using UnityEngine;

namespace Protocol.Implement
{
    public static class SceneSync
    {
        public static void Process(Protocol.Define.SceneSync msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();

            long lag = TimeManager.GetInstance().LocalToServerTime() - msg.unit_updates.milliseconds;
            float lagSeconds = lag / 1000f;

            for (int i = 0; i < msg.unit_updates.units.Count; i++ )
            {
                pm.UnitUpdate(
                    msg.unit_updates.units[i].id,
                    msg.unit_updates.units[i].score,
                    new Vector2(msg.unit_updates.units[i].pos.x, msg.unit_updates.units[i].pos.y),
                    new Vector2(msg.unit_updates.units[i].towards.x, msg.unit_updates.units[i].towards.y),
                    msg.unit_updates.units[i].status,
                    lagSeconds
                    );
            }

            if (msg.dot_adds != null)
            {
                for (int i = 0; i < msg.dot_adds.dots.Count; i++)
                {
                    Debug.Log(msg.dot_adds.dots[i].id);
                }
            }

            if (msg.dot_removes != null)
            {
                MapManager mm = MapManager.GetInstance();
                for (int i = 0; i < msg.dot_removes.ids.Count; i++)
                {
                    mm.DotsRemove(msg.dot_removes.ids[i]);
                }
            }

        }
    }
}
