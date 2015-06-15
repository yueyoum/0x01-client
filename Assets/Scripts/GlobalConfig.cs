using System;
using System.Collections.Generic;

public static class GlobalConfig
{
    public static float SyncInterval { get; set; }

    public static class Unit
    {
        public static float InitSize { get; set; }
    }

    public static class Map
    {
        public static int MinX { get; set; }
        public static int MinY { get; set; }
        public static int MaxX { get; set; }
        public static int MaxY { get; set; }
    }
}
