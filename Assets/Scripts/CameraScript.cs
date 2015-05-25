using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float ResizeSpeed { get; set; }

    private float size;
    private Camera camera_component = null;

    // Use this for initialization
    void Start()
    {
        camera_component = gameObject.GetComponent<Camera>();
        size = camera_component.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
         if(Mathf.Abs(camera_component.orthographicSize - size) > 0.01f)
         {
             camera_component.orthographicSize = Mathf.Lerp(camera_component.orthographicSize, size, Time.deltaTime * ResizeSpeed);
         }
    }


    public void ReSize(float targetSize)
    {
        size = targetSize;
    }
}
