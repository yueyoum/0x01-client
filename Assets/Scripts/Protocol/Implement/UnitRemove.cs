
using UnityEngine;

namespace Protocol.Implement
{
    public static class UnitRemove
    {
        public static void Process(Protocol.Define.UnitRemove msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();
            foreach (string id in msg.ids)
            {
                pm.UnitRemove(id);
            }
        }
    }
}
