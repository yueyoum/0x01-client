
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace Protocol
{
    public static class ProtocolHandler
    {
        private readonly static Dictionary<Int16, Type> IdToTypeDict = new Dictionary<Int16, Type>{
            {1, typeof(Protocol.Define.TimeSync)},
            {100, typeof(Protocol.Define.UnitAdd)},
            {101, typeof(Protocol.Define.UnitUpdate)},
            {102, typeof(Protocol.Define.UnitRemove)},
        };

        private readonly static Dictionary<Type, Action<Object>> MethodDispatcher = new Dictionary<Type,  Action<Object>>{
            {typeof(Protocol.Define.TimeSync), o => Protocol.Implement.TimeSync.Process((Protocol.Define.TimeSync)o)},
            {typeof(Protocol.Define.UnitAdd), o => Protocol.Implement.UnitAdd.Process((Protocol.Define.UnitAdd)o)},
            {typeof(Protocol.Define.UnitUpdate), o => Protocol.Implement.UnitUpdate.Process((Protocol.Define.UnitUpdate)o)},
            {typeof(Protocol.Define.UnitRemove), o => Protocol.Implement.UnitRemove.Process((Protocol.Define.UnitRemove)o)},
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


        public static byte[] PackWithId(Protocol.Define.TimeSync data)
        {
            return PackWithId(data, 1);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitAdd data)
        {
            return PackWithId(data, 100);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitUpdate data)
        {
            return PackWithId(data, 101);
        }
            

        public static byte[] PackWithId(Protocol.Define.UnitRemove data)
        {
            return PackWithId(data, 102);
        }
            

    }
}
