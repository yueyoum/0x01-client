using UnityEngine;
using System.Collections.Generic;

public class DotBuffer : MonoBehaviour
{
    private float dotRemoveReportInterval;
    private float runningTime;
    private Dictionary<PlayerScript, float> playerScirptAddScoreDict;
    private List<string> dotIds;

    void Awake()
    {
        dotRemoveReportInterval = 0.25f;
        runningTime = 0f;
        playerScirptAddScoreDict = new Dictionary<PlayerScript, float>();
        dotIds = new List<string>();

        GameManager.DotBufferScript = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        runningTime += Time.deltaTime;
        if(runningTime >= dotRemoveReportInterval)
        {
            runningTime = 0f;
            if(dotIds.Count > 0)
            {
                Protocol.Define.DotRemove msg = new Protocol.Define.DotRemove();
                msg.ids.AddRange(dotIds);
                byte[] data = Protocol.ProtocolHandler.PackWithId(msg);
                Transport.GetInstance().Send(data);

                dotIds.Clear();
                playerScirptAddScoreDict.Clear();
            }
        }
    }

    public void DotRemove(PlayerScript ps, string id)
    {
        if (playerScirptAddScoreDict.ContainsKey(ps))
        {
            playerScirptAddScoreDict[ps] += GlobalConfig.Dot.ScoreValue;
        }
        else
        {
            playerScirptAddScoreDict.Add(ps, GlobalConfig.Dot.ScoreValue);
        }

        dotIds.Add(id);
    }
}
