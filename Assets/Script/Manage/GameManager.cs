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

    public GameObject Enemy;
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
    public HQControl HQ;
    public List<GameObject> SheepList;
    public List<GameButtonEvent> SkillButtonList;
    public List<GameObject> grassprefab;
    public List<RectTransform> sheepMarker;

    public float PlanetScale;
    public float initialtime;
    public int initialSheep;
    
    int index;
    float Timer;
    float skillcooltimer = 10f;
    public bool TimerStart;
    int Skillindexcount;

    public bool check;

    public GameObject ActivatedSkillPrefab;

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

    private void Start()
    {
        HQ = Player.GetComponent<PlayerControlThree>().HQ.GetComponent<HQControl>();
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
        PlayerScore = Player.GetComponent<PlayerControlThree>().Score;
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
        SheepCount = Player.GetComponent<PlayerControlThree>().SheepCount;
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
        EnemyScore = Enemy.GetComponent<PlayerControlThree>().Score;
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
        Player.GetComponent<PlayerControlThree>().IsgameOver = true;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = true;
        yield return new WaitForSeconds(3);
        Readytext.SetActive(false);
        EndText.SetActive(true);
        EndScreen.SetActive(false);
        Player.GetComponent<PlayerControlThree>().IsgameOver = false;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = false;
        TimerStart = true;
        yield return 0;
    }

    IEnumerator FinishRoutine()
    {
        EndScreen.SetActive(true);
        Player.GetComponent<PlayerControlThree>().IsgameOver = true;
        Enemy.GetComponent<PlayerControlThree>().IsgameOver = true;
        PlayManage.Instance.PlayerScore = Player.GetComponent<PlayerControlThree>().SheepCount;
        PlayManage.Instance.EnemyScore = Enemy.GetComponent<PlayerControlThree>().SheepCount;
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

    void CheckCameraFix()
    {
        if (SelectedButton != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillIndex != 0 && SkillDB.SkillPrefab[SelectedButton.gameObject.GetComponent<GameButtonEvent>().SkillIndex].GetComponentInChildren<SkillBase>().FindSkillNeedCameraFix())
        {
            mainCamera.IsSkillCutScene = true;
            HQ.Arrow.gameObject.SetActive(true);
        }
        else
        {
            mainCamera.IsSkillCutScene = false;
            HQ.Arrow.gameObject.SetActive(false);
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
                    CalSkillIndexCount(SkillButtonList[0]);
                    skillcooltimer = 10f;

                }
                else if (!SkillButtonList[1].IsSkillCanActive)
                {
                    SkillButtonList[1].IsSkillCanActive = true;
                    SkillDB.ButtonIconInRandomList(SkillButtonList[1].gameObject.GetComponent<Button>(), Skillindexcount);
                    CalSkillIndexCount(SkillButtonList[1]);
                    skillcooltimer = 10f;
                }
            }
        }
    }

    void CalSkillIndexCount(GameButtonEvent targetButton)
    {
        targetButton.SkillIndex = SkillDB.SkillIndexList[Skillindexcount];
        if (Skillindexcount < SkillDB.SkillIndexList.Count - 1)
        {
            Skillindexcount++;
        }
        else
            Skillindexcount = 0;
    }

    public void SkillFireAction()
    {
        if (SelectedButton.gameObject.GetComponent<GameButtonEvent>() == null || !SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
            return;
        else if (SelectedButton.gameObject.GetComponent<GameButtonEvent>() != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
        {
            GameButtonEvent GBEtemp = SelectedButton.gameObject.GetComponent<GameButtonEvent>();
            check = false;
            SelectedButton.image.sprite = SkillDB.SkillIcon[0];
            GBEtemp.IsSkillCanActive = false;
            ActivatedSkillPrefab = Instantiate(SkillDB.SkillPrefab[GBEtemp.SkillIndex]);
            GBEtemp.SkillIndex = 0;

            check = ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SetInstance(this.Player, this.Enemy);

            if (HQ.Arrow.gameObject.activeSelf)
            {
                ActivatedSkillPrefab.transform.rotation = this.HQ.ArrowPivot.rotation;
                HQ.Arrow.gameObject.SetActive(false);
            }
            else
                ActivatedSkillPrefab.transform.rotation = this.HQ.gameObject.transform.rotation;

            if (check)
                ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SS = SkillState.LAUNCHED;
            else
            {
                while (check)
                {
                    check = ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SetInstance(this.Player, this.Enemy);
                }
                ActivatedSkillPrefab.GetComponentInChildren<SkillBase>().SS = SkillState.LAUNCHED;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ShowUIText();
        TimerSet();
        ShowNegativeObjectInCompassbar(mainCamera.gameObject,Enemy, NegativeMarketPrefab);
        CheckCameraFix();
        for(int i = 0; i<SheepList.Count; i++)
        {
            ShowPositiveObjectInCompassbar(mainCamera.gameObject, SheepList[i], sheepMarker[i]);
        }

        if (SelectedButton != null && SelectedButton.gameObject.GetComponent<GameButtonEvent>().IsSkillCanActive)
        {
            SkillFireButton.gameObject.SetActive(true);
        }
        else
        {
            SkillFireButton.gameObject.SetActive(false);
        }
    }
}