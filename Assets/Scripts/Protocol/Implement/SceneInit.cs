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

            for (int i = 0; i < msg.dot_adds.dots.Count; i++)
            {
                mm.DotsAdd(
                    msg.dot_adds.dots[i].id,
                    new Vector2(msg.dot_adds.dots[i].pos.x, msg.dot_adds.dots[i].pos.y),
                    Utils.IntToColor(msg.dot_adds.dots[i].color)
                    );
            }

            for (int i = 0; i < msg.unit_adds.units.Count; i++)
            {
                pm.UnitAdd(
                    false,
                    msg.unit_adds.units[i].id,
                    msg.unit_adds.units[i].name,
                    msg.unit_adds.units[i].score,
                    Utils.IntToColor(msg.unit_adds.units[i].color),
                    new Vector2(msg.unit_adds.units[i].pos.x, msg.unit_adds.units[i].pos.y),
                    new Vector2(msg.unit_adds.units[i].towards.x, msg.unit_adds.units[i].towards.y),
                    msg.unit_adds.units[i].status
                    );
            }

            EventManger.GetInstance().TrigSceneLoad();

        }
    }
}
