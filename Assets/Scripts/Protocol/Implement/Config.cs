using UnityEngine;

namespace Protocol.Implement
{
    public static class Config
    {
        public static void Process(Protocol.Define.Config msg)
        {
            // Logic here
            Debug.Log("Config.sync_interval = " + msg.sync_interval);
        }

    }
}
