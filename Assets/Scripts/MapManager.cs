using UnityEngine;
using System.Collections.Generic;

using Vectrosity;

public class MapManager
{
    // The Map Border Axis. This Value Like The Orthographic Camera Size
    public static float BorderSizeX { get; private set; }
    public static float BorderSizeY { get; private set; }

    // This Values Below are the Camera viewport size
    public float XMin { get; private set; }
    public float YMin { get; private set; }
    public float XMax { get; private set; }
    public float YMax { get; private set; }

    private float OldYMax { get; set; }
    private float OldXMax { get; set; }

    private static int GridSize { get; set; }
    private VectorLine line;

    private Dictionary<string, GameObject> dots = new Dictionary<string, GameObject>();

    private static MapManager instance = null;
    private MapManager()
    {
        GridSize = 5;
        BorderSizeX = GlobalConfig.Map.BoundX;
        BorderSizeY = GlobalConfig.Map.BoundY;

        VectorLine.SetCamera3D(CameraManager.CameraMain);
        line = new VectorLine("grid", new Vector3[0], null, 1.0f, LineType.Discrete);

        OldYMax = 0f;
        OldXMax = 0f;

        SetBackground();

        List<Vector3> borderPoints = new List<Vector3>();
        borderPoints.Add(new Vector3(-BorderSizeX, -BorderSizeY, 0));
        borderPoints.Add(new Vector3(BorderSizeX, -BorderSizeY, 0));
        borderPoints.Add(new Vector3(BorderSizeX, BorderSizeY, 0));
        borderPoints.Add(new Vector3(-BorderSizeX, BorderSizeY, 0));
        borderPoints.Add(new Vector3(-BorderSizeX, -BorderSizeY, 0));

        VectorLine border = new VectorLine("border", borderPoints, null, 1f, LineType.Continuous);
        border.SetColor(Color.green);
        border.collider = true;
        border.Draw3D();
    }


    public static MapManager GetInstance()
    {
        if (instance == null)
        {
            instance = new MapManager();
        }

        return instance;
    }


    private void DetectSize()
    {
        int sw = Screen.width;
        int sh = Screen.height;

        float vs = Camera.main.orthographicSize * 2f;
        float hs = vs * sw / sh;

        XMax = hs / 2;
        YMax = vs / 2;
        XMin = -XMax;
        YMin = -YMax;

        if (YMax > BorderSizeY || XMax > BorderSizeX)
        {
            throw new System.Exception("Map Size Error!");
        }
    }

    public void SetBackground()
    {
        DetectSize();

        if (YMax != OldYMax || XMax != OldXMax)
        {
            OldXMax = XMax;
            OldYMax = YMax;
            UpdateBackground();
        }
    }

    private void UpdateBackground()
    {
        float xStart = -BorderSizeX * 2;
        float xEnd = -xStart;
        float yStart = -BorderSizeY * 2;
        float yEnd = -yStart;

        int xLineAmount = (int)((0 - xStart) / GridSize * 2 + 1);
        int yLineAmount = (int)((0 - yStart) / GridSize * 2 + 1);
        int numberPoints = (xLineAmount + yLineAmount) * 2;

        line.Resize(numberPoints);

        int index = 0;

        for (float x = xStart; x < xEnd; x+=GridSize )
        {
            line.points3[index++] = new Vector3(x, yStart, 0);
            line.points3[index++] = new Vector3(x, yEnd, 0);
        }

        for (float y = yStart; y < yEnd; y+=GridSize )
        {
            line.points3[index++] = new Vector3(xStart, y, 0);
            line.points3[index++] = new Vector3(xEnd, y, 0);
        }

        line.SetColor(new Color(0.6f, 0.6f, 0.6f));
        line.Draw3D();
    }

    public bool FindEmptyArea(out Vector2 point)
    {
        float halfOfInitSize = GlobalConfig.Unit.InitSize / 2f;

        Collider2D[] result;

        for (int i = 0; i < 100; i++)
        {
            point = new Vector2(
                Random.Range(-BorderSizeX + halfOfInitSize, BorderSizeX - halfOfInitSize),
                Random.Range(-BorderSizeY + halfOfInitSize, BorderSizeY - halfOfInitSize)
                );

            result = Physics2D.OverlapCircleAll(point, GlobalConfig.Unit.InitSize);
            if (result.Length == 0)
            {
                // this area has no players
                return true;
            }
        }

        point = new Vector2(0, 0);
        return false;
    }



    public void DotsAdd(string id, Vector2 pos, Color color)
    {
        GameObject obj = GameManager.DotPoolScript.Get();
        obj.transform.position = pos;

        SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
        sp.color = color;

        dots.Add(id, obj);
    }

    public void DotsRemove(string id)
    {
        if (!dots.ContainsKey(id))
        {
            return;
        }

        GameObject obj = dots[id];
        dots.Remove(id);

        GameManager.DotPoolScript.Put(obj);

        Debug.Log("Dots Remove");
    }
}

