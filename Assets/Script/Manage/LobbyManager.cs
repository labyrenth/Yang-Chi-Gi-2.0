using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : ManagerBase {

    public Text playerleveltext;
    public Text playerIDtext;
    public Text playerEXP;
    public GameObject clientObject;

    int level;
    float EXP;
    float maxEXP;

    private void Start()
    {
        LobbyInit();
        CalEXP();
        
    }

    void LobbyInit()
    {
        playerleveltext = GameObject.Find("PlayerLevel").GetComponent<Text>();
        playerIDtext = GameObject.Find("PlayerID").GetComponent<Text>();
        playerEXP = GameObject.Find("PlayerExp").GetComponent<Text>();
        level = PlayManage.Instance.playerlevel;
        EXP = PlayManage.Instance.EXP;
        if (level < 10)
        {
            playerleveltext.text = "Level : 0" + level.ToString();
        }
        else
        {
            playerleveltext.text = "Level : " + level.ToString();
        }
        playerIDtext.text = PlayManage.Instance.playerID;
        maxEXP = level * 1000;
        playerEXP.text = "EXP : " + PlayManage.Instance.EXP.ToString("N0") + " / " + maxEXP.ToString("N0");
    }

    void CalEXP()
    {
        if (EXP >= maxEXP)
        {
            EXP -= maxEXP;
            level += 1;
            PlayManage.Instance.playerlevel = this.level;
            PlayManage.Instance.EXP = this.EXP;
            LobbyInit();
        }
    }
}
