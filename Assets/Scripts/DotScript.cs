using UnityEngine;
using System.Collections;

public class DotScript : MonoBehaviour
{
    // Use this for initialization
    private int speed;

    void Start()
    {
        speed = Random.Range(50, 150);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}


