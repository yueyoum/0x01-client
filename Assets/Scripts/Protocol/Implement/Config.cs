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
            GlobalConfig.Map.MinX = msg.map_minx;
            GlobalConfig.Map.MinY = msg.map_miny;
            GlobalConfig.Map.MaxX = msg.map_maxx;
            GlobalConfig.Map.MaxY = msg.map_maxy;
        }

    }
}
