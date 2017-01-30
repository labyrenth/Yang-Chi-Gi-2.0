using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : ManagerBase {

    GameObject Planet;
    Text UItext;
    Text UIscore;
    Text UIcurrentSheep;
    Text UIEnemyScore;
    GameObject EndScreen;

    GameObject Enemy;
    Button SelectedButton;
    Button SkillFireButton;
    RectTransform Compassbar;

    float SheepCount;
    public float PlayerScore;
    public float EnemyScore;
    public int PlayerNumber;

    public GameObject Player;
    public GameObject Sheephorde;
    public GameObject bronzesheepprefab;
    public GameObject silversheepprefab;
    public GameObject goldensheepprefab;
    public GameObject BackGround;
    public RectTransform positiveMarkerPrefab;
    public RectTransform NegativeMarketPrefab;
    public SkillDataBase SkillDB;
    public CameraControl mainCamera;

    public List<GameObject> SheepList;
    public List<GameButtonEvent> SkillButtonList;
    public List<GameObject> grassprefab;
    public List<RectTransform> sheepMarker;

    public float PlanetScale;
    public float initialtime;
    public int initialSheep;
    
    int index;
    float Timer;
    public float skillcooltimer = 10f;
    bool TimerStart;
    int Skillindexcount;

    public override void Awake()
    {
        base.Awake();
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.Find("Sheephorde");
        BackGround = GameObject.Find("BackGround");

        if (PlayerNumber == 1)
        {
            Player = GameObject.Find("PlayerOne");
            Enemy = GameObject.Find("PlayerTwo");
        }
        else if (PlayerNumber == 2)
        {
            Player = GameObject.Find("PlayerTwo");
            Enemy = GameObject.Find("PlayerOne");
        }

        //UI관련 초기화.
        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIscore = GameObject.Find("ScoreText").GetComponent<Text>();
        UIcurrentSheep = GameObject.Find("CurrentSheepText").GetComponent<Text>();
        UIEnemyScore = GameObject.Find("EnemyScoreText").GetComponent<Text>();
        Compassbar = GameObject.Find("Compassbar").GetComponent<RectTransform>();
        EndScreen = GameObject.Find("EndScreen");
        SkillButtonList.Add(GameObject.Find("SkillButton1").GetComponent<GameButtonEvent>());
        SkillButtonList.Add(GameObject.Find("SkillButton2").GetComponent<GameButtonEvent>());
        SkillFireButton = GameObject.Find("SkillFire").GetComponent<Button>();
        StartCoroutine("ReadyScreen");

        SheepCount = 0;
        Timer = 0;
        TimerStart = false;

        //오브젝트 생성.
        SheepSpawn(bronzesheepprefab, PlanetScale, initialSheep);
        GrassSpawn(grassprefab, 24.5f, 150);
        //스킬 관련 초기화.
        SkillDB = this.gameObject.GetComponent<SkillDataBase>();
        Skillindexcount = 0;
    }

    void Showremainingtime()
    {
        string timetext;
        if (initialtime - Timer >= 0)
        {
            timetext = "Left Time : " + (initialtime - Timer).ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            finishgame();
        }

        UItext.text = timetext;
    }

    void ShowScore()
    {
        string scoretext;
        PlayerScore = Player.GetComponent<PlayerControltwo>().Score;
        if (PlayerScore >= 10)
        {
            scoretext = "My Score : " + PlayerScore;
        }
        else
        {
            scoretext = "My Score : 0" + PlayerScore;
        }
        UIscore.text = scoretext;
    }

    void ShowMySheep()
    {
        string scoretext;
        SheepCount = Player.GetComponent<PlayerControltwo>().SheepCount;
        if (SheepCount >= 10)
        {
            scoretext = "Current Sheep : " + SheepCount;
        }
        else
        {
            scoretext = "Current Sheep : 0" + SheepCount;
        }
        UIcurrentSheep.text = scoretext;
    }

    void ShowEnemyScore()
    {
        string scoretext;
        EnemyScore = Enemy.GetComponent<PlayerControltwo>().Score;
        if (EnemyScore >= 10)
        {
            scoretext = "Enemy Score : " + EnemyScore;
        }
        else
        {
            scoretext = "Enemy Score : 0" + EnemyScore;
        }
        UIEnemyScore.text = scoretext;
    }

    void ShowUIText()
    {
        Showremainingtime();
        ShowScore();
        ShowEnemyScore();
        ShowMySheep();
    }

    void finishgame()
    {
        StartCoroutine("FinishRoutine");
    }

    public void SheepSpawn(GameObject sheepprefab,float scale, int number)   //양을 임의의 위치에 소환하는 메서드.
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 newposition = Random.onUnitSphere * scale;
            if (Vector3.Distance(newposition, Player.transform.position) > 2 && Vector3.Distance(newposition, Enemy.transform.position) > 2)
            {
                GameObject tempSheep = Instantiate(sheepprefab, newposition, Quaternion.Euler(0, 0, 0), Sheephorde.transform);
                RectTransform tempMarker = Instantiate(positiveMarkerPrefab, Compassbar);
                tempSheep.transform.rotation = Quaternion.FromToRotation(tempSheep.transform.up, newposition) * tempSheep.transform.rotation;
                SheepList.Add(tempSheep);
                sheepMarker.Add(tempMarker);
            }
            else
            {
                i--;
            }
        }
    }

    public void GrassSpawn(List<GameObject> grasslist, float scale, int number)
    {
        int listcount = grasslist.Count-1;
        for (int i = 0; i < number; i++)
        {
            int index = Random.Range(0, listcount);
            Vector3 newposition = Random.onUnitSphere * scale;
            GameObject tempgrass = Instantiate(grasslist[index], newposition, Quaternion.Euler(0, 0, 0), BackGround.transform);
            tempgrass.transform.rotation = Quaternion.FromToRotation(tempgrass.transform.up, newposition) * tempgrass.transform.rotation;
        }    
    }

    void ShowNegativeObjectInCompassbar(GameObject MainObject, GameObject TargetObject, RectTransform Marker)
    {
        float angle;
        Vector3 PO = MainObject.transform.position;
        Vector3 TO = TargetObject.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(MainObject.transform.right, PTVector);    //플레이어의 오른쪽 벡터를 기준으로 내적.
        Marker.localPosition = new Vector3(Mathf.Clamp(angle * 15, -250, 250), 0, 0);
    }

    void ShowPositiveObjectInCompassbar(GameObject MainObject, GameObject TargetObject, RectTransform Marker)
    {
        float angle;
        Vector3 PO = MainObject.transform.position + new Vector3(0, 1, 0);
        Vector3 TO = TargetObject.transform.position + new Vector3(0, 1, 0);
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(MainObject.transform.right, PTVector);    //플레이어의 오른쪽 벡터를 기준으로 내적.
        Marker.localPosition = new Vector3(Mathf.Clamp(angle * 24, -250, 250), 0, 0);
    }

    IEnumerator ReadyScreen()
    {
        EndScreen.SetActive(true);
        GameObject Readytext = GameObject.Find("ReadyText");
        GameObject EndText = GameObject.Find("EndText");
        EndText.SetActive(false);
        Player.GetComponent<PlayerControltwo>().IsgameOver = true;
        Enemy.GetComponent<PlayerControltwo>().IsgameOver = true;
        yield return new WaitForSeconds(3);
        Readytext.SetActive(false);
        EndText.SetActive(true);
        EndScreen.SetActive(false);
        Player.GetComponent<PlayerControltwo>().IsgameOver = false;
        Enemy.GetComponent<PlayerControltwo>().IsgameOver = false;
        TimerStart = true;
        yield return 0;
    }

    IEnumerator FinishRoutine()
    {
        EndScreen.SetActive(true);
        Player.GetComponent<PlayerControltwo>().IsgameOver = true;
        Enemy.GetComponent<PlayerControltwo>().IsgameOver = true;
        PlayManage.Instance.PlayerScore = Player.GetComponent<PlayerControltwo>().SheepCount;
        PlayManage.Instance.EnemyScore = Enemy.GetComponent<PlayerControltwo>().SheepCount;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Result");
    }

    void TimerSet()
    {
        if (TimerStart)
        {
            Timer += Time.deltaTime;
            SkillCooltime();
        }
    }

    public void FindAndRemoveAtSheepList(GameObject target)
    {
        index = SheepList.FindIndex(x => x.gameObject == target);
        SheepList.RemoveAt(index);
        sheepMarker[index].gameObject.SetActive(false);
        sheepMarker.RemoveAt(index);
    }

    public void SkillButtonAction(Button targetbutton)
    {
        if (SelectedButton == null)
        {
            SelectedButton = targetbutton;
            SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
        }
        else
        {
            if (SelectedButton == targetbutton)
            {
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
                SelectedButton = null;
            }
            else
            {
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
                SelectedButton = targetbutton;
                SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillButtonActive();
            }
        }
    }

    void SkillCooltime()
    {
        if (TimerStart && skillcooltimer >= 0)
        {
            skillcooltimer -= Time.deltaTime;
        }
        if (skillcooltimer < 0)
        {
            if (!SkillButtonList[0].IsSkillCanActive || !SkillButtonList[1].IsSkillCanActive)
            {
                if (!SkillButtonList[0].IsSkillCanActive)
                {
                    SkillButtonList[0].IsSkillCanActive = true;
                    SkillDB.ButtonIconInRandomList(SkillButtonList[0].gameObject.GetComponent<Button>(), Skillindexcount);
                    CalSkillIndexCount();
                    skillcooltimer = 10f;

                }
                else if (!SkillButtonList[1].IsSkillCanActive)
                {
                    SkillButtonList[1].IsSkillCanActive = true;
                    SkillDB.ButtonIconInRandomList(SkillButtonList[1].gameObject.GetComponent<Button>(), Skillindexcount);
                    CalSkillIndexCount();
                    skillcooltimer = 10f;
                }
            }
        }
    }

    void CalSkillIndexCount()
    {
            if (Skillindexcount < SkillDB.SkillIndexList.Count-1)
                Skillindexcount++;
            else
                Skillindexcount = 0;
    }


    public void SkillFireAction()
    {
        if (SelectedButton.gameObject.GetComponent<GameButtonEvent>() == null || !SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
            return;
        else if(SelectedButton.gameObject.GetComponent<GameButtonEvent>() != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
        {
            SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive = false;
        }
    }//수정필요

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowUIText();
        TimerSet();
        ShowNegativeObjectInCompassbar(mainCamera.gameObject,Enemy, NegativeMarketPrefab);
        for(int i = 0; i<SheepList.Count; i++)
        {
            ShowPositiveObjectInCompassbar(mainCamera.gameObject, SheepList[i], sheepMarker[i]);
        }
        if (SelectedButton == null)
        {
            SkillFireButton.gameObject.SetActive(false);
        }
        else
        {
            SkillFireButton.gameObject.SetActive(true);
        }
    }
}
