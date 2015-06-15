using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Root : MonoBehaviour
{
    public Text sw_t;
    public Text sh_t;
    public Text vs_t;
    public Text hs_t;

    public GameObject PlayerPrefab;
    public GameObject UnitUIPrefab;

    void Awake()
    {
        GameManager.RootScript = this;

        GlobalConfig.Unit.InitSize = 5f;

        EventManger.GetInstance();

        CameraManager.CameraMain = Camera.main;
        CameraManager.MaxSize = 40;
        CameraManager.MinSize = 10;
        CameraManager.InitSize = 20;
        CameraManager.ResizeSpeed = 3;
        CameraManager.GetInstance();

        MapManager.BorderSizeY = 80;
        MapManager.BorderSizeX = 100;
        MapManager.GridSize = 5;
        MapManager.GetInstance();

        PlayerManager.GetInstance();

        TimeManager.GetInstance();

        Transport.uri = "ws://192.168.1.109:9001/ws/";
        Transport.GetInstance().Connect();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int sw = Screen.width;
        int sh = Screen.height;

        float vs = Camera.main.orthographicSize * 2f;
        float hs = vs * sw / sh;

        sw_t.text = "Screen Width: " + sw;
        sh_t.text = "Screen Height:" + sh;
        vs_t.text = "Camera V Size:" + vs;
        hs_t.text = "Camera H Szie:" + hs;

        MapManager mm = MapManager.GetInstance();
        mm.SetBackground();
    }

    void OnApplicationQuit()
    {
    }
}
