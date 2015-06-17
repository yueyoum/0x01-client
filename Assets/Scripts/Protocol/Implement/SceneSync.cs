using UnityEngine;

namespace Protocol.Implement
{
    public static class SceneSync
    {
        public static void Process(Protocol.Define.SceneSync msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();
            MapManager mm = MapManager.GetInstance();


            long serverTime = TimeManager.GetInstance().ServerTimeWithLag();
            float lag = (serverTime - msg.update_at) / 1000f;

            for (int i = 0; i < msg.unit_updates.Count; i++ )
            {
                pm.UnitUpdate(
                    msg.unit_updates[i].id,
                    msg.unit_updates[i].size,
                    new Vector2(msg.unit_updates[i].pos.x, msg.unit_updates[i].pos.y),
                    lag
                    );
            }

            for (int i = 0; i < msg.unit_removes.Count; i++ )
            {
                pm.UnitRemove(msg.unit_removes[i]);
            }

            for (int i = 0; i < msg.dot_adds.Count; i++)
            {
                Debug.Log(msg.dot_adds[i].id);
            }

            for (int i = 0; i < msg.dot_removes.Count; i++)
            {
                mm.DotsRemove(msg.dot_removes[i]);
            }
        }
    }
}
