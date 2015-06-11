using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScript : MonoBehaviour
{
    public string Id { get; set; }
    public float Size { get; set; }
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
            //posRunningTime = Lag + 0.09f;
            posRunningTime = Lag;
            inMove = true;
        }
    }


    //private float boundSize;
    private Vector3 oldPos;
    private Vector3 realPos;
    private float posTotalTime = 0.2f;
    private float posRunningTime = 0f;
    private bool inMove = true;


    private GameObject UI;
    private RectTransform uiTransform;
    private Text uiText;

    void Awake()
    {
        //SpriteRenderer sp = GetComponent<SpriteRenderer>();
        //boundSize = sp.sprite.bounds.size.x;
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
                gameObject.transform.position = Vector3.Lerp(oldPos, realPos, posRunningTime / posTotalTime);
                posRunningTime += Time.deltaTime;
                //Debug.Log("position: " + gameObject.transform.position + "runningTime = " + posRunningTime);
            }
        }



        UI.transform.position = gameObject.transform.position;
        uiTransform.sizeDelta = new Vector2(Size, Size);
        uiText.text = "大小:" + System.Math.Round(Size, 2); 
    }

    void LateUpdate()
    {
        if (gameObject.tag == "MyPlayerMain")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }

        UI.transform.position = gameObject.transform.position;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger: " + other.gameObject.name);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.black);
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

}
