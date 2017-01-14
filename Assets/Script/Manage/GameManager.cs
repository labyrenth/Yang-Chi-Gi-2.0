using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    GameObject Planet;
    Text UItext;
    Text UIscore;
    GameObject EndScreen;
    GameObject Player;

    RectTransform Compassbar;

    float PlayerScore;

    public GameObject Enemy;
    public GameObject Sheephorde;
    public GameObject bronzesheepprefab;
    public GameObject silversheepprefab;
    public GameObject goldensheepprefab;
    public GameObject BackGround;
    public RectTransform positiveMarkerPrefab;
    public RectTransform NegativeMarketPrefab;

    public List<GameObject> SheepList;
    public List<GameObject> grassprefab;
    public List<RectTransform> sheepMarker;
    public float PlanetScale;
    public float initialtime;
    public int initialSheep;
    
    void Start()
    {
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.Find("Sheephorde");
        BackGround = GameObject.Find("BackGround");

        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIscore = GameObject.Find("ScoreText").GetComponent<Text>();
        Compassbar = GameObject.Find("Compassbar").GetComponent<RectTransform>();

        EndScreen = GameObject.Find("EndScreen");
        EndScreen.SetActive(false);
        Player = GameObject.Find("PlayerOne");
        
        PlayerScore = 0;

        SheepSpawn(bronzesheepprefab, PlanetScale,initialSheep);
        GrassSpawn(grassprefab, 24.5f, 150);
    }

    void Showremainingtime()
    {
        string timetext;
        if (initialtime - Time.fixedTime >= 0)
        {
            timetext = "Left Time : " + (initialtime - Time.fixedTime).ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            finishgame();
        }

        UItext.text = timetext;
    }

    void ShowMyScore()
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

    void finishgame()
    {
        EndScreen.SetActive(true);
        Player.GetComponent<PlayerControltwo>().IsgameOver = true;
    }

    public void SheepSpawn(GameObject sheepprefab,float scale, int number)   //양을 임의의 위치에 소환하는 메서드.
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 newposition = Random.onUnitSphere * scale;
            GameObject tempSheep = Instantiate(sheepprefab, newposition, Quaternion.Euler(0, 0, 0), Sheephorde.transform);
            RectTransform tempMarker = Instantiate(positiveMarkerPrefab, Compassbar);
            tempSheep.transform.rotation = Quaternion.FromToRotation(tempSheep.transform.up, newposition) * tempSheep.transform.rotation;
            SheepList.Add(tempSheep);
            sheepMarker.Add(tempMarker);
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
        angle = Vector3.Dot(Player.transform.right, PTVector);    //플레이어의 오른쪽 벡터를 기준으로 내적.
        Marker.localPosition = new Vector3(Mathf.Clamp(angle * 15, -300, 300), 0, 0);
    }

    void ShowPositiveObjectInCompassbar(GameObject MainObject, GameObject TargetObject, RectTransform Marker)
    {
        float angle;
        Vector3 PO = MainObject.transform.position + new Vector3(0, 1, 0);
        Vector3 TO = TargetObject.transform.position + new Vector3(0, 1, 0);
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(Player.transform.right, PTVector);    //플레이어의 오른쪽 벡터를 기준으로 내적.
        if (TargetObject.GetComponent<SheepControltwo>().Master == null)
            Marker.localPosition = new Vector3(Mathf.Clamp(angle * 24, -300, 300), 0, 0);
        else
            Marker.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Showremainingtime();
        ShowMyScore();
        ShowNegativeObjectInCompassbar(Player,Enemy, NegativeMarketPrefab);
        for(int i = 0; i<SheepList.Count; i++)
        {
            ShowPositiveObjectInCompassbar(Player, SheepList[i], sheepMarker[i]);
        }
    }
}
