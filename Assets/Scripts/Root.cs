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

    private bool hasInitialized = false;

    void Awake()
    {
        GameManager.RootScript = this;

        BestHTTP.HTTPManager.ConnectTimeout = System.TimeSpan.FromSeconds(3);

        GlobalConfig.GetConfig();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalConfig.IsGetConfigDone())
        {
            return;
        }

        InitializeGameComponent();

        Timer.GetInstance().Update();


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


    void InitializeGameComponent()
    {
        if (hasInitialized)
        {
            return;
        }

        hasInitialized = true;

        Debug.Log("Init Game Component");
        TimeManager.GetInstance();
        Timer.GetInstance();
        EventManger.GetInstance();

        Transport.uri = "ws://192.168.1.109:9001/ws/";
        Transport.GetInstance().Connect();

        CameraManager.CameraMain = Camera.main;
        CameraManager.MaxSize = 40;
        CameraManager.MinSize = 10;
        CameraManager.InitSize = 20;
        CameraManager.ResizeSpeed = 3;
        CameraManager.GetInstance();

        MapManager.GetInstance();
        PlayerManager.GetInstance();
    }
}
