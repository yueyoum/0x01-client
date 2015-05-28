using UnityEngine;
using System.Collections;



public class PlayerScript : MonoBehaviour
{
    public Vector3 Towards { get; set; }
    public float Size { get; set; }
    public string Name { get; set; }

    // Use this for initialization
    void Start()
    {
        if (gameObject.tag == "MyPlayerMain")
        {
            CameraManager.GetInstance().MoveMainCamera(gameObject.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Towards * Time.deltaTime * 10;

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
    }

    public void Stop()
    {
        Towards = new Vector3(0, 0, 0);
    }
}
