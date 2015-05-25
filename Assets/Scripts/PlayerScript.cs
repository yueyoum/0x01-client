using UnityEngine;
using System.Collections;


public enum PlayerStatus
{
    STOP,
    RUNNING,
}


public class PlayerScript : MonoBehaviour
{
    public Vector3 Towards { get; set; }
    public float Size { get; set; }
    private PlayerStatus Status { get; set; }

    // Use this for initialization
    void Start()
    {
        Status = PlayerStatus.STOP;
        if (gameObject.tag == "MyPlayerMain")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Status == PlayerStatus.RUNNING)
        {
            gameObject.transform.position += Towards * Time.deltaTime * 16;
        }

        if (gameObject.tag == "MyPlayerMain")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger: " + other.gameObject.name);
    }


    public void SetTarget(Vector3 targetPosition)
    {
        Vector3 towards = targetPosition - gameObject.transform.position;
        towards.Normalize();
        Towards = towards;

        Status = PlayerStatus.RUNNING;
    }

    public void Stop()
    {
        Status = PlayerStatus.STOP;
    }
}
