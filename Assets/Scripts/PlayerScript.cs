using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScript : MonoBehaviour
{
    public string Id { get; set; }

    private Vector3 oldTowards = new Vector3();
    private Vector3 currentTowards = new Vector3();
    private Vector3 realTowards = new Vector3();

    private float towardsChangeTotalSeconds = 0.3f;
    private float towardsChangeRunningSeconds = 0f;

    public Vector3 Towards
    {
        get
        {
            return realTowards;
        }
        set
        {
            oldTowards = currentTowards;
            realTowards = value;
            towardsChangeRunningSeconds = 0f;
        }
    }

    private float score;

    private float oldSize;
    private float currentSize;
    private float realSize;
    private float sizeChangeTotalSeconds = 0.5f;
    private float sizeChangeRunningSeconds = 0f;


    public float Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            oldSize = currentSize;
            realSize = Size;
            sizeChangeRunningSeconds = 0f;
            Debug.Log("Score Change");
            Debug.Log(oldSize + ", " + realSize);
        }

    }


    public float Size
    {
        get
        {
            if (Score == 0)
            {
                return GlobalConfig.Unit.InitSize;
            }

            return (float)System.Math.Log(Score, 10) * 10 + GlobalConfig.Unit.InitSize;
        }
    }



    public float Speed
    {
        get
        {
            return GlobalConfig.Unit.SizeToSpeedParam / currentSize;
        }
    }

    public string Name { get; set; }
    public Protocol.Define.Unit.UnitStatus Status { get; set; }

    private float acc = 1f;
    private float accStart = 0f;
    private float accStartAt = 0f;
    private float accParamMulti = 0f;
    private static readonly float accCap = 50f;

    private float boundSize;
    private float scale;

    private GameObject UI;
    private RectTransform uiTransform;
    private Text uiText;

    void Awake()
    {
        oldSize = Size;
        currentSize = oldSize;
        realSize = oldSize;

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
        // move
        currentTowards = Vector3.Lerp(oldTowards, realTowards, towardsChangeRunningSeconds / towardsChangeTotalSeconds);
        towardsChangeRunningSeconds += Time.deltaTime;
        gameObject.transform.position += currentTowards * Time.deltaTime * Speed * acc;

        UI.transform.position = gameObject.transform.position;


        // size
        currentSize = Mathf.Lerp(oldSize, realSize, sizeChangeRunningSeconds / sizeChangeTotalSeconds);
        sizeChangeRunningSeconds += Time.deltaTime;
        if (Mathf.Abs(currentSize - realSize) > 0.1f)
        {
            scale = currentSize / boundSize;
            gameObject.transform.localScale = new Vector3(scale, scale, 1);
        }

        uiTransform.sizeDelta = new Vector2(currentSize, currentSize);

        uiText.text = "大小:" + System.Math.Round(currentSize, 2) + "速度:" + System.Math.Round(Speed, 2); 


        if (Input.GetKeyDown(KeyCode.Space))
        {
            float speed = Vector3.SqrMagnitude(Towards);
            Debug.Log("Go...");
            accStart = accCap/speed;
            acc = accStart;
            accStartAt = 0f;
            accParamMulti = (acc - 1) / Mathf.Pow(0.5f, 3f);

            Status = Protocol.Define.Unit.UnitStatus.Jump;
        }
    }

    void LateUpdate()
    {
        if (gameObject.tag == "MyPlayerMain")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }

        UI.transform.position = gameObject.transform.position;
    }


    void FixedUpdate()
    {
        if (acc == 1)
        {
            return;
        }

        if (acc - 1 < 0.1f)
        {
            acc = 1;
            return;
        }

        accStartAt += Time.fixedDeltaTime;
        acc = accStart - Mathf.Pow(accStartAt, 3) * accParamMulti;
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

        uiTransform.sizeDelta = new Vector2(currentSize, currentSize);

    }


    public void SetTarget(Vector3 target)
    {
        if (Status != Protocol.Define.Unit.UnitStatus.Jump)
        {
            Vector3 towards = target - gameObject.transform.position;
            towards.Normalize();
            Towards = towards;
            Status = Protocol.Define.Unit.UnitStatus.Move;
        }

    }

    public void Stop()
    {
        if (Status == Protocol.Define.Unit.UnitStatus.Move)
        {
            Towards = new Vector3(0, 0, 0);
            Status = Protocol.Define.Unit.UnitStatus.Idle;
        }
    }

    public void AddScore(float changedValue)
    {
        Score += changedValue;
        EventManger.GetInstance().TrigUnitScoreChange(this);
    }
}
