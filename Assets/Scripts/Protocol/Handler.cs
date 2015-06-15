
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace Protocol
{
    public static class ProtocolHandler
    {
        private readonly static Dictionary<Int16, Type> IdToTypeDict = new Dictionary<Int16, Type>{
            {1, typeof(Protocol.Define.Config)},
            {2, typeof(Protocol.Define.TimeSync)},
            {100, typeof(Protocol.Define.UnitCreate)},
            {101, typeof(Protocol.Define.UnitAdd)},
            {102, typeof(Protocol.Define.UnitMove)},
            {103, typeof(Protocol.Define.UnitSplit)},
            {104, typeof(Protocol.Define.UnitEject)},
            {1000, typeof(Protocol.Define.SceneInit)},
            {1001, typeof(Protocol.Define.SceneSync)},
        };

        private readonly static Dictionary<Type, Action<Object>> MethodDispatcher = new Dictionary<Type,  Action<Object>>{
            {typeof(Protocol.Define.Config), o => Protocol.Implement.Config.Process((Protocol.Define.Config)o)},
            {typeof(Protocol.Define.TimeSync), o => Protocol.Implement.TimeSync.Process((Protocol.Define.TimeSync)o)},
            {typeof(Protocol.Define.UnitCreate), o => Protocol.Implement.UnitCreate.Process((Protocol.Define.UnitCreate)o)},
            {typeof(Protocol.Define.UnitAdd), o => Protocol.Implement.UnitAdd.Process((Protocol.Define.UnitAdd)o)},
            {typeof(Protocol.Define.UnitMove), o => Protocol.Implement.UnitMove.Process((Protocol.Define.UnitMove)o)},
            {typeof(Protocol.Define.UnitSplit), o => Protocol.Implement.UnitSplit.Process((Protocol.Define.UnitSplit)o)},
            {typeof(Protocol.Define.UnitEject), o => Protocol.Implement.UnitEject.Process((Protocol.Define.UnitEject)o)},
            {typeof(Protocol.Define.SceneInit), o => Protocol.Implement.SceneInit.Process((Protocol.Define.SceneInit)o)},
            {typeof(Protocol.Define.SceneSync), o => Protocol.Implement.SceneSync.Process((Protocol.Define.SceneSync)o)},
        };

        public static Type GetProtocolTypeById(Int16 id)
        {
            return IdToTypeDict[id];
        }


        public static byte[] Pack(Object data)
        {
            using (var ms = new MemoryStream())
            {
                var ser = new Serializer();
                ser.Serialize(ms, data);
                return ms.ToArray();
            }
        }

        private static byte[] PackWithId(Object data, Int16 id)
        {
            var dataBytes = Pack(data);

            id = IPAddress.HostToNetworkOrder(id);
            var idBytes = BitConverter.GetBytes(id);

            var buffer = new byte[dataBytes.Length + idBytes.Length];

            idBytes.CopyTo(buffer, 0);
            dataBytes.CopyTo(buffer, idBytes.Length);
            return buffer;
        }


        public static void Process(byte[] data)
        {
            var ms = new MemoryStream(data);
            var br = new BinaryReader(ms);

            var id = br.ReadInt16();
            id = IPAddress.NetworkToHostOrder(id);

            var dataBytes = br.ReadBytes(data.Length - 2);
            ms.Close();

            var dataStream = new MemoryStream(dataBytes);

            var ser = new Serializer();
            var protocolType = GetProtocolTypeById(id);
            var msg = ser.Deserialize(dataStream, null, protocolType);

            dataStream.Close();

            MethodDispatcher[protocolType](msg);
        }


        public static byte[] PackWithId(Protocol.Define.Config data)
        {
            return PackWithId(data, 1);
        }
            

        public static byte[] PackWithId(Protocol.Define.TimeSync data)
        {
            return PackWithId(data, 2);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitCreate data)
        {
            return PackWithId(data, 100);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitAdd data)
        {
            return PackWithId(data, 101);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitMove data)
        {
            return PackWithId(data, 102);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitSplit data)
        {
            return PackWithId(data, 103);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitEject data)
        {
            return PackWithId(data, 104);
        }
            

        public static byte[] PackWithId(Protocol.Define.SceneInit data)
        {
            return PackWithId(data, 1000);
        }
            

        public static byte[] PackWithId(Protocol.Define.SceneSync data)
        {
            return PackWithId(data, 1001);
        }
            

    }
}
