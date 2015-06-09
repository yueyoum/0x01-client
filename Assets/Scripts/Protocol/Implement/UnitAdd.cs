using UnityEngine;

namespace Protocol.Implement
{
    public static class UnitAdd
    {
        public static void Process(Protocol.Define.UnitAdd msg)
        {
            // Logic here
            PlayerManager pm = PlayerManager.GetInstance();

            for (int i = 0; i < msg.units.Count; i++)
            {
                pm.UnitAdd(
                    false,
                    msg.units[i].id,
                    msg.units[i].name,
                    msg.units[i].score,
                    Utils.IntToColor(msg.units[i].color),
                    new Vector2(msg.units[i].pos.x, msg.units[i].pos.y),
                    new Vector2(msg.units[i].towards.x, msg.units[i].towards.y),
                    msg.units[i].status
                    );
            }
            Debug.Log("NetWorking: UnitAdd");
        }
    }
}
