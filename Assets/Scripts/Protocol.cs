using System;
using System.Collections.Generic;

namespace Protocol
{
    public static class Protocol
    {
        public static System.Object GetObject(string cmd)
        {
            switch(cmd)
            {
                case "Start":
                    return new Client.GameStart();
                case "Init":
                    return new Client.PlayerInit();
                case "GetStatus":
                    return new Server.PlayerGetStatus();
                case "End":
                    return new Server.GameEnd();
                case "Update":
                    return new Both.PlayerUpdate();
                case "Die":
                    return new Both.PlayerDie();
                default:
                    return null;
            }
        }
    }

    public interface IProtocol
    {
        string Cmd { get; set; }
    }

    public abstract class AbstractPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public float[] Color { get; set; }
        public float[] Pos { get; set; }
        public float[] Towards { get; set; }
    }

    namespace Both
    {
        //Both Server to Client, and Client to Server
        public class PlayerDie : IProtocol
        {
            private string cmd = "Die";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }

            public int Id { get; set; }
        }

        public class PlayerUpdate: AbstractPlayer, IProtocol
        {
            private string cmd = "Update";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }

            public bool IsOwn { get; set; }
        }
    }



    namespace Server
    {
        // Server to Client

        public class PlayerGetStatus : IProtocol
        {
            private string cmd = "GetStatus";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }

        public class GameEnd : IProtocol
        {
            private string cmd = "End";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }
    }


    namespace Client
    {
        // Client to Server
        public class GameStart : IProtocol
        {
            private string cmd = "Start";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }


        public class PlayerInit : AbstractPlayer, IProtocol
        {
            private string cmd = "Init";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }
    }
}
