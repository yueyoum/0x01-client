using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScript : MonoBehaviour
{
    public string Name { get; set; }
    public float Lag { get; set; }
    public Vector3 Pos
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            realPos = value;
            oldPos = gameObject.transform.position;
            posRunningTime = Lag;
            inMove = true;
        }
    }

    public float Size
    {
        get
        {
            return currentSize;
        }
        set
        {
            realSize = value;
            oldSize = currentSize;
            reSizeRunningTime = Lag;
            inResize = true;
        }
    }



    private float boundSize;

    private float oldSize;
    private float currentSize;
    private float realSize;
    private float reSizeRunningTime = 0f;
    private bool inResize = true;

    private Vector3 oldPos;
    private Vector3 realPos;
    private float posRunningTime = 0f;
    private bool inMove = true;
    private float reSizeScale;

    private GameObject UI;
    private RectTransform uiTransform;
    private Text uiText;

    void Awake()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        boundSize = sp.sprite.bounds.size.x;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(inMove)
        {
            if(Vector3.Distance(gameObject.transform.position, realPos) < 0.05f)
            {
                gameObject.transform.position = realPos;
                posRunningTime = 0f;
                inMove = false;
            }
            else
            {
                gameObject.transform.position = Vector3.Lerp(oldPos, realPos, posRunningTime / GlobalConfig.SyncInterval);
                posRunningTime += Time.deltaTime;
            }
        }

        if(inResize)
        {
            if(Mathf.Abs(currentSize - realSize) < 0.05f)
            {
                currentSize = realSize;
                reSizeRunningTime = 0f;
                inResize = false;
            }
            else
            {
                currentSize = Mathf.Lerp(oldSize, realSize, reSizeRunningTime / GlobalConfig.SyncInterval);
                reSizeRunningTime += Time.deltaTime;
            }

            reSizeScale = currentSize / boundSize;
            gameObject.transform.localScale = new Vector3(reSizeScale, reSizeScale, 1);
        }


        UI.transform.position = gameObject.transform.position;
        uiTransform.sizeDelta = new Vector2(Size, Size);
        uiText.text = "大小:" + System.Math.Round(Size, 2); 
    }

    void LateUpdate()
    {
        if (gameObject.tag == "MyUnit")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }

        UI.transform.position = gameObject.transform.position;
    }


    public void InitUI(GameObject ui)
    {
        UI = ui;
        uiText = ui.transform.Find("Text").GetComponent<Text>();
        uiTransform = ui.GetComponent<RectTransform>();

        ui.transform.position = gameObject.transform.position;

        uiTransform.sizeDelta = new Vector2(Size, Size);
    }

    public void InitPosition(Vector3 pos)
    {
        gameObject.transform.position = pos;
        Pos = pos;
    }

    public void InitSize(float size)
    {
        currentSize = size;
        Size = size;
    }

}
