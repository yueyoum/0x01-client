using UnityEngine;

namespace Protocol.Implement
{
    public static class SceneInit
    {
        public static void Process(Protocol.Define.SceneInit msg)
        {
            // Logic here
            Debug.Log("SceneInit");
            MapManager mm = MapManager.GetInstance();
            PlayerManager pm = PlayerManager.GetInstance();

            for (int i = 0; i < msg.dot_adds.Count; i++)
            {
                mm.DotsAdd(
                    msg.dot_adds[i].id,
                    new Vector2(msg.dot_adds[i].pos.x, msg.dot_adds[i].pos.y),
                    Utils.IntToColor(msg.dot_adds[i].color)
                    );
            }

            for (int i = 0; i < msg.unit_adds.Count; i++)
            {
                pm.UnitAdd(
                    false,
                    msg.unit_adds[i].id,
                    msg.unit_adds[i].name,
                    msg.unit_adds[i].size,
                    Utils.IntToColor(msg.unit_adds[i].color),
                    new Vector2(msg.unit_adds[i].pos.x, msg.unit_adds[i].pos.y)
                    );
            }

            EventManger.GetInstance().TrigSceneLoad();

        }
    }
}
