using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotPool : MonoBehaviour
{

    public GameObject dotPrefab;
    private Queue<GameObject> dots = new Queue<GameObject>();

    void Awake()
    {
        GameManager.DotPoolScript = this;
    }

    // Use this for initialization
    void Start()
    {
        //CreateDots(50);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateDots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject d = (GameObject)Instantiate(dotPrefab);
            SpriteRenderer sp = d.GetComponent<SpriteRenderer>();

            float scale = 2 / sp.sprite.bounds.size.x;
            d.transform.localScale = new Vector3(scale, scale, 1);
            d.SetActive(false);

            dots.Enqueue(d);
        }
    }



    public GameObject Get()
    {
        if (dots.Count == 0)
        {
            CreateDots(50);
        }

        GameObject obj = dots.Dequeue();
        obj.SetActive(true);

        return obj;
    }

    public void Put(GameObject obj)
    {
        obj.SetActive(false);
        dots.Enqueue(obj);
    }
}
