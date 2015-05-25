using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPool : MonoBehaviour
{

    private Queue<PlayerUnit> players = new Queue<PlayerUnit>();

    void Awake()
    {
        GameManager.PlayerPoolScript = this;
    }

    // Use this for initialization
    void Start()
    {
        CreatePlayers(10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlayers(int amount)
    {
        for(int i=0; i<amount; i++)
        {
            GameObject p = (GameObject)Instantiate(GameManager.RootScript.PlayerPrefab);
            SpriteRenderer sp = p.GetComponent<SpriteRenderer>();

            float scale = PlayerManager.InitSize / sp.sprite.bounds.size.x;
            p.transform.localScale = new Vector3(scale, scale, 1);

            PlayerScript s = p.GetComponent<PlayerScript>();

            p.SetActive(false);

            PlayerUnit pu = new PlayerUnit(p, s);

            players.Enqueue(pu);
        }
    }

    public PlayerUnit Get()
    {
        return Get("Untagged");
    }

    public PlayerUnit Get(string tagName)
    {
        if(players.Count == 0)
        {
            CreatePlayers(10);
        }

        PlayerUnit pu = players.Dequeue();
        pu.Player.tag = tagName;
        pu.Player.SetActive(true);
        return pu;
    }

    public void Put(PlayerUnit pu)
    {
        pu.Player.tag = "";
        pu.Player.SetActive(false);
        players.Enqueue(pu);
    }
}
