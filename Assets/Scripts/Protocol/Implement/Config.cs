using UnityEngine;

namespace Protocol.Implement
{
    public static class Config
    {
        public static void Process(Protocol.Define.Config msg)
        {
            // Logic here
            Debug.Log("Config.sync_interval = " + msg.sync_interval);
            GlobalConfig.SyncInterval = msg.sync_interval / 1000.0f;
            GlobalConfig.Map.BoundX = msg.map_x;
            GlobalConfig.Map.BoundY = msg.map_y;
        }
    }
}
