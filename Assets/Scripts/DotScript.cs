using UnityEngine;
using System.Collections;

public class DotScript : MonoBehaviour
{
    // Use this for initialization
    private int speed;
    private GameObject target;

    public string Id { get; set; }

    void Start()
    {
        speed = Random.Range(50, 100);
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        if (target != null)
        {

            transform.position = Vector3.Lerp(gameObject.transform.position, target.transform.position, Time.deltaTime * 20);
            if (target.layer == 9)
            {
                // MyUnits
                if (Vector3.Distance(transform.position, target.transform.position) <= 1f)
                {
                    GameManager.DotBufferScript.DotRemove(target.GetComponent<PlayerScript>(), Id);

                    target = null;
                    MapManager.GetInstance().DotsRemove(Id);
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null)
        {
            return;
        }

        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            // MyUnits, OtherUnits
            target = other.gameObject;
        }
    }
}


