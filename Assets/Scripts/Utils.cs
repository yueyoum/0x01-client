using UnityEngine;
using System.IO;
using System.Collections.Generic;


public static class Utils
{
    public static Color IntToColor(int number)
    {

        return (Color)IntToColor32(number);
    }

    public static Color32 IntToColor32(int number)
    {
        byte r = (byte)((number >> 16) & 0xFF);
        byte g = (byte)((number >> 8) & 0xFF);
        byte b = (byte)(number & 0xFF);

        return new Color32(r, g, b, 255);
    }
    

    public static int ColorToInt(Color c)
    {
        return Color32ToInt((Color32)c);
    }

    public static int Color32ToInt(Color32 c)
    {
        byte[] bs = new byte[] { c.b, c.g, c.r, 0 };

        MemoryStream ms = new MemoryStream(bs);
        BinaryReader br = new BinaryReader(ms);

        int number = br.ReadInt32();

        br.Close();
        ms.Close();

        return number;
    }

    public static Color RandomColor()
    {
        Color c;
        //switch(Random.Range(1, 3))
        //{
        //    case 1:
        //        c = new Color(
        //            Random.Range(0.8f, 1.0f),
        //            Random.Range(0.3f, 0.7f),
        //            Random.Range(0.3f, 0.7f)
        //            );
        //        break;
        //    case 2:
        //        c = new Color(
        //            Random.Range(0.3f, 0.7f),
        //            Random.Range(0.8f, 1.0f),
        //            Random.Range(0.3f, 0.7f)
        //            );
        //        break;
        //    default:
        //        c = new Color(
        //            Random.Range(0.3f, 0.7f),
        //            Random.Range(0.3f, 0.7f),
        //            Random.Range(0.8f, 1.0f)
        //            );
        //        break;
        //}

        c = new Color(
            Random.Range(0.5f, 0.9f),
            Random.Range(0.5f, 0.9f),
            Random.Range(0.5f, 0.9f)
            );

        return c;
    }

    public static string FormatNowDatetime()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
    }
}
