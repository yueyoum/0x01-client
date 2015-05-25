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
                case "Eat":
                    return new Client.PlayerEat();
                case "Add":
                    return new Server.PlayerAdd();
                case "End":
                    return new Server.GameEnd();
                case "Update":
                    return new Both.PlayerUpdate();
                case "Die":
                    return new Server.PlayerDie();
                default:
                    return null;
            }
        }
    }

    public interface IProtocol
    {
        string Cmd { get; set; }
    }

    public abstract class AbstractPlayerBase
    {
        public int Id { get; set; }
        public float Size { get; set; }
        public float[] Pos { get; set; }
        public float[] Towards { get; set; }
    }

    public abstract class AbstractPlayerFull : AbstractPlayerBase
    {
        public string Name { get; set; }
        public float[] Color { get; set; }
    }


    namespace Both
    {
        //Both Server to Client, and Client to Server
        public class PlayerUpdate: AbstractPlayerBase, IProtocol
        {
            private string cmd = "Update";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }
    }


    namespace Server
    {
        // Server to Client

        public class PlayerAdd : AbstractPlayerFull, IProtocol
        {
            private string cmd = "Add";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }

            public bool IsOwn { get; set; }
        }


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


        public class PlayerInit : AbstractPlayerFull, IProtocol
        {
            private string cmd = "Init";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }
        }

        public class PlayerEat : AbstractPlayerBase, IProtocol
        {
            private string cmd = "Eat";
            public string Cmd
            {
                get { return cmd; }
                set { cmd = value; }
            }

            public int TId { get; set; }
        }
    }
}
