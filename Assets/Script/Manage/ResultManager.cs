using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    float playerScore;
    float enemyScore;
    float EXP;

    Text playerName;
    Text enemyName;
    Text playerresult;
    Text enemyresult;

    GameObject win;
    GameObject lose;
    Text EXPgaintext;
    
	// Use this for initialization
	void Start ()
    {
        StartInit();
        StartCoroutine("ResultRoutine");
	}

    void StartInit()
    {
        playerScore = PlayManage.Instance.PlayerScore;
        enemyScore = PlayManage.Instance.EnemyScore;
        EXP = PlayManage.Instance.EXP;

        playerName = GameObject.Find("PlayerName").GetComponent<Text>();
        enemyName = GameObject.Find("EnemyName").GetComponent<Text>();
        playerresult = GameObject.Find("PlayerScore").GetComponent<Text>();
        enemyresult = GameObject.Find("EnemyScore").GetComponent<Text>();
        win = GameObject.Find("Win");
        lose = GameObject.Find("Lose");
        EXPgaintext = GameObject.Find("EXPText").GetComponent<Text>();

        playerName.text = PlayManage.Instance.playerID;

        playerName.gameObject.SetActive(false);
        enemyName.gameObject.SetActive(false);
        playerresult.gameObject.SetActive(false);
        enemyresult.gameObject.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        EXPgaintext.gameObject.SetActive(false);
    }

    void ShowScore(Text scoretext)
    {

    }

    IEnumerator ResultRoutine()
    {

        yield return 0;
    }

    IEnumerator FadeIn()
    {
        for (float i = 1f; i >= 0; i -= 0.025f)
        {
            Color color = new Vector4(0, 0, 0, i);
            yield return 0;
        }
    }
}
