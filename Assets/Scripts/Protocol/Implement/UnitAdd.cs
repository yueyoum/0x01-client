using UnityEngine;

namespace Protocol.Implement
{
    public static class UnitAdd
    {
        public static void Process(Protocol.Define.UnitAdd msg)
        {
            // Logic here

            PlayerManager pm = PlayerManager.GetInstance();
            foreach(Protocol.Define.Unit unit in msg.units)
            {
                pm.UnitAdd(
                    false,
                    unit.id,
                    unit.name,
                    unit.size,
                    unit.color,
                    new Vector2(unit.pos[0], unit.pos[1]),
                    new Vector2(unit.move_vector[0], unit.move_vector[1])
                    );
            }
        }
    }
}
